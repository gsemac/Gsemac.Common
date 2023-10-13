using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

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

    }

}