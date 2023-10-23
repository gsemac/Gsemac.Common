using Gsemac.IO;
using Gsemac.Net.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Gsemac.Net.Dns {

    public sealed class DnsMessageSerializer :
        IDnsMessageSerializer {

        // Public members

        public void Serialize(Stream stream, IDnsMessage message) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (message is null)
                throw new ArgumentNullException(nameof(message));

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
        public IDnsMessage Deserialize(Stream stream) {

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

                // TODO: Handle service records and additional records.

                _ = reader.ReadUInt16(bits: 16); // NSCOUNT
                _ = reader.ReadUInt16(bits: 16); // ARCOUNT

                int byteOffset = 12;

                Dictionary<int, string> labelAddressDict = new Dictionary<int, string>();

                // Read the questions.

                for (int i = 0; i < questionCount; ++i)
                    message.Questions.Add(ReadQuestion(reader, labelAddressDict, ref byteOffset));

                // Read the answers.

                for (int i = 0; i < answerCount; ++i)
                    message.Answers.Add(ReadAnswer(reader, labelAddressDict, ref byteOffset));

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

        private string ReadNameSection(BitReader reader, IDictionary<int, string> addressLabelDict, ref int byteOffset) {

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

                    labels.Add(Tuple.Create(-1, addressLabelDict[labelAddress])); // -1 indicates pointer, so we don't cache this name

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

                if (labelOffset < 0)
                    continue;

                string labelName = string.Join(".", labels.Select(t => t.Item2).Skip(i));

                if (!addressLabelDict.ContainsKey(labelOffset))
                    addressLabelDict[labelOffset] = labelName;

            }

            return string.Join(".", labels.Select(t => t.Item2));

        }
        private DnsQuestion ReadQuestion(BitReader reader, IDictionary<int, string> addressLabelDict, ref int byteOffset) {

            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            if (addressLabelDict is null)
                throw new ArgumentNullException(nameof(addressLabelDict));

            string name = ReadNameSection(reader, addressLabelDict, ref byteOffset);

            DnsRecordType dnsRecordType = (DnsRecordType)reader.ReadUInt16(bits: 16); // QTYPE
            DnsRecordClass dnsClass = (DnsRecordClass)reader.ReadUInt16(bits: 16); // QCLASS

            byteOffset += 4;

            return new DnsQuestion() {
                Name = name,
                RecordType = dnsRecordType,
                Class = dnsClass,
            };

        }
        private DnsAnswer ReadAnswer(BitReader reader, IDictionary<int, string> addressLabelDict, ref int byteOffset) {

            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            if (addressLabelDict is null)
                throw new ArgumentNullException(nameof(addressLabelDict));

            string name = ReadNameSection(reader, addressLabelDict, ref byteOffset);
            DnsRecordType dnsRecordType = (DnsRecordType)reader.ReadUInt16(bits: 16); //  TYPE
            DnsRecordClass dnsClass = (DnsRecordClass)reader.ReadUInt16(bits: 16); // CLASS
            uint ttlSeconds = reader.ReadUInt32(bits: 32); //  TTL
            _ = reader.ReadUInt16(bits: 16); // RDLENGTH

            byteOffset += 10; // RDLENGTH not included

            DnsAnswer answer = new DnsAnswer() {
                Name = name,
                RecordType = dnsRecordType,
                Class = dnsClass,
                TimeToLive = TimeSpan.FromSeconds(ttlSeconds),
            };

            // The format of the following RDATA segment depends on the record type.

            switch (dnsRecordType) {

                case DnsRecordType.A:

                    // We have 4 octets representing the IPv4 address.

                    answer.HostAddress = ReadARecord(reader, ref byteOffset);

                    break;

                    // TODO: Handle other cases.

            }

            return answer;

        }
        private IPAddress ReadARecord(BitReader reader, ref int byteOffset) {

            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            byte[] buffer = reader.ReadBytes(4);

            byteOffset += buffer.Length;

            if (buffer.Length != 4)
                throw new Exception(ExceptionMessages.InvalidDnsHostAddress);

            IPAddress address = new IPAddress(buffer);

            return address;

        }

    }

}