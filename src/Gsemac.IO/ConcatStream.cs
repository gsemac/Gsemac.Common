﻿using System;
using System.IO;

namespace Gsemac.IO {

    public sealed class ConcatStream :
        Stream {

        // Public members

        public override bool CanRead => stream1.CanRead && stream2.CanRead;
        public override bool CanSeek => stream1.CanSeek && stream2.CanSeek;
        public override bool CanWrite => stream1.CanWrite && stream2.CanWrite;
        public override long Length => stream1.Length + stream2.Length;
        public override long Position {
            get => GetPosition();
            set => SetPosition(value);
        }

        public ConcatStream(Stream stream1, Stream stream2) {

            if (stream1 is null)
                throw new ArgumentNullException(nameof(stream1));

            if (stream2 is null)
                throw new ArgumentNullException(nameof(stream2));

            this.stream1 = stream1;
            this.stream2 = stream2;

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

            int bytesRead = stream1.Read(buffer, offset, count);

            count -= bytesRead;

            if (count > 0)
                bytesRead += stream2.Read(buffer, offset + bytesRead, count);

            return bytesRead;

        }
        public override void Write(byte[] buffer, int offset, int count) {

            long bytesLeftInStream1 = stream1.Length - stream1.Position;
            long bytesWritten = Math.Min(count, bytesLeftInStream1);

            stream1.Write(buffer, offset, (int)bytesWritten);

            if (bytesWritten < count)
                stream2.Write(buffer, offset + (int)bytesWritten, count - (int)bytesWritten);

        }

        // Protected members

        protected override void Dispose(bool disposing) {

            if (disposing) {

                stream1.Dispose();
                stream2.Dispose();

            }

            base.Dispose(disposing);

        }

        // Private members

        private readonly Stream stream1;
        private readonly Stream stream2;

        private long GetPosition() {

            if (stream1.Position < stream1.Length)
                return stream1.Position;

            return stream1.Length + stream2.Position;

        }
        private long SetPosition(long position) {

            if (position <= stream1.Length) {

                if (stream2.Position > 0)
                    stream2.Seek(0, SeekOrigin.Begin);

                stream1.Seek(position, SeekOrigin.Begin);

            }
            else {

                if (stream1.Position < stream1.Length)
                    stream1.Seek(0, SeekOrigin.End);

                stream2.Seek(position - stream1.Position, SeekOrigin.Begin);

            }

            return Position;

        }

    }

}