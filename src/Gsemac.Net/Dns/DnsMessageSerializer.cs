using Gsemac.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Gsemac.Net.Dns {

    public sealed class DnsMessageSerializer {

        // Public members

        public void Serialize(DnsMessage message, Stream stream) {

            if (message is null)
                throw new ArgumentNullException(nameof(message));

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            // See the following assignment for a good description of the anatomy of a DNS packet:
            // https://mislove.org/teaching/cs4700/spring11/handouts/project1-primer.pdf

            using (BitWriter writer = new BitWriter(stream, ByteOrder.BigEndian)) {

                // Write the message header

                writer.Write((ushort)message.Id, numberOfBits: 16); // ID
                writer.Write(message.Type == DnsMessageType.Response); // QR
                writer.Write((byte)message.OpCode, numberOfBits: 4); // OPCODE
                writer.Write(message.IsAuthoritativeAnswer); // AA
                writer.Write(message.IsTruncated); // TC
                writer.Write(message.RecursionDesired); // RD
                writer.Write(message.RecursionAvailable); // DA
                writer.Write(0, numberOfBits: 3); // Z
                writer.Write((byte)message.ResponseCode, numberOfBits: 4); // RCODE
                writer.Write((ushort)message.Questions.Count, numberOfBits: 16); // QDCOUNT
                writer.Write((ushort)message.Answers.Count, numberOfBits: 16); // ANCOUNT
                writer.Write((ushort)0, numberOfBits: 16); // NSCOUNT
                writer.Write((ushort)0, numberOfBits: 16); // ARCOUNT

                int byteOffset = 12;

                // Write the questions

                Dictionary<string, int> compressionDict = new Dictionary<string, int>();

                foreach (DnsQuestion question in message.Questions) {

                    string[] domainParts = question.Name.Split('.');

                    foreach (string domainPart in domainParts) {

                        if (compressionDict.TryGetValue(domainPart, out int address)) {

                            // Write a pointer to the previously-written name part.

                            writer.Write(true);
                            writer.Write(true);
                            writer.Write((ushort)address, numberOfBits: 14);

                            byteOffset += 2;

                        }
                        else {

                            // Write the name part directly.

                            compressionDict[domainPart] = byteOffset;

                            writer.Write((byte)domainPart.Length, numberOfBits: 8);

                            ++byteOffset;

                            foreach (byte asciiByte in Encoding.ASCII.GetBytes(domainPart)) {

                                writer.Write(asciiByte, numberOfBits: 8);

                                ++byteOffset;

                            }

                        }

                    }

                    // Mark the end of the names segment.

                    writer.Write(0, numberOfBits: 8);

                    writer.Write((ushort)question.RecordType, numberOfBits: 16); // QTYPE
                    writer.Write((ushort)question.Class, numberOfBits: 16); // QCLASS

                    byteOffset += 5;

                }

                // TODO: Write the answers

            }

        }
        public DnsMessage Deserialize(Stream stream) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            throw new NotImplementedException();

        }

    }

}