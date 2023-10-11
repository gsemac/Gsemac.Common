using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace Gsemac.IO.Tests {

    [TestClass]
    public class BitWriterTests {

        // Write

        [TestMethod]
        public void TestWriteBool() {

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
        public void TestWriteByte() {

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
        public void TestWriteByteArray() {

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
        public void TestWriteChar() {

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
        public void TestWriteUIntWithLittleEndianByteOrder() {

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
        public void TestWriteUIntWithBigEndianByteOrder() {

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
        public void TestWriteUShortWithLittleEndianByteOrder() {

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
        public void TestWriteUShortWithBigEndianByteOrder() {

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
        public void TestWritePartialByte() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BitWriter writer = new BitWriter(stream)) {

                    writer.Write(true);
                    writer.Write(3, numberOfBits: 4);
                    writer.Write(true);

                }

                CollectionAssert.AreEqual(new byte[] {
                    0b10011100,
                }, stream.ToArray().Take(1).ToArray());

            }

        }
        [TestMethod]
        public void TestWritePartialUShortWithNumberOfBitsLessThanByte() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BitWriter writer = new BitWriter(stream)) {

                    writer.Write(true);
                    writer.Write((ushort)3, numberOfBits: 3);
                    writer.Write(true);

                }

                CollectionAssert.AreEqual(new byte[] {
                    0b10111000,
                }, stream.ToArray().Take(1).ToArray());

            }

        }
        [TestMethod]
        public void TestWritePartialUShortWithNumberOfBitsGreaterThanByte() {

            using (MemoryStream stream = new MemoryStream()) {

                using (BitWriter writer = new BitWriter(stream)) {

                    writer.Write(true);
                    writer.Write((ushort)3, numberOfBits: 9);
                    writer.Write(true);

                }

                CollectionAssert.AreEqual(new byte[] {
                    0b10000000,
                    0b11100000,
                }, stream.ToArray().Take(2).ToArray());

            }

        }

    }

}