using Gsemac.Collections.Extensions;
using Gsemac.Net.Properties;
using Gsemac.Polyfills.System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;

namespace Gsemac.Net.Dns {

    public sealed class DnsClient :
        IDnsClient {

        // Public members

        ICollection<IDnsResolver> Resolvers { get; } = new List<IDnsResolver>();

        public DnsClient() { }
        public DnsClient(Uri endpoint) :
           this(DnsResolver.Create(endpoint)) {
        }
        public DnsClient(IPAddress endpoint) :
            this(DnsResolver.Create(endpoint)) {
        }
        public DnsClient(IPEndPoint endpoint) :
            this(DnsResolver.Create(endpoint)) {
        }
        public DnsClient(string endpoint) :
            this(DnsResolver.Create(endpoint)) {
        }
        public DnsClient(IDnsResolver resolver) {

            if (resolver is null)
                throw new ArgumentNullException(nameof(resolver));

            Resolvers.Add(resolver);

        }

        public IEnumerable<IPAddress> GetHostAddresses(string hostNameOrAddress) {

            if (hostNameOrAddress is null)
                throw new ArgumentNullException(nameof(hostNameOrAddress));

            if (IPAddress.TryParse(hostNameOrAddress, out IPAddress parsedIPAddress))
                return new[] { parsedIPAddress };

            return GetHostEntry(hostNameOrAddress).AddressList;

        }
        public IPHostEntry GetHostEntry(IPAddress address) {

            if (address is null)
                throw new ArgumentNullException(nameof(address));

            // Use the system DNS if no resolvers were added.

            if (!Resolvers.Any())
                return System.Net.Dns.GetHostEntry(address);

            // We want to get the aliases and IP addresses associated with a given IP address.
            // To do this, we'll make two requests-- We'll get a PTR record for the IP address to get the hostname, and then get the A records for the hostname.

            string reverseLookupAddress = DnsUtilities.GetReverseLookupAddress(address);

            IDnsMessage ptrDnsQuery = new DnsMessage() {
                Id = GetNextMessageId(),
                Type = DnsMessageType.Query,
            };

            ptrDnsQuery.Questions.Add(new DnsQuestion(reverseLookupAddress, DnsRecordType.PTR, DnsRecordClass.Internet));

            IDnsMessage ptrDnsResponse = GetDnsResponse(ptrDnsQuery);

            string domainName = ptrDnsResponse.Answers.First().DomainName;

            // Get the A records associated with the returned hostname.

            IDnsMessage aDnsQuery = new DnsMessage() {
                Id = GetNextMessageId(),
                Type = DnsMessageType.Query,
            };

            aDnsQuery.Questions.Add(new DnsQuestion(domainName, DnsRecordType.A, DnsRecordClass.Internet));

            IDnsMessage aDnsResponse = GetDnsResponse(aDnsQuery);

            // Create the host entry.
            // Note that the "Aliases" property is always empty when using this method.

            IPHostEntry hostEntry = new IPHostEntry() {
                HostName = domainName,
                AddressList = aDnsResponse.Answers.Select(a => a.HostAddress).ToArray(),
            };

            return hostEntry;

        }
        public IPHostEntry GetHostEntry(string hostNameOrAddress) {

            if (hostNameOrAddress is null)
                throw new ArgumentNullException(nameof(hostNameOrAddress));

            if (hostNameOrAddress.Length > MaxHostNameLength)
                throw new ArgumentOutOfRangeException(nameof(hostNameOrAddress), $"The length of {nameof(hostNameOrAddress)} parameter is greater than {MaxHostNameLength} characters.");

            // Use the system DNS if no resolvers were added.
            // GetHostEntry will also return local IP information for the empty string.

            if (!Resolvers.Any() || string.IsNullOrWhiteSpace(hostNameOrAddress))
                return System.Net.Dns.GetHostEntry(hostNameOrAddress);

            // If an IP address was passed, perform a reverse lookup on the IP address.

            if (IPEndPointEx.TryParse(hostNameOrAddress, out IPEndPoint ipEndPoint))
                return GetHostEntry(ipEndPoint.Address);

            // If a hostname was passed, get the IP addresses for the host.

            IDnsMessage dnsQuery = new DnsMessage() {
                Id = GetNextMessageId(),
                Type = DnsMessageType.Query,
            };

            dnsQuery.Questions.Add(new DnsQuestion(hostNameOrAddress, DnsRecordType.A, DnsRecordClass.Internet));

            IDnsMessage dnsResponse = GetDnsResponse(dnsQuery);

            IPHostEntry hostEntry = new IPHostEntry {
                HostName = hostNameOrAddress,
                AddressList = dnsResponse.Answers.Select(answer => answer.HostAddress).ToArray(),
            };

            return hostEntry;

        }

        public string GetHostName() {

            return System.Net.Dns.GetHostName();

        }

        public void Dispose() {

            random.Dispose();

        }

        // Private members

        private const int MaxHostNameLength = 255;

        private readonly RandomNumberGenerator random = RandomNumberGenerator.Create();
        private readonly IDnsCache cache = new DnsCache();

        private IDnsMessage GetDnsResponse(IDnsMessage dnsQuery) {

            if (dnsQuery is null)
                throw new ArgumentNullException(nameof(dnsQuery));

            // For each question, check the cache to see if we already have records available.

            Tuple<IDnsQuestion, IDnsAnswer[]>[] cachedAnswers = dnsQuery.Questions
                .Select(q => {

                    if (cache.TryGetRecords(q, out IDnsAnswer[] records))
                        return Tuple.Create(q, records);

                    return Tuple.Create(q, (IDnsAnswer[])null);

                })
                .Where(t => t.Item2 is object && t.Item2.Length > 0)
                .ToArray();

            // Remove the questions that we already have records for.

            foreach (IDnsQuestion question in cachedAnswers.Where(t => t.Item2 is object).Select(t => t.Item1))
                dnsQuery.Questions.Remove(question);

            IDnsMessage dnsResponse = null;

            if (dnsQuery.Questions.Any()) {

                // For reference, how Windows handles DNS queries is described here:
                // https://learn.microsoft.com/en-us/troubleshoot/windows-server/networking/dns-client-resolution-timeouts

                Exception lastException = null;

                foreach (IDnsResolver resolver in Resolvers.Where(r => r is object)) {

                    try {

                        dnsResponse = resolver.Resolve(dnsQuery);

                    }
                    catch (Exception ex) {

                        lastException = ex;

                    }

                }

                if (dnsResponse is null && lastException is object)
                    throw lastException;

                if (dnsResponse is null)
                    throw new Exception(ExceptionMessages.DnsRequestDidNotReceiveResponse);

                if (dnsResponse.ResponseCode != DnsResponseCode.NoError)
                    throw new DnsException(dnsResponse.ResponseCode);

                if (dnsResponse.Id != dnsQuery.Id)
                    throw new DnsException(ExceptionMessages.DnsMessageIdMismatch);

                // Cache the records that we received.

                foreach (IDnsQuestion q in dnsResponse.Questions) {

                    IDnsAnswer[] records = dnsResponse.Answers
                        .Where(a => a.Name.Equals(q.Name) && a.RecordType == q.RecordType && a.RecordClass == q.RecordClass)
                        .ToArray();

                    cache.AddRecords(q.Name, records);

                }

            }
            else {

                // All of the requested records were in the cache.

                dnsResponse = new DnsMessage() {
                    Id = dnsQuery.Id,
                    ResponseCode = DnsResponseCode.NoError,
                    Type = DnsMessageType.Response,
                };

                dnsResponse.Questions.AddRange(dnsQuery.Questions);

            }

            // Combined the new and cached records.

            if (cachedAnswers.Any()) {

                dnsResponse.Questions.AddRange(cachedAnswers.Select(t => t.Item1));
                dnsResponse.Answers.AddRange(cachedAnswers.SelectMany(t => t.Item2));

            }

            return dnsResponse;

        }
        private ushort GetNextMessageId() {

            byte[] buffer = new byte[2];

            random.GetBytes(buffer);

            return BitConverter.ToUInt16(buffer, 0);

        }


    }

}