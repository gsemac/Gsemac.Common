using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        public override bool CanWrite => options.RedirectStandardInput;
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
        /// Returns true if the process has exited.
        /// </summary>
        public bool ProcessExited => HasProcessExited();
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
        public ProcessStream(string filename, IProcessStreamOptions options = null) :
            this(filename, string.Empty, options ?? ProcessStreamOptions.Default) {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessStream"/> class.
        /// </summary>
        /// <param name="filename">File path to the process.</param>
        /// <param name="arguments">Arguments to pass to the process.</param>
        /// <param name="options">Output flags.</param>
        public ProcessStream(string filename, string arguments, IProcessStreamOptions options = null) :
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
        public ProcessStream(ProcessStartInfo startInfo, IProcessStreamOptions options = null) {

            this.options = options;

            // Start the process immediately.

            StartProcess(startInfo);

        }

        /// <summary>
        /// Flushes buffered writes to Standard Input. 
        /// </summary>
        public override void Flush() {

            if (HasProcessStarted() && !HasProcessExited() && stdinStream != null) {

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
            int bytesRead = 0;

            if (stream is object) {

                // Flush before reading in case the user wrote anything for the process to respond to.

                Flush();

                bytesRead = stream.Read(buffer, offset, count);

                // If we didn't read any data, it's possible that the process just hasn't written any yet.
                // We'll block until we get data or the timeout is reached.

                while (bytesRead <= 0 && Blocking && readerTasksWritingToStdOut > 0) {

                    if (!readerTaskMutex.WaitOne(ReadTimeout))
                        throw new TimeoutException(Properties.ExceptionMessages.ReadOperationTimedOut);

                    bytesRead = stream.Read(buffer, offset, count);

                }

            }

            return bytesRead;

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

            if (cancellationTokenSource != null)
                cancellationTokenSource.Dispose();

            if (readerTaskMutex != null)
                readerTaskMutex.Dispose();

            process = null;
            stdoutStream = null;
            stderrStream = null;
            stdinStream = null;
            cancellationTokenSource = null;
            readerTaskMutex = null;

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
        private bool processHasStarted = false;
        private int exitCode = 0;
        private readonly IProcessStreamOptions options = ProcessStreamOptions.Default;
        private ConcurrentMemoryStream stdoutStream;
        private ConcurrentMemoryStream stderrStream;
        private ConcurrentMemoryStream stdinStream;

        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private AutoResetEvent readerTaskMutex = new AutoResetEvent(false);
        private readonly List<Task> readerTasks = new List<Task>();
        private volatile int readerTasksWritingToStdOut = 0;

        private bool GetCanRead() {

            // Consider the stream to be readable if we are able to read from stdin.

            return options.RedirectStandardOutput ||
                (options.RedirectStandardError && options.RedirectStandardErrorToStandardOutput);

        }

        private void CreateProcess(ProcessStartInfo startInfo) {

            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            startInfo.RedirectStandardOutput = options.RedirectStandardInput;
            startInfo.RedirectStandardError = options.RedirectStandardError;
            startInfo.RedirectStandardInput = options.RedirectStandardInput;

            if (options.RedirectStandardOutput || options.RedirectStandardErrorToStandardOutput)
                stdoutStream = new ConcurrentMemoryStream();

            if (options.RedirectStandardError)
                stderrStream = new ConcurrentMemoryStream();

            if (options.RedirectStandardInput)
                stdinStream = new ConcurrentMemoryStream();

            if (process is null)
                process = new Process { StartInfo = startInfo };

            process.EnableRaisingEvents = true;

        }
        private void StartProcess(ProcessStartInfo startInfo) {

            if (!HasProcessStarted()) {

                processHasStarted = true;

                CreateProcess(startInfo ?? new ProcessStartInfo());

                process.Start();

                // Begin asynchronously reading from both output streams.

                if (options.RedirectStandardOutput)
                    StartStandardOutputReaderTask();

                if (options.RedirectStandardError)
                    StartStandardErrorReaderTask();

                if (options.RedirectStandardOutput || options.RedirectStandardError) {

                    // Wait for at least one of the reader tasks to begin reading.
                    // We need to do this before any calls to Read, or else tasksUsingStdOutStream <= 0 and the read will unblock and think there's nothing left.

                    DateTimeOffset startTime = DateTimeOffset.Now;
                    TimeSpan timeout = TimeSpan.FromSeconds(5);

                    while (readerTasksWritingToStdOut <= 0 && !readerTasks.All(task => task.IsCompleted) && (DateTimeOffset.Now - startTime) < timeout)
                        Thread.Sleep(TimeSpan.FromMilliseconds(100));

                }

            }

        }
        private void AbortProcess(bool waitForReaderThreadsToExit) {

            CancelReaderThreadsAndWait(waitForReaderThreadsToExit);

        }

        private bool HasProcessStarted() {

            return processHasStarted;

        }
        private bool HasProcessExited() {

            return process?.HasExited ?? true;

        }

        private Task StartStandardOutputReaderTask() {

            CancellationToken cancellationToken = cancellationTokenSource.Token;
            Stream inputStream = process.StandardOutput.BaseStream;
            Stream outputStream = stdoutStream;

            Task task = Task.Factory.StartNew(() => {

                Interlocked.Increment(ref readerTasksWritingToStdOut);

                int bytesRead = 0;
                byte[] buffer = new byte[bufferSize];

                do {

                    bytesRead = inputStream.Read(buffer, 0, buffer.Length);

                    if (bytesRead > 0) {

                        outputStream.Write(buffer, 0, bytesRead);

                        readerTaskMutex.Set();

                    }

                } while (bytesRead > 0 && !cancellationToken.IsCancellationRequested && process is object);

                // Unblock read if we were the only one still using the stdout stream.

                Interlocked.Decrement(ref readerTasksWritingToStdOut);

                if (readerTasksWritingToStdOut <= 0)
                    readerTaskMutex.Set();

            }, cancellationToken);

            readerTasks.Add(task);

            return task;

        }
        private Task StartStandardErrorReaderTask() {

            CancellationToken cancellationToken = cancellationTokenSource.Token;

            bool redirectingStdErrToStdOut = options.RedirectStandardErrorToStandardOutput;
            bool redirectingStdErrOnly = !options.RedirectStandardOutput && options.RedirectStandardError;
            bool writingToStdOut = redirectingStdErrToStdOut | redirectingStdErrOnly;

            Stream inputStream = process.StandardError.BaseStream;
            Stream outputStream = stderrStream;

            Task task = Task.Factory.StartNew(() => {

                if (writingToStdOut) {

                    Interlocked.Increment(ref readerTasksWritingToStdOut);

                    outputStream = stdoutStream;

                }

                // Continously read from the stream until the task is cancelled or there's nothing left to read.

                int bytesRead = 0;
                byte[] buffer = new byte[bufferSize];

                do {

                    bytesRead = inputStream.Read(buffer, 0, buffer.Length);

                    if (bytesRead > 0) {

                        outputStream.Write(buffer, 0, bytesRead);

                        if (writingToStdOut)
                            readerTaskMutex.Set();

                    }

                } while (bytesRead > 0 && !cancellationToken.IsCancellationRequested && process is object);

                if (writingToStdOut) {

                    // If we were writing to the stdout stream and were the only one still doing so, we need to unblock read.

                    Interlocked.Decrement(ref readerTasksWritingToStdOut);

                    if (readerTasksWritingToStdOut <= 0)
                        readerTaskMutex.Set();

                }

            }, cancellationToken);

            return task;

        }

        private void CancelReaderThreadsAndWait(bool waitForReaderThreadsToExit) {

            // Cancel all tasks.

            if (!waitForReaderThreadsToExit && cancellationTokenSource != null)
                cancellationTokenSource.Cancel();

            // Kill the process if it hasn't exited yet (so any tasks blocked on read unblock).

            if (!ProcessExited)
                process.Kill();

            if (ProcessExited && process != null)
                ExitCode = process.ExitCode;

            // Wait for all tasks to exit.

            foreach (Task task in readerTasks)
                task.Wait();

            readerTasks.Clear();

        }

    }

}