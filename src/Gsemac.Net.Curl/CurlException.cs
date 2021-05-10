using System;
using System.Text;

namespace Gsemac.Net.Curl {

    public sealed class CurlException :
        Exception {

        // Public members

        public CurlCode ResultCode { get; }

        public CurlException(CurlCode resultCode) :
            base(GetExceptionMessage(resultCode)) {

            this.ResultCode = resultCode;

        }
        public CurlException(int exitCode) :
           this((CurlCode)exitCode) {
        }

        public CurlException(CurlCode resultCode, string message) :
           base(message) {

            this.ResultCode = resultCode;

        }
        public CurlException(int exitCode, string message) :
          base(message) {

            this.ResultCode = (CurlCode)exitCode;

        }

        public CurlException() :
            this(-1) {
        }
        public CurlException(string message) :
            base(message) {

            this.ResultCode = (CurlCode)(-1);

        }
        public CurlException(string message, Exception innerException) :
            base(message, innerException) {

            this.ResultCode = (CurlCode)(-1);

        }

        // Private members

        private static string GetExceptionMessage(CurlCode resultCode) {

            StringBuilder sb = new StringBuilder();

            sb.Append($"({(int)resultCode}) ");

            if ((int)resultCode == -1) {

                sb.Append("The process exited prematurely.");

            }
            else {

                switch (resultCode) {

                    case CurlCode.OK:
                        sb.Append("The operation completed successfully.");
                        break;

                    case CurlCode.CouldntResolveHost:
                        sb.Append($"No such host is known.");
                        break;

                    default:
                        sb.Append($"The operation returned an error: {(int)resultCode}");
                        break;

                }

            }

            return sb.ToString();

        }

    }

}