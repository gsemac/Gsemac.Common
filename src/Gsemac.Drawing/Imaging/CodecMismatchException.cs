using System;
using System.Runtime.Serialization;

namespace Gsemac.Drawing.Imaging {

    [Serializable]
    public class CodecMismatchException :
        Exception {

        // Public members

        public CodecMismatchException() :
            this(Properties.ExceptionMessages.IncorrectCodec) {
        }
        public CodecMismatchException(string message) :
            base(message) {
        }
        public CodecMismatchException(string message, Exception innerException) :
            base(message, innerException) {
        }

        // Protected members

        protected CodecMismatchException(SerializationInfo info, StreamingContext context) :
            base(info, context) {
        }

    }

}