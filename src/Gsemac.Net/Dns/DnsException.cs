using System;
using System.Runtime.Serialization;

namespace Gsemac.Net.Dns {

    public class DnsException :
        Exception {

        // Public members

        public DnsResponseCode ResponseCode { get; }

        public DnsException(DnsResponseCode responseCode) {

            ResponseCode = responseCode;


        }

        public DnsException(string message) : base(message) {
        }
        public DnsException(string message, Exception innerException) :
            base(message, innerException) {
        }
        public DnsException(SerializationInfo info, StreamingContext context) :
            base(info, context) {
        }

    }

}