using System.Collections.Generic;

namespace Gsemac.Text.Ini {

    public interface IIni :
         IEnumerable<IIniSection> {

        IIniSection this[string sectionName] { get; }

        IIniSection Global { get; }
        IIniSectionCollection Sections { get; }

    }

}