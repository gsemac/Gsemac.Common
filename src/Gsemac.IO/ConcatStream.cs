using System;
using System.IO;

namespace Gsemac.IO {

    public sealed class ConcatStream :
        Stream {

        // Public members

        public override bool CanRead => stream1.CanRead && stream2.CanRead;
        public override bool CanSeek => stream1.CanSeek && stream2.CanSeek;
        public override bool CanWrite => stream1.CanWrite && stream2.CanWrite;
        public override long Length => GetLength();
        public override long Position {
            get => GetPosition();
            set => SetPosition(value);
        }

        public ConcatStream(Stream stream1, Stream stream2) :
            this(stream1, stream2, closeStreams: false) {
        }
        public ConcatStream(Stream stream1, Stream stream2, bool closeStreams) {

            if (stream1 is null)
                throw new ArgumentNullException(nameof(stream1));

            if (stream2 is null)
                throw new ArgumentNullException(nameof(stream2));

            this.stream1 = stream1;
            this.stream2 = stream2;
            this.closeStreams = closeStreams;

            if (stream2.CanSeek)
                stream2StartPos = stream2.Position;

        }

        public override long Seek(long offset, SeekOrigin origin) {

            long position;

            switch (origin) {

                case SeekOrigin.Begin:
                    position = offset;
                    break;

                case SeekOrigin.Current:
                    position = GetPosition() + offset;
                    break;

                case SeekOrigin.End:
                    position = Length + offset;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(origin));

            }

            return SetPosition(position);

        }
        public override void SetLength(long value) {

            if (value >= stream1.Length) {

                stream2.SetLength(value - stream1.Length);

            }
            else {

                stream1.SetLength(value);
                stream2.SetLength(0);

            }

        }
        public override void Flush() {

            stream1.Flush();
            stream2.Flush();

        }

        public override int Read(byte[] buffer, int offset, int count) {

            if (buffer is null)
                throw new ArgumentNullException(nameof(buffer));

            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), Core.Properties.ExceptionMessages.NonNegativeNumberRequired);

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), Core.Properties.ExceptionMessages.NonNegativeNumberRequired);

            int bytesRead = stream1.Read(buffer, offset, count);

            count -= bytesRead;

            if (count > 0)
                bytesRead += stream2.Read(buffer, offset + bytesRead, count);

            return bytesRead;

        }
        public override void Write(byte[] buffer, int offset, int count) {

            if (buffer is null)
                throw new ArgumentNullException(nameof(buffer));

            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), Core.Properties.ExceptionMessages.NonNegativeNumberRequired);

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), Core.Properties.ExceptionMessages.NonNegativeNumberRequired);

            long bytesLeftInStream1 = stream1.Length - stream1.Position;
            long bytesWritten = Math.Min(count, bytesLeftInStream1);

            stream1.Write(buffer, offset, (int)bytesWritten);

            if (bytesWritten < count)
                stream2.Write(buffer, offset + (int)bytesWritten, count - (int)bytesWritten);

        }

        public override void Close() {

            if (closeStreams) {

                stream1.Close();
                stream2.Close();

            }

            base.Close();

        }

        // Protected members

        protected override void Dispose(bool disposing) {

            if (disposing && closeStreams) {

                stream1.Dispose();
                stream2.Dispose();

            }

            base.Dispose(disposing);

        }

        // Private members

        private readonly Stream stream1;
        private readonly Stream stream2;
        private readonly long stream2StartPos = 0;
        private readonly bool closeStreams = false;

        private long GetLength() {

            return stream1.Length + (stream2.Length - stream2StartPos);

        }
        private long GetPosition() {

            if (stream1.Position < stream1.Length)
                return stream1.Position;

            return stream1.Length + (stream2.Position - stream2StartPos);

        }
        private long SetPosition(long position) {

            if (position <= stream1.Length) {

                if (stream2.Position > stream2StartPos)
                    stream2.Seek(stream2StartPos, SeekOrigin.Begin);

                stream1.Seek(position, SeekOrigin.Begin);

            }
            else {

                if (stream1.Position < stream1.Length)
                    stream1.Seek(0, SeekOrigin.End);

                stream2.Seek(position - stream1.Length + stream2StartPos, SeekOrigin.Begin);

            }

            return Position;

        }

    }

}