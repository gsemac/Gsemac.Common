using System.Collections.Generic;

namespace Gsemac.IO {

    public interface IHasSupportedFileFormats {

        IEnumerable<ICodecCapabilities> GetSupportedFileFormats();

    }

}