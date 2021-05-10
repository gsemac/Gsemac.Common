namespace Gsemac.Net.Curl {

    public interface ICurlDataCopier {

        WriteFunctionCallback Header { get; }
        ReadFunctionCallback Read { get; }
        WriteFunctionCallback Write { get; }
        ProgressFunctionCallback Progress { get; }

    }

}