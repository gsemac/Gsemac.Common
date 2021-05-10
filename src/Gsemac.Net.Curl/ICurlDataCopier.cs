using System;

namespace Gsemac.Net.Curl {

    public interface ICurlDataCopier :
        IDisposable {

        WriteFunctionCallback Header { get; }
        ReadFunctionCallback Read { get; }
        WriteFunctionCallback Write { get; }
        ProgressFunctionCallback Progress { get; }

    }

}