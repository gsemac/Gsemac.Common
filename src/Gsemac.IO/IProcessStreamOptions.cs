namespace Gsemac.IO {

    public interface IProcessStreamOptions {

        /// <summary>
        /// Redirect Standard Input to the stream.
        /// </summary>
        bool RedirectStandardOutput { get; }
        /// <summary>
        /// Redirect writes to the stream to Standard Input.
        /// </summary>
        bool RedirectStandardInput { get; }
        /// <summary>
        /// Redirect Standard Error to the stream.
        /// </summary>
        bool RedirectStandardError { get; }
        /// <summary>
        /// Redirect Standard Output and Standard Error to the same stream.
        /// </summary>
        bool RedirectStandardErrorToStandardOutput { get; }
        bool ExitOnUserInputPrompt { get; }

    }

}