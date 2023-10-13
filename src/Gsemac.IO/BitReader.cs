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

            return base.ReadDecimal();

        }
        public override double ReadDouble() {

            return base.ReadDouble();

        }
        public override float ReadSingle() {

            return base.ReadSingle();

        }

        public override short ReadInt16() {

            return base.ReadInt16();

        }
        public override int ReadInt32() {

            return base.ReadInt32();

        }
        public override long ReadInt64() {

            return base.ReadInt64();

        }
        public override sbyte ReadSByte() {

            return base.ReadSByte();

        }
        public override ushort ReadUInt16() {

            return base.ReadUInt16();

        }
        public override uint ReadUInt32() {

            return base.ReadUInt32();

        }
        public override ulong ReadUInt64() {

            return base.ReadUInt64();

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

    }

}