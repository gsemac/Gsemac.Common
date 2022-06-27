using System.Collections.Generic;

namespace Gsemac.Text.Ini {

    public interface IIniSection :
        IEnumerable<IIniProperty> {

        string this[string propertyName] { get; set; }

        string Comment { get; set; }
        string Name { get; }
        IIniPropertyCollection Properties { get; }
        IIniSectionCollection Sections { get; }

    }

}