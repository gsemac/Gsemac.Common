using System;

namespace Gsemac.Net.Http {

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ResponseStreamAlreadyDecompressedAttribute :
        Attribute {
    }

}