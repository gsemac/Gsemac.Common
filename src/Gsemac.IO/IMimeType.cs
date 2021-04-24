using System.Collections.Generic;

namespace Gsemac.IO {

    public interface IMimeType {

        string Type { get; }
        string Subtype { get; }
        IDictionary<string, string> Parameters { get; }

    }

}