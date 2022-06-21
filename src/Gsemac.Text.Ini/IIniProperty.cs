using System.Collections.Generic;

namespace Gsemac.Text.Ini {

    public interface IIniProperty {

        string Name { get; }
        string Value { get; set; }
        ICollection<string> Values { get; }

    }

}