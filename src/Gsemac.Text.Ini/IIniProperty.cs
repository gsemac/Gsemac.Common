using System.Collections.Generic;

namespace Gsemac.Text.Ini {

    public interface IIniProperty {

        string Comment { get; set; }
        string Name { get; }
        string Value { get; set; }
        ICollection<string> Values { get; }

    }

}