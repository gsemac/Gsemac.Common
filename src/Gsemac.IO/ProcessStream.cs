using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Gsemac.IO {

    /// <summary>
    /// Provides a stream-base interface for interacting with a process.
    /// </summary>
    public class ProcessStream :
        Stream {

        // Public members

        public override bool CanRead => GetCanRead();
        public override bool CanSeek => false;
        public override bool CanWrite => options.HasFlag(ProcessStreamOptions.RedirectStandardInput);
        public override bool CanTimeout => true;
        public override long Length => throw new NotSupportedException(Properties.ExceptionMessages.StreamDoesNotSupportSeeking);
        public override long Position {
            get => throw new NotSupportedException(Properties.ExceptionMessages.StreamDoesNotSupportSeeking);
            set => throw new NotSupportedException(Properties.ExceptionMessages.StreamDoesNotSupportSeeking);
        }
        public override int ReadTimeout { get; set; } = Timeout.Infinite;
        public override int WriteTimeout { get; set; } = Timeout.Infinite;

        /// <summary>
        /// If set to true, reads will block until data is available.
        /// </summary>
        public bool Blocking { get; set; } = true;
        /// <summary>
        /// Returns true if the process has been started.
        /// </summary>
        public bool ProcessStarted { get; private set; } = false;
        /// <summary>
        /// Returns true if the process has exited.
        /// </summary>
        public bool ProcessExited => process is null || process.HasExited;
        /// <summary>
        /// Returns the exit code of the process.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Process has not exited.
        /// </exception>
        public int ExitCode {
            get {

                if (!ProcessExited)
                    throw new InvalidOperationException("Process was null or has not yet exited.");

                return exitCode;

            }
            private set => exitCode = value;
        }
        /// <summary>
        /// Returns a stream containing output from Standard Error.
        /// </summary>
        /// <exception cref="Exception">
        /// Standard Error is not being redirected.
        /// </exception>
        public Stream StandardError {
            get {

                if (stderrStream is null)
                    throw new Exception("Output from standard error is not being captured in this stream.");

                return stderrStream;

            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessStream"/> class.
        /// </summary>
        /// <param name="filename">File path to the process.</param>
        /// <param name="options">Output flags.</param>
        public ProcessStream(string filename, ProcessStreamOptions options = ProcessStreamOptions.Default) :
            this(filename, string.Empty, options) {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessStream"/> class.
        /// </summary>
        /// <param name="filename">File path to the process.</param>
        /// <param name="arguments">Arguments to pass to the process.</param>
        /// <param name="options">Output flags.</param>
        public ProcessStream(string filename, string arguments, ProcessStreamOptions options = ProcessStreamOptions.Default) :
            this(new ProcessStartInfo {
                FileName = filename,
                Arguments = arguments
            }, options) {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessStream"/> class.
        /// </summary>
        /// <param name="startInfo"><see cref="ProcessStartInfo"/> object used to start the process.</param>
        /// <param name="options">Output flags.</param>
        public ProcessStream(ProcessStartInfo startInfo, ProcessStreamOptions options = ProcessStreamOptions.Default) {

            this.options = options;

            // Start the process immediately.

            StartProcess(startInfo);

        }

        /// <summary>
        /// Flushes buffered writes to Standard Input. 
        /// </summary>
        public override void Flush() {

            if (ProcessStarted && !ProcessExited && stdinStream != null) {

                void performWrite() {

                    stdinStream.CopyTo(process.StandardInput.BaseStream);
                    process.StandardInput.BaseStream.Flush();

                }

                if (WriteTimeout == Timeout.Infinite)
                    performWrite();
                else
                    using (Task task = Task.Factory.StartNew(performWrite))
                        if (!task.Wait(WriteTimeout))
                            throw new IOException("Write operation timed out.");

            }

        }
        public override int Read(byte[] buffer, int offset, int count) {

            Stream stream = stdoutStream;

            if (stream != null) {

                // Flush before reading in case the user wrote anything for the process to respond to.
                Flush();

                int bytesRead = stream.Read(buffer, offset, count);

                // If we didn't read any data, it's possible that the process just hasn't written any yet.
                // We'll block until we get data or the timeout is reached.

                while (bytesRead <= 0 && Blocking && Thread.VolatileRead(ref tasksUsingStdOutStream) > 0) {

                    if (!readerLock.WaitOne(ReadTimeout))
                        throw new IOException("Read operation timed out.");

                    bytesRead = stream.Read(buffer, offset, count);

                }

                return bytesRead;

            }
            else
                return 0;

        }
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException(Properties.ExceptionMessages.StreamDoesNotSupportSeeking);
        public override void SetLength(long value) => throw new NotSupportedException(Properties.ExceptionMessages.StreamDoesNotSupportThisOperation);
        public override void Write(byte[] buffer, int offset, int count) {

            if (!CanWrite)
                throw new NotSupportedException(Properties.ExceptionMessages.StreamDoesNotSupportWriting);

            // Write to standard input.

            if (stdinStream != null)
                stdinStream.Write(buffer, offset, count);

        }
        public override void Close() {

            // Flush pending writes to the process.

            Flush();

            // When explicitly closed, wait for reader tasks to finish.

            AbortProcess(waitForReaderThreadsToExit: true);

            if (process != null) {

                if (ProcessExited)
                    ExitCode = process.ExitCode;

                process.Dispose();

            }

            if (stdoutStream != null)
                stdoutStream.Dispose();

            if (stderrStream != null)
                stderrStream.Dispose();

            if (stdinStream != null)
                stdinStream.Dispose();

            if (cancelTokenSource != null)
                cancelTokenSource.Dispose();

            if (readerLock != null)
                readerLock.Dispose();

            process = null;
            stdoutStream = null;
            stderrStream = null;
            stdinStream = null;
            cancelTokenSource = null;
            readerLock = null;

        }

        // Protected members

        protected override void Dispose(bool disposing) {

            if (disposing) {

                // Abort before calling Close() so we don't wait for reader tasks to finish.
                AbortProcess(false);

                Close();

            }

            base.Dispose(disposing);

        }

        // Private members

        private const int bufferSize = 4096;

        private Process process;
        private int exitCode = 0;
        private readonly ProcessStreamOptions options = ProcessStreamOptions.Default;
        private ConcurrentMemoryStream stdoutStream;
        private ConcurrentMemoryStream stderrStream;
        private ConcurrentMemoryStream stdinStream;

        private CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        private AutoResetEvent readerLock = new AutoResetEvent(false);
        private readonly List<Task> tasks = new List<Task>();
        private int tasksUsingStdOutStream = 0;

        private bool GetCanRead() {

            // Consider the stream to be readable if we are able to read from stdin.

            return options.HasFlag(ProcessStreamOptions.RedirectStandardOutput) ||
                (options.HasFlag(ProcessStreamOptions.RedirectStandardError) && options.HasFlag(ProcessStreamOptions.RedirectStandardErrorToStandardOutput));

        }

        private void CreateProcess(ProcessStartInfo startInfo) {

            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            if (options.HasFlag(ProcessStreamOptions.RedirectStandardOutput))
                startInfo.RedirectStandardOutput = true;

            if (options.HasFlag(ProcessStreamOptions.RedirectStandardError))
                startInfo.RedirectStandardError = true;

            if (options.HasFlag(ProcessStreamOptions.RedirectStandardInput))
                startInfo.RedirectStandardInput = true;

            if (options.HasFlag(ProcessStreamOptions.RedirectStandardOutput) || options.HasFlag(ProcessStreamOptions.RedirectStandardErrorToStandardOutput))
                stdoutStream = new ConcurrentMemoryStream();

            if (options.HasFlag(ProcessStreamOptions.RedirectStandardError))
                stderrStream = new ConcurrentMemoryStream();

            if (options.HasFlag(ProcessStreamOptions.RedirectStandardInput))
                stdinStream = new ConcurrentMemoryStream();

            if (process is null)
                process = new Process { StartInfo = startInfo };

            process.EnableRaisingEvents = true;

        }
        private void StartProcess(ProcessStartInfo startInfo) {

            if (!ProcessStarted) {

                ProcessStarted = true;

                CreateProcess(startInfo ?? new ProcessStartInfo());

                process.Start();

                // Begin asynchronously reading from both output streams.

                if (options.HasFlag(ProcessStreamOptions.RedirectStandardOutput))
                    StartStandardOutputReaderTask();

                if (options.HasFlag(ProcessStreamOptions.RedirectStandardError))
                    StartStandardErrorReaderTask();

            }

        }
        private void AbortProcess(bool waitForReaderThreadsToExit) {

            CancelReaderThreadsAndWait(waitForReaderThreadsToExit);

        }
        private void CancelReaderThreadsAndWait(bool waitForReaderThreadsToExit) {

            // Cancel all tasks.

            if (!waitForReaderThreadsToExit && cancelTokenSource != null)
                cancelTokenSource.Cancel();

            // Kill the process if it hasn't exited yet (so any tasks blocked on read unblock).

            if (!ProcessExited)
                process.Kill();

            if (ProcessExited && process != null)
                ExitCode = process.ExitCode;

            // Wait for all tasks to exit.

            foreach (Task task in tasks)
                task.Wait();

            tasks.Clear();

        }

        private void StartStandardOutputReaderTask() {

            CancellationToken token = cancelTokenSource.Token;

            tasks.Add(Task.Factory.StartNew(() => {

                Stream inStream = process.StandardOutput.BaseStream;
                Stream outStream = stdoutStream;

                Interlocked.Increment(ref this.tasksUsingStdOutStream);

                int bytesRead = 0;
                byte[] buffer = new byte[bufferSize];

                do {

                    bytesRead = inStream.Read(buffer, 0, buffer.Length);

                    if (bytesRead > 0) {

                        outStream.Write(buffer, 0, bytesRead);

                        readerLock.Set();

                    }

                } while (bytesRead > 0 && !token.IsCancellationRequested && process != null);

                // Unblock read if we were the only one still using the stdout stream.

                Interlocked.Decrement(ref this.tasksUsingStdOutStream);

                int tasksUsingStdOutStream = Thread.VolatileRead(ref this.tasksUsingStdOutStream);

                if (tasksUsingStdOutStream <= 0)
                    readerLock.Set();

            }));

        }
        private void StartStandardErrorReaderTask() {

            CancellationToken token = cancelTokenSource.Token;

            tasks.Add(Task.Factory.StartNew(() => {

                bool redirectingToStdOutStream = options.HasFlag(ProcessStreamOptions.RedirectStandardErrorToStandardOutput);
                bool readingStdErrOnly = !options.HasFlag(ProcessStreamOptions.RedirectStandardOutput) && options.HasFlag(ProcessStreamOptions.RedirectStandardError);
                bool usingStdOutStream = redirectingToStdOutStream | readingStdErrOnly;

                Stream inStream = process.StandardError.BaseStream;
                Stream outStream = stderrStream;

                if (usingStdOutStream) {

                    Interlocked.Increment(ref tasksUsingStdOutStream);

                    outStream = stdoutStream;

                }

                // Continously read from the stream until the task is cancelled or there's nothing left to read.

                int bytesRead = 0;
                byte[] buffer = new byte[bufferSize];

                do {

                    bytesRead = inStream.Read(buffer, 0, buffer.Length);

                    if (bytesRead > 0) {

                        outStream.Write(buffer, 0, bytesRead);

                        if (usingStdOutStream)
                            readerLock.Set();

                    }

                } while (bytesRead > 0 && !token.IsCancellationRequested && process != null);

                if (usingStdOutStream) {

                    // If we were writing to the stdout stream and were the only one still doing so, we need to unblock read.

                    Interlocked.Decrement(ref this.tasksUsingStdOutStream);

                    int tasksUsingStdOutStream = Thread.VolatileRead(ref this.tasksUsingStdOutStream);

                    if (tasksUsingStdOutStream <= 0)
                        readerLock.Set();

                }

            }));

        }

    }

}