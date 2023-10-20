using Gsemac.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            using (BitWriter writer = new BitWriter(stream, Encoding.ASCII, ByteOrder.BigEndian)) {

                // Write the message header.

                writer.Write((ushort)message.Id, bits: 16); // ID
                writer.Write(message.Type == DnsMessageType.Response); // QR
                writer.Write((byte)message.OpCode, bits: 4); // OPCODE
                writer.Write(message.IsAuthoritativeAnswer); // AA
                writer.Write(message.IsTruncated); // TC
                writer.Write(message.RecursionDesired); // RD
                writer.Write(message.RecursionAvailable); // DA
                writer.Write(0, bits: 3); // Z
                writer.Write((byte)message.ResponseCode, bits: 4); // RCODE
                writer.Write((ushort)message.Questions.Count, bits: 16); // QDCOUNT
                writer.Write((ushort)message.Answers.Count, bits: 16); // ANCOUNT
                writer.Write((ushort)0, bits: 16); // NSCOUNT
                writer.Write((ushort)0, bits: 16); // ARCOUNT

                int byteOffset = 12;

                Dictionary<string, int> labelAddressDict = new Dictionary<string, int>();

                // Write the questions.

                foreach (DnsQuestion question in message.Questions)
                    WriteQuestion(writer, question, labelAddressDict, ref byteOffset);

                // TODO: Write the answers.
                // Answers should use the same compression dictionary as the questions.

            }

        }
        public DnsMessage Deserialize(Stream stream) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            DnsMessage message = new DnsMessage();

            using (BitReader reader = new BitReader(stream, Encoding.ASCII, ByteOrder.BigEndian)) {

                // Read the message header.

                message.Id = reader.ReadUInt16(bits: 16); // ID
                message.Type = reader.ReadBoolean() ? DnsMessageType.Response : DnsMessageType.Query; // QR
                message.OpCode = (DnsOpCode)reader.ReadByte(bits: 4); // OPCODE
                message.IsAuthoritativeAnswer = reader.ReadBoolean(); // AA
                message.IsTruncated = reader.ReadBoolean(); // TC
                message.RecursionDesired = reader.ReadBoolean(); // RD
                message.RecursionAvailable = reader.ReadBoolean(); // RA
                reader.ReadByte(bits: 3); // Z
                message.ResponseCode = (DnsResponseCode)reader.ReadByte(bits: 4); // RCODE

                int questionCount = reader.ReadUInt16(bits: 16); // QDCOUNT
                int answerCount = reader.ReadUInt16(bits: 16); // ANCOUNT
                int serviceRecordCount = reader.ReadUInt16(bits: 16); // NSCOUNT
                int additionalRecordCount = reader.ReadUInt16(bits: 16); // ARCOUNT

                int byteOffset = 12;

                Dictionary<int, string> labelAddressDict = new Dictionary<int, string>();

                // Read the questions.

                for (int i = 0; i < questionCount; ++i)
                    message.Questions.Add(ReadQuestion(reader, labelAddressDict, ref byteOffset));

            }

            return message;

        }

        // Private members

        private void WriteQuestion(BitWriter writer, DnsQuestion question, IDictionary<string, int> labelAddressDict, ref int byteOffset) {

            if (writer is null)
                throw new ArgumentNullException(nameof(writer));

            if (question is null)
                throw new ArgumentNullException(nameof(question));

            if (labelAddressDict is null)
                throw new ArgumentNullException(nameof(labelAddressDict));

            // A domain name can be represented as:
            //
            // - A sequence of labels ending in a zero octet
            // - A pointer
            // - A sequence of labels ending with a pointer
            //
            // Note that a pointer always marks the end of a domain name.

            // I found this article helpful in explaining the compression scheme:
            // https://spathis.medium.com/how-dns-got-its-messages-on-diet-c49568b234a2

            string domainName = question.Name;

            while (true) {

                if (labelAddressDict.TryGetValue(domainName, out int labelOffset)) {

                    // Write a pointer to the previously-written label, terminating the domain name.

                    writer.Write(0b11, bits: 2);
                    writer.Write((ushort)labelOffset, bits: 14);

                    byteOffset += 2;

                    break;

                }
                else if (string.IsNullOrEmpty(domainName)) {

                    // Write the zero octet, terminating the domain name.

                    writer.Write(0, bits: 8);

                    ++byteOffset;

                    break;

                }
                else {

                    // Write the next segment of the domain name.

                    int domainNameDelimiterIndex = domainName.IndexOf('.');

                    string labelName = domainNameDelimiterIndex > 0 ?
                        domainName.Substring(0, domainNameDelimiterIndex) :
                        domainName;

                    // TODO: Verify the maximum byte offset (0x3FFF).

                    labelAddressDict[domainName] = byteOffset;

                    domainName = domainNameDelimiterIndex > 0 ?
                        domainName.Substring(domainNameDelimiterIndex + 1) :
                        string.Empty;

                    // TODO: Verify the maximum label length (0x1F) (the first two bits are reserved for pointers).

                    writer.Write((byte)labelName.Length, bits: 8);

                    ++byteOffset;

                    foreach (byte asciiByte in Encoding.ASCII.GetBytes(labelName)) {

                        writer.Write(asciiByte, bits: 8);

                        ++byteOffset;

                    }

                }

            }

            writer.Write((ushort)question.RecordType, bits: 16); // QTYPE
            writer.Write((ushort)question.Class, bits: 16); // QCLASS

            byteOffset += 4;

        }
        private DnsQuestion ReadQuestion(BitReader reader, IDictionary<int, string> addressLabelDict, ref int byteOffset) {

            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            if (addressLabelDict is null)
                throw new ArgumentNullException(nameof(addressLabelDict));

            List<Tuple<int, string>> labels = new List<Tuple<int, string>>();

            while (true) {

                byte labelPointerFlag = reader.ReadByte(bits: 2);

                if (labelPointerFlag == 0x3) {

                    // Look up the label value associated with this pointer.

                    // TODO: Validate the pointer is correct.

                    int labelAddress = reader.ReadUInt16(bits: 14);

                    labels.Add(Tuple.Create(byteOffset, addressLabelDict[labelAddress]));

                    byteOffset += 2;

                    // A pointer marks the end of a domain name.

                    break;

                }
                else {

                    // Read the label directly.

                    int labelOffset = byteOffset;
                    ushort labelLength = reader.ReadByte(bits: 6);

                    ++byteOffset;

                    // A zero octet marks the end of the domain name.

                    if (labelLength <= 0)
                        break;

                    byte[] labelBytes = reader.ReadBytes(labelLength);
                    string labelName = Encoding.ASCII.GetString(labelBytes);

                    labels.Add(Tuple.Create(labelOffset, labelName));

                    byteOffset += labelBytes.Length;

                }

            }

            // Add the labels we read to the compression dictionary for future lookups.

            for (int i = 0; i < labels.Count; ++i) {

                int labelOffset = labels[i].Item1;
                string labelName = string.Join(".", labels.Select(t => t.Item2).Skip(i));

                if (!addressLabelDict.ContainsKey(labelOffset))
                    addressLabelDict[labelOffset] = labelName;

            }

            DnsRecordType dnsRecordType = (DnsRecordType)reader.ReadUInt16(bits: 16); // QTYPE
            DnsClass dnsClass = (DnsClass)reader.ReadUInt16(bits: 16); // QCLASS

            byteOffset += 4;

            return new DnsQuestion() {
                Name = string.Join(".", labels.Select(t => t.Item2)),
                RecordType = dnsRecordType,
                Class = dnsClass,
            };

        }

    }

}