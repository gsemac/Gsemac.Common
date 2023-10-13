using System;
using System.IO;
using System.Text;

namespace Gsemac.IO {

    public sealed class BitReader :
        BinaryReader {

        // Public members

        public BitReader(Stream stream) :
            this(stream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true), ByteOrder.Default) {
        }
        public BitReader(Stream stream, ByteOrder byteOrder) :
           this(stream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true), byteOrder) {
        }
        public BitReader(Stream stream, Encoding encoding) :
            this(stream, encoding, ByteOrder.Default) {

            this.encoding = encoding;

        }
        public BitReader(Stream stream, Encoding encoding, ByteOrder byteOrder) :
            base(stream, encoding) {

            this.encoding = encoding;
            this.byteOrder = byteOrder;

        }

        public override int Read() {

            return base.Read();

        }
        public override int Read(byte[] buffer, int index, int count) {

            if (buffer is null)
                throw new ArgumentNullException(nameof(buffer));

            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (buffer.Length - index < count)
                throw new ArgumentException($"The buffer length minus {nameof(index)} is less than {nameof(count)}.");

            int bytesRead = 0;

            for (int i = index; i < index + count; ++i) {

                if (NeedsNextByte() && !LoadNextByte())
                    break;

                buffer[i] = ReadByte();

                ++bytesRead;

            }

            return bytesRead;

        }
        public override int Read(char[] buffer, int index, int count) {

            return base.Read(buffer, index, count);

        }

        public override bool ReadBoolean() {

            if (NeedsNextByte() && !LoadNextByte())
                throw new EndOfStreamException();

            return (currentByte & (0x80 >> bitIndex++)) > 0;

        }
        public override byte ReadByte() {

            if (NeedsNextByte() && !LoadNextByte())
                throw new EndOfStreamException();

            byte result = 0;

            result |= (byte)(currentByte << bitIndex);

            int bitsRequired = bitIndex;

            // We either consumed the entire byte, or the remains of the byte.

            if (bitsRequired > 0) {

                if (!LoadNextByte())
                    throw new EndOfStreamException();

                result |= (byte)(currentByte >> (BitsPerByte - bitsRequired));

                bitIndex = (byte)bitsRequired;

            }
            else {

                bitIndex = BitsPerByte;

            }

            return result;

        }
        public override byte[] ReadBytes(int count) {

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0)
                return new byte[] { };

            byte[] buffer = new byte[count];

            int bytesRead = Read(buffer, 0, count);

            if (bytesRead < buffer.Length) {

                byte[] newBuffer = new byte[bytesRead];

                Array.Copy(buffer, newBuffer, newBuffer.Length);

                buffer = newBuffer;

            }

            return buffer;

        }

        public override int PeekChar() {

            return base.PeekChar();

        }
        public override char ReadChar() {

            return base.ReadChar();

        }
        public override char[] ReadChars(int count) {

            return base.ReadChars(count);

        }
        public override string ReadString() {

            return base.ReadString();

        }

        public override decimal ReadDecimal() {

            byte[] buffer = new byte[sizeof(decimal)];

            if (Read(buffer, 0, buffer.Length) < buffer.Length)
                throw new EndOfStreamException();

            return BytesToDecimal(buffer);

        }
        public override double ReadDouble() {

            byte[] buffer = new byte[sizeof(double)];

            if (Read(buffer, 0, buffer.Length) < buffer.Length)
                throw new EndOfStreamException();

            return BitConverter.ToDouble(buffer, 0);

        }
        public override float ReadSingle() {

            byte[] buffer = new byte[sizeof(float)];

            if (Read(buffer, 0, buffer.Length) < buffer.Length)
                throw new EndOfStreamException();

            return BitConverter.ToSingle(buffer, 0);

        }

        public override short ReadInt16() {

            byte[] buffer = new byte[sizeof(short)];

            if (ReadOrderedBytes(buffer, 0, buffer.Length) < buffer.Length)
                throw new EndOfStreamException();

            return BitConverter.ToInt16(buffer, 0);

        }
        public override int ReadInt32() {

            byte[] buffer = new byte[sizeof(int)];

            if (ReadOrderedBytes(buffer, 0, buffer.Length) < buffer.Length)
                throw new EndOfStreamException();

            return BitConverter.ToInt32(buffer, 0);

        }
        public override long ReadInt64() {

            byte[] buffer = new byte[sizeof(long)];

            if (ReadOrderedBytes(buffer, 0, buffer.Length) < buffer.Length)
                throw new EndOfStreamException();

            return BitConverter.ToInt64(buffer, 0);

        }
        public override sbyte ReadSByte() {

            return (sbyte)ReadByte();

        }
        public override ushort ReadUInt16() {

            byte[] buffer = new byte[sizeof(ushort)];

            if (ReadOrderedBytes(buffer, 0, buffer.Length) < buffer.Length)
                throw new EndOfStreamException();

            return BitConverter.ToUInt16(buffer, 0);

        }
        public override uint ReadUInt32() {

            byte[] buffer = new byte[sizeof(uint)];

            if (ReadOrderedBytes(buffer, 0, buffer.Length) < buffer.Length)
                throw new EndOfStreamException();

            return BitConverter.ToUInt32(buffer, 0);

        }
        public override ulong ReadUInt64() {

            byte[] buffer = new byte[sizeof(ulong)];

            if (ReadOrderedBytes(buffer, 0, buffer.Length) < buffer.Length)
                throw new EndOfStreamException();

            return BitConverter.ToUInt64(buffer, 0);

        }

        // Private members

        private const byte BitsPerByte = 8;

        private readonly Encoding encoding;
        private readonly ByteOrder byteOrder;
        private bool isBufferInitialized = false;
        private byte currentByte = 0;
        private byte bitIndex = 0;

        private bool NeedsNextByte() {

            return !isBufferInitialized ||
                 bitIndex >= BitsPerByte;

        }
        private bool LoadNextByte() {

            int nextByteFromStream = BaseStream.ReadByte();

            if (nextByteFromStream < 0)
                return false;

            currentByte = (byte)nextByteFromStream;
            bitIndex = 0;

            isBufferInitialized = true;

            return true;

        }
        private bool IsByteReorderingRequired() {

            return BitConverter.IsLittleEndian && byteOrder == ByteOrder.BigEndian ||
                !BitConverter.IsLittleEndian && byteOrder == ByteOrder.LittleEndian;

        }
        private int ReadOrderedBytes(byte[] buffer, int index, int count) {

            if (buffer is null)
                throw new ArgumentNullException(nameof(buffer));

            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (buffer.Length - index < count)
                throw new ArgumentException($"The buffer length minus {nameof(index)} is less than {nameof(count)}.");

            if (count <= 0)
                return 0;

            int bytesRead = 0;

            while (bytesRead < count) {

                bytesRead += Read(buffer, index, count);

                if (bytesRead <= 0)
                    break;

                index += bytesRead;

            }

            if (IsByteReorderingRequired())
                Array.Reverse(buffer);

            return bytesRead;

        }

        public static decimal BytesToDecimal(byte[] buffer) {

            // The following implementation is based on decimal.ToDecimal:
            // https://github.com/microsoft/referencesource/blob/master/mscorlib/system/decimal.cs#L594

            if (buffer is null)
                throw new ArgumentNullException(nameof(buffer));

            int lo = buffer[0] | (buffer[1] << 8) | (buffer[2] << 16) | (buffer[3] << 24);
            int mid = buffer[4] | (buffer[5] << 8) | (buffer[6] << 16) | (buffer[7] << 24);
            int hi = buffer[8] | (buffer[9] << 8) | (buffer[10] << 16) | (buffer[11] << 24);
            int flags = buffer[12] | (buffer[13] << 8) | (buffer[14] << 16) | (buffer[15] << 24);

            return new decimal(new int[] {
                lo,
                mid,
                hi,
                flags,
            });
        }

    }

}