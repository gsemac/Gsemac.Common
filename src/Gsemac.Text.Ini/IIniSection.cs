using System.Collections.Generic;

namespace Gsemac.Text.Ini {

    public interface IIniSection :
        IEnumerable<IIniProperty> {

        string this[string propertyName] { get; set; }

        string Name { get; }
        string Comment { get; set; }
        IIniPropertyCollection Properties { get; }
        IIniSectionCollection Sections { get; }

    }

}