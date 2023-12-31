using Gsemac.Net.Curl.Properties;
using System;
using System.Text;

namespace Gsemac.Net.Curl {

    public sealed class CurlException :
        Exception {

        // Public members

        public CurlCode ResultCode { get; }

        public CurlException(CurlCode resultCode) :
            base(GetExceptionMessage(resultCode)) {

            ResultCode = resultCode;

        }
        public CurlException(int exitCode) :
           this((CurlCode)exitCode) {
        }

        public CurlException(CurlCode resultCode, string message) :
           base(message) {

            ResultCode = resultCode;

        }
        public CurlException(int exitCode, string message) :
          base(message) {

            ResultCode = (CurlCode)exitCode;

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

            // The exception message format is based on the ones provided by the console version of cURL.

            StringBuilder sb = new StringBuilder();

            sb.Append($"({(int)resultCode}) ");

            if ((int)resultCode < 0) {

                sb.Append(string.Format(ExceptionMessages.CurlProcessReturnedErrorCode, resultCode));

            }
            else {

                switch (resultCode) {

                    case CurlCode.OK:
                        sb.Append(CurlCodeDescriptions.Ok);
                        break;

                    case CurlCode.CouldntResolveHost:
                        sb.Append(CurlCodeDescriptions.CouldntResolveHost);
                        break;

                    case CurlCode.CouldntConnect:
                        sb.Append(CurlCodeDescriptions.CouldntConnect);
                        break;

                    case CurlCode.SslConnectError:
                        sb.Append(CurlCodeDescriptions.SslConnectError);
                        break;

                    case CurlCode.PeerFailedVerification:
                        sb.Append(CurlCodeDescriptions.PeerFailedVerification);
                        break;

                    case CurlCode.SendFailRewind:
                        sb.Append(CurlCodeDescriptions.SendFailRewind);
                        break;

                    default:
                        sb.Append(string.Format(ExceptionMessages.CurlOperationReturnedErrorCode, (int)resultCode));
                        break;

                }

            }

            return sb.ToString();

        }

    }

}