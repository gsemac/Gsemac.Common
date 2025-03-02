using System;

namespace Gsemac.Net.Curl.Native {

    internal interface ICurlDataCopier :
        IDisposable {

        WriteFunctionCallback Header { get; }
        ReadFunctionCallback Read { get; }
        WriteFunctionCallback Write { get; }
        ProgressFunctionCallback Progress { get; }

    }

}