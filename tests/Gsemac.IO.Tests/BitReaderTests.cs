using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace Gsemac.IO.Tests {

    [TestClass]
    public class BitReaderTests {

        // Read

        [TestMethod]
        public void TestReadWithAlignedBytes() {

            using (MemoryStream stream = new MemoryStream(new byte[] {
                0b01100000,
                0b01001110,
            }))
            using (BitReader reader = new BitReader(stream)) {

                byte[] buffer = new byte[2];

                int bytesRead = stream.Read(buffer, 0, buffer.Length);

                Assert.AreEqual(2, bytesRead);
                Assert.AreEqual(0b01100000, buffer[0]);
                Assert.AreEqual(0b01001110, buffer[1]);

            }

        }
        [TestMethod]
        public void TestReadWithUnalignedBytes() {

            using (MemoryStream stream = new MemoryStream(new byte[] {
                0b11100000,
                0b01001110,
                0b01000110,
            }))
            using (BitReader reader = new BitReader(stream)) {

                Assert.IsTrue(reader.ReadBoolean());

                byte[] buffer = new byte[2];

                int bytesRead = reader.Read(buffer, 0, buffer.Length);

                Assert.AreEqual(2, bytesRead);
                Assert.AreEqual(0b11000000, buffer[0]);
                Assert.AreEqual(0b10011100, buffer[1]);

            }

        }

        // ReadBoolean

        [TestMethod]
        public void TestReadBoolean() {

            using (MemoryStream stream = new MemoryStream(new byte[] {
                0b01100000,
            }))
            using (BitReader reader = new BitReader(stream)) {

                Assert.IsFalse(reader.ReadBoolean());
                Assert.IsTrue(reader.ReadBoolean());
                Assert.IsTrue(reader.ReadBoolean());
                Assert.IsFalse(reader.ReadBoolean());

            }

        }

        // ReadByte

        [TestMethod]
        public void TestReadByteWithAlignedBytes() {

            using (MemoryStream stream = new MemoryStream(new byte[] {
                0b01100000,
                0b01001110,
            }))
            using (BitReader reader = new BitReader(stream)) {

                Assert.AreEqual(0b01100000, reader.ReadByte());
                Assert.AreEqual(0b01001110, reader.ReadByte());

            }

        }
        [TestMethod]
        public void TestReadByteWithUnalignedBytes() {

            using (MemoryStream stream = new MemoryStream(new byte[] {
                0b11100000,
                0b01001110,
            }))
            using (BitReader reader = new BitReader(stream)) {

                Assert.IsTrue(reader.ReadBoolean());
                Assert.AreEqual(0b11000000, reader.ReadByte());

            }

        }
        [TestMethod]
        public void TestReadByteWithPartialByte() {

            using (MemoryStream stream = new MemoryStream(new byte[] {
                0b01100000,
            }))
            using (BitReader reader = new BitReader(stream)) {

                Assert.AreEqual(3, reader.ReadByte(numberOfBits: 3));

            }

        }
        [TestMethod]
        public void TestReadByteWithZeroLengthByte() {

            using (MemoryStream stream = new MemoryStream(new byte[] {
                0b01100000,
            }))
            using (BitReader reader = new BitReader(stream)) {

                Assert.AreEqual(0, reader.ReadByte(numberOfBits: 0));

            }

        }

        // ReadBytes

        [TestMethod]
        public void TestReadBytes() {

            using (MemoryStream stream = new MemoryStream(new byte[] {
                0b01100000,
                0b01001110,
            }))
            using (BitReader reader = new BitReader(stream)) {

                byte[] buffer = reader.ReadBytes(2);

                Assert.AreEqual(2, buffer.Length);
                Assert.AreEqual(0b01100000, buffer[0]);
                Assert.AreEqual(0b01001110, buffer[1]);

            }

        }
        [TestMethod]
        public void TestReadBytesWithCountLargerThanStreamLength() {

            using (MemoryStream stream = new MemoryStream(new byte[] {
                0b01100000,
                0b01001110,
            }))
            using (BitReader reader = new BitReader(stream)) {

                byte[] buffer = reader.ReadBytes(128);

                Assert.AreEqual(2, buffer.Length);
                Assert.AreEqual(0b01100000, buffer[0]);
                Assert.AreEqual(0b01001110, buffer[1]);

            }

        }

        // ReadChar

        [TestMethod]
        public void TestReadCharWithAsciiChar() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII, leaveOpen: true))
                    writer.Write('A');

                stream.Position = 0;

                using (BitReader reader = new BitReader(stream, Encoding.ASCII))
                    Assert.AreEqual('A', reader.ReadChar());

            }

        }
        [TestMethod]
        public void TestReadCharWithUtf8Char() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true))
                    writer.Write('字');

                stream.Position = 0;

                using (BitReader reader = new BitReader(stream, Encoding.UTF8))
                    Assert.AreEqual('字', reader.ReadChar());

            }

        }
        [TestMethod]
        public void TestReadCharWithUnicodeChar() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Unicode, leaveOpen: true))
                    writer.Write('字');

                stream.Position = 0;

                using (BitReader reader = new BitReader(stream, Encoding.Unicode))
                    Assert.AreEqual('字', reader.ReadChar());

            }

        }

        // ReadString

        [TestMethod]
        public void TestReadStringWithAsciiString() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII, leaveOpen: true))
                    writer.Write("Hello, world!");

                stream.Position = 0;

                using (BitReader reader = new BitReader(stream, Encoding.ASCII))
                    Assert.AreEqual("Hello, world!", reader.ReadString());

            }

        }
        [TestMethod]
        public void TestReadStringWithUtf8String() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true))
                    writer.Write("こんにちは世界！");

                stream.Position = 0;

                using (BitReader reader = new BitReader(stream, Encoding.UTF8))
                    Assert.AreEqual("こんにちは世界！", reader.ReadString());

            }

        }

        // ReadDecimal

        [TestMethod]
        public void TestReadDecimal() {

            decimal value = 4.78M;

            using (MemoryStream stream = new MemoryStream()) {

                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true))
                    writer.Write(value);

                stream.Seek(0, SeekOrigin.Begin);

                using (BitReader reader = new BitReader(stream))
                    Assert.AreEqual(value, reader.ReadDecimal());

            }

        }

        // ReadUInt16

        [TestMethod]
        public void TestReadReadUInt16WithAlignedBytes() {

            using (MemoryStream stream = new MemoryStream(new byte[] {
                0b01100000,
                0b01001110,
            }))
            using (BitReader reader = new BitReader(stream, ByteOrder.BigEndian)) {

                Assert.AreEqual(24654, reader.ReadUInt16());

            }

        }
        [TestMethod]
        public void TestReadReadUInt16WithUnalignedBytes() {

            using (MemoryStream stream = new MemoryStream(new byte[] {
                0b11110000,
                0b00100111,
                0b00110011,
            }))
            using (BitReader reader = new BitReader(stream, ByteOrder.BigEndian)) {

                Assert.IsTrue(reader.ReadBoolean());
                Assert.AreEqual(57422, reader.ReadUInt16());

            }

        }
        [TestMethod]
        public void TestReadReadUInt16WithLittleEndianByteOrder() {

            using (MemoryStream stream = new MemoryStream(new byte[] {
                0b01100000,
                0b01001110,
            }))
            using (BitReader reader = new BitReader(stream, ByteOrder.LittleEndian)) {

                Assert.AreEqual(20064, reader.ReadUInt16());

            }

        }
        [TestMethod]
        public void TestReadReadUInt16WithWithPartialByte() {

            using (MemoryStream stream = new MemoryStream(new byte[] {
                0b01100000,
            }))
            using (BitReader reader = new BitReader(stream)) {

                Assert.AreEqual(3, reader.ReadUInt16(numberOfBits: 3));

            }

        }
        [TestMethod]
        public void TestReadReadUInt16WithPartialByteAndMultipleBytesAndLittleEndianByteOrder() {

            using (MemoryStream stream = new MemoryStream(new byte[] {
                0b00000011,
                0b01000000,
            }))
            using (BitReader reader = new BitReader(stream, ByteOrder.LittleEndian)) {

                Assert.AreEqual(3, reader.ReadUInt16(numberOfBits: 9));

            }

        }
        [TestMethod]
        public void TestReadReadUInt16WithPartialByteAndMultipleBytesAndBigEndianByteOrder() {

            using (MemoryStream stream = new MemoryStream(new byte[] {
                0b00000001,
                0b11000000,
            }))
            using (BitReader reader = new BitReader(stream, ByteOrder.BigEndian)) {

                Assert.AreEqual(3, reader.ReadUInt16(numberOfBits: 9));

            }

        }
        [TestMethod]
        public void TestReadReadUInt16WithWithEvenlyDivisibleNumberOfBits() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true))
                    writer.Write(ushort.MaxValue);

                stream.Position = 0;

                using (BitReader reader = new BitReader(stream))
                    Assert.AreEqual(ushort.MaxValue, reader.ReadUInt16(numberOfBits: 16));

            }

        }
        [TestMethod]
        public void TestReadReadUInt16WithWithEvenlyDivisibleNumberOfBitsAndLittleEndianByteOrder() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BitWriter writer = new BitWriter(stream, Encoding.UTF8, ByteOrder.LittleEndian, leaveOpen: true))
                    writer.Write(ushort.MaxValue);

                stream.Position = 0;

                using (BitReader reader = new BitReader(stream, ByteOrder.LittleEndian))
                    Assert.AreEqual(ushort.MaxValue, reader.ReadUInt16(numberOfBits: 16));

            }

        }
        [TestMethod]
        public void TestReadReadUInt16WithWithEvenlyDivisibleNumberOfBitsAndBigEndianByteOrder() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BitWriter writer = new BitWriter(stream, Encoding.UTF8, ByteOrder.BigEndian, leaveOpen: true))
                    writer.Write(ushort.MaxValue);

                stream.Position = 0;

                using (BitReader reader = new BitReader(stream, ByteOrder.BigEndian))
                    Assert.AreEqual(ushort.MaxValue, reader.ReadUInt16(numberOfBits: 16));

            }

        }

        // ReadUInt32

        [TestMethod]
        public void TestReadReadUInt32WithAlignedBytes() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true))
                    writer.Write(uint.MaxValue);

                stream.Seek(0, SeekOrigin.Begin);

                using (BitReader reader = new BitReader(stream, ByteOrder.LittleEndian))
                    Assert.AreEqual(uint.MaxValue, reader.ReadUInt32());

            }

        }
        [TestMethod]
        public void TestReadReadUInt32WithUnalignedBytes() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BitWriter writer = new BitWriter(stream, Encoding.UTF8, ByteOrder.LittleEndian, leaveOpen: true)) {

                    writer.Write(true);
                    writer.Write(uint.MaxValue);

                }

                stream.Seek(0, SeekOrigin.Begin);

                using (BitReader reader = new BitReader(stream, ByteOrder.LittleEndian)) {

                    reader.ReadBoolean();

                    Assert.AreEqual(uint.MaxValue, reader.ReadUInt32());

                }

            }

        }

    }

}