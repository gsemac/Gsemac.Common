using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Text;

namespace Gsemac.IO.Tests {

    [TestClass]
    public class BitWriterTests {

        // Write

        [TestMethod]
        public void TestWriteWithBool() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BitWriter writer = new BitWriter(stream)) {

                    writer.Write(true);
                    writer.Write(false);
                    writer.Write(true);

                }

                Assert.AreEqual(0b10100000, stream.ToArray()[0]);

            }

        }
        [TestMethod]
        public void TestWriteWithByte() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BitWriter writer = new BitWriter(stream)) {

                    writer.Write(true);
                    writer.Write((byte)91);
                    writer.Write(true);

                }

                CollectionAssert.AreEqual(new byte[] {
                    0b10101101,
                    0b11000000
                }, stream.ToArray().Take(2).ToArray());

            }

        }
        [TestMethod]
        public void TestWriteWithByteArray() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BitWriter writer = new BitWriter(stream)) {

                    writer.Write(true);
                    writer.Write(new byte[] {
                        1,
                        2,
                        3,
                    });
                    writer.Write(true);

                }

                CollectionAssert.AreEqual(new byte[] {
                    0b10000000,
                    0b10000001,
                    0b00000001,
                    0b11000000,
                }, stream.ToArray().Take(4).ToArray());

            }

        }

        [TestMethod]
        public void TestWriteWithChar() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BitWriter writer = new BitWriter(stream)) {

                    writer.Write(true);
                    writer.Write('A');

                }

                CollectionAssert.AreEqual(new byte[] {
                    0b10100000,
                    0b10000000
                }, stream.ToArray().Take(2).ToArray());

            }

        }
        [TestMethod]
        public void TestWriteWithAsciiChar() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BitWriter writer = new BitWriter(stream, Encoding.ASCII, leaveOpen: true))
                    writer.Write('A');

                stream.Position = 0;

                using (BinaryReader reader = new BinaryReader(stream, Encoding.ASCII))
                    Assert.AreEqual('A', reader.ReadChar());

            }

        }
        [TestMethod]
        public void TestWriteWithUtf8Char() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BitWriter writer = new BitWriter(stream, Encoding.UTF8, leaveOpen: true))
                    writer.Write('字');

                stream.Position = 0;

                using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8))
                    Assert.AreEqual('字', reader.ReadChar());

            }

        }

        [TestMethod]
        public void TestWriteWithAsciiString() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BitWriter writer = new BitWriter(stream, Encoding.ASCII, leaveOpen: true))
                    writer.Write("Hello, world!");

                stream.Position = 0;

                using (BinaryReader reader = new BinaryReader(stream, Encoding.ASCII))
                    Assert.AreEqual("Hello, world!", reader.ReadString());

            }

        }
        [TestMethod]
        public void TestWriteWithUtf8String() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BitWriter writer = new BitWriter(stream, Encoding.UTF8, leaveOpen: true))
                    writer.Write("こんにちは世界！");

                stream.Position = 0;

                using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8))
                    Assert.AreEqual("こんにちは世界！", reader.ReadString());

            }

        }

        [TestMethod]
        public void TestWriteWithDecimal() {

            decimal value = 4.78M;

            using (MemoryStream stream = new MemoryStream()) {

                using (BitWriter writer = new BitWriter(stream, Encoding.UTF8, leaveOpen: true))
                    writer.Write(value);

                stream.Seek(0, SeekOrigin.Begin);

                using (BinaryReader reader = new BinaryReader(stream))
                    Assert.AreEqual(value, reader.ReadDecimal());

            }

        }

        [TestMethod]
        public void TestWriteWithUIntWithLittleEndianByteOrder() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BitWriter writer = new BitWriter(stream, ByteOrder.LittleEndian)) {

                    writer.Write((uint)3);

                }

                CollectionAssert.AreEqual(new byte[] {
                    0b00000011,
                    0b00000000,
                    0b00000000,
                    0b00000000,
                }, stream.ToArray().Take(4).ToArray());

            }

        }
        [TestMethod]
        public void TestWriteWithUIntWithBigEndianByteOrder() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BitWriter writer = new BitWriter(stream, ByteOrder.BigEndian)) {

                    writer.Write((uint)3);

                }

                CollectionAssert.AreEqual(new byte[] {
                    0b00000000,
                    0b00000000,
                    0b00000000,
                    0b00000011,
                }, stream.ToArray().Take(4).ToArray());

            }

        }
        [TestMethod]

        public void TestWriteWithUShortWithLittleEndianByteOrder() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BitWriter writer = new BitWriter(stream, ByteOrder.LittleEndian)) {

                    writer.Write((ushort)3);

                }

                CollectionAssert.AreEqual(new byte[] {
                    0b00000011,
                    0b00000000,
                }, stream.ToArray().Take(4).ToArray());

            }

        }
        [TestMethod]
        public void TestWriteWithUShortWithBigEndianByteOrder() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BitWriter writer = new BitWriter(stream, ByteOrder.BigEndian)) {

                    writer.Write((ushort)3);

                }

                CollectionAssert.AreEqual(new byte[] {
                    0b00000000,
                    0b00000011,
                }, stream.ToArray().Take(4).ToArray());

            }

        }

        [TestMethod]
        public void TestWriteWithPartialByte() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BitWriter writer = new BitWriter(stream)) {

                    writer.Write(true);
                    writer.Write(3, bits: 4);
                    writer.Write(true);

                }

                CollectionAssert.AreEqual(new byte[] {
                    0b10011100,
                }, stream.ToArray().Take(1).ToArray());

            }

        }

        [TestMethod]
        public void TestWriteWithPartialUShortWithNumberOfBitsLessThanByte() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BitWriter writer = new BitWriter(stream)) {

                    writer.Write(true);
                    writer.Write((ushort)3, bits: 3);
                    writer.Write(true);

                }

                CollectionAssert.AreEqual(new byte[] {
                    0b10111000,
                }, stream.ToArray().Take(1).ToArray());

            }

        }
        [TestMethod]
        public void TestWriteWithPartialUShortWithNumberOfBitsGreaterThanByte() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BitWriter writer = new BitWriter(stream, ByteOrder.LittleEndian)) {

                    writer.Write(true);
                    writer.Write((ushort)3, bits: 9);
                    writer.Write(true);

                }

                CollectionAssert.AreEqual(new byte[] {
                    0b10000001,
                    0b10100000,
                }, stream.ToArray().Take(2).ToArray());

            }

        }

        [TestMethod]
        public void TestWriteWithPartialUIntWithLittleEndianByteOrder() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BitWriter writer = new BitWriter(stream, ByteOrder.LittleEndian)) {

                    writer.Write((uint)3, bits: 16);

                }

                CollectionAssert.AreEqual(new byte[] {
                    0b00000011,
                    0b00000000,
                }, stream.ToArray().Take(2).ToArray());

            }

        }
        [TestMethod]
        public void TestWriteWithPartialUIntWithBigEndianByteOrder() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BitWriter writer = new BitWriter(stream, ByteOrder.BigEndian)) {

                    writer.Write((uint)3, bits: 16);

                }

                CollectionAssert.AreEqual(new byte[] {
                    0b00000000,
                    0b00000011,
                }, stream.ToArray().Take(2).ToArray());

            }

        }

    }

}