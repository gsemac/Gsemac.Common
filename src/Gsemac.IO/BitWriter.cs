using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Gsemac.IO {

    // TODO: Implementing seeking (by bytes and bits), and implementing reading bytes from the stream to modify their bits.
    // For now, this class works best for writing to a stream that doesn't already contain data.

    public sealed class BitWriter :
        BinaryWriter {

        // Public members

        public BitWriter(Stream stream) :
            this(stream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true), ByteOrder.Default) {
        }
        public BitWriter(Stream stream, ByteOrder byteOrder) :
           this(stream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true), byteOrder) {
        }
        public BitWriter(Stream stream, Encoding encoding) :
            this(stream, encoding, ByteOrder.Default) {

            this.encoding = encoding;

        }
        public BitWriter(Stream stream, Encoding encoding, ByteOrder byteOrder) :
            base(stream, encoding) {

            this.encoding = encoding;
            this.byteOrder = byteOrder;

        }

        public override void Write(bool value) {

            WriteBit(value);

        }
        public override void Write(byte value) {

            // The bit index will always remain the same after writing a byte.

            byte previousBitIndex = bitIndex;

            currentByte |= (byte)(value >> bitIndex);

            CommitByte();

            currentByte |= (byte)(value << (BitsPerByte - previousBitIndex));

            bitIndex = previousBitIndex;

        }
        public override void Write(byte[] buffer) {

            if (buffer is null)
                throw new ArgumentNullException(nameof(buffer));

            Write(buffer, 0, buffer.Length);

        }
        public override void Write(byte[] buffer, int index, int count) {

            if (buffer is null)
                throw new ArgumentNullException(nameof(buffer));

            if (index < 0 || index >= buffer.Length || count < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (buffer.Length - index < count)
                throw new ArgumentException($"The buffer length minus {nameof(index)} is less than {nameof(count)}.");

            for (int i = index; i < index + count; ++i)
                Write(buffer[i]);

        }

        public override void Write(char ch) {

            Write(encoding.GetBytes(new char[] { ch }, 0, 1));

        }
        public override void Write(char[] chars) {

            if (chars is null)
                throw new ArgumentNullException(nameof(chars));

            Write(chars, 0, chars.Length);

        }
        public override void Write(char[] chars, int index, int count) {

            if (chars is null)
                throw new ArgumentNullException(nameof(chars));

            if (index < 0 || index >= chars.Length || count < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (chars.Length - index < count)
                throw new ArgumentException($"The buffer length minus {nameof(index)} is less than {nameof(count)}.");

            for (int i = index; i < index + count; ++i)
                Write(chars[i]);

        }
        public override void Write(string value) {

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            // BinaryWriter writes the string prefixed with a 7-bit encoded integer length.

            Write7BitEncodedInt(value.Length);

            Write(encoding.GetBytes(value));

        }

        public override void Write(decimal value) {

            foreach (int part in decimal.GetBits(value))
                Write(part);

        }
        public override void Write(double value) {

            Write(BitConverter.GetBytes(value));

        }
        public override void Write(float value) {

            Write(BitConverter.GetBytes(value));

        }

        public override void Write(int value) {

            WriteOrderedBytes(BitConverter.GetBytes(value));

        }
        public override void Write(long value) {

            WriteOrderedBytes(BitConverter.GetBytes(value));

        }
        public override void Write(sbyte value) {

            WriteOrderedBytes(BitConverter.GetBytes(value));

        }
        public override void Write(short value) {

            WriteOrderedBytes(BitConverter.GetBytes(value));

        }
        public override void Write(uint value) {

            WriteOrderedBytes(BitConverter.GetBytes(value));

        }
        public override void Write(ulong value) {

            WriteOrderedBytes(BitConverter.GetBytes(value));

        }
        public override void Write(ushort value) {

            WriteOrderedBytes(BitConverter.GetBytes(value));

        }

        public void Write(byte value, int numberOfBits) {

            WriteLowOrderBits(new byte[] { value }, numberOfBits);

        }
        public void Write(uint value, int numberOfBits) {

            WriteLowOrderBits(BitConverter.GetBytes(value), numberOfBits);

        }
        public void Write(ulong value, int numberOfBits) {

            WriteLowOrderBits(BitConverter.GetBytes(value), numberOfBits);

        }
        public void Write(ushort value, int numberOfBits) {

            WriteLowOrderBits(BitConverter.GetBytes(value), numberOfBits);

        }

        public override void Flush() {

            if (bitIndex > 0)
                CommitByte();

            base.Flush();

        }
        public override void Close() {

            Flush();

            base.Close();

        }
        protected override void Dispose(bool disposing) {

            if (disposing) {

                // We only flush instead of closing-- The underlying BinaryWriter does not call Close() when disposing either.

                Flush();

            }

            base.Dispose(disposing);

        }

        // Private members

        private const byte BitsPerByte = 8;

        private readonly Encoding encoding;
        private readonly ByteOrder byteOrder;
        private byte currentByte = 0;
        private byte bitIndex = 0;

        private void WriteBit(bool value) {

            byte bitMask = (byte)(0x80 >> bitIndex);

            if (value) {

                currentByte |= bitMask;

            }
            else {

                currentByte &= (byte)~bitMask;

            }

            if (++bitIndex >= 8)
                CommitByte();

        }
        private void WriteBits(byte value, int startIndex, int count) {

            for (int j = startIndex; j < Math.Min(startIndex + count, BitsPerByte); ++j) {

                WriteBit((value & (0x80 >> j)) > 0);

            }

        }
        private void WriteLowOrderBits(byte[] bytes, int numberOfBits) {

            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes));

            if (numberOfBits < 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfBits));

            if (numberOfBits <= 0)
                return;

            // If we're on a little endian system, we want to write the first N bytes.
            // If we're on a big endian system, we want to write the last N bytes.

            // For simplicity, we'll just normalize our byte array to little endian.

            bytes = ToLittleEndian(bytes);

            if (byteOrder == ByteOrder.BigEndian || (byteOrder == ByteOrder.Default && !BitConverter.IsLittleEndian)) {

                // Write the bytes in big endian order.

                // If we're writing a partial byte, we want the cutoff in the last byte.
                // Writing 3 with 9 bits would look like:
                // .......0
                // 00000011

                int bitIndex = (BitsPerByte - numberOfBits % BitsPerByte) % BitsPerByte;
                int byteIndex = Math.Min(numberOfBits / BitsPerByte, bytes.Length) - 1;

                for (int i = byteIndex; i >= 0; --i) {

                    WriteBits(bytes[i], bitIndex, BitsPerByte - bitIndex);

                    bitIndex = 0;

                }

            }
            else {

                // Write the bytes in little endian order.

                // If we're writing a partial byte, we want the cutoff in the first byte.
                // Writing 3 with 9 bits would look like:
                // 00000011
                // 0.......

                int byteIndex = 0;
                int totalBytes = (int)Math.Ceiling(numberOfBits / (double)BitsPerByte);

                for (int i = byteIndex; i < totalBytes; ++i) {

                    bool isLastIteration = i + 1 >= totalBytes;

                    int bitIndex = isLastIteration ?
                        (BitsPerByte - numberOfBits % BitsPerByte) % BitsPerByte :
                        0;

                    WriteBits(bytes[i], bitIndex, BitsPerByte - bitIndex);

                }

            }

        }
        private bool IsByteReorderingRequired() {

            return BitConverter.IsLittleEndian && byteOrder == ByteOrder.BigEndian ||
                !BitConverter.IsLittleEndian && byteOrder == ByteOrder.LittleEndian;

        }
        private byte[] ToLittleEndian(byte[] bytes) {

            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes));

            if (!BitConverter.IsLittleEndian)
                return bytes.Reverse().ToArray();

            return bytes;

        }
        private void WriteOrderedBytes(byte[] bytes) {

            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes));

            if (bytes.Length <= 0)
                return;

            if (IsByteReorderingRequired()) {

                for (int i = bytes.Length - 1; i >= 0; --i) {

                    Write(bytes[i]);

                }

            }
            else {

                Write(bytes);

            }

        }
        private void CommitByte() {

            base.Write(currentByte);

            currentByte = 0;
            bitIndex = 0;

        }

    }

}