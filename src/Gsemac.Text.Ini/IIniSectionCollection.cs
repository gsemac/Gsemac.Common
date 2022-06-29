using System.Collections.Generic;

namespace Gsemac.Text.Ini {

    public interface IIniSectionCollection :
        ICollection<IIniSection> {

        IIniSection this[string sectionName] { get; }

        void Add(string name);
        bool Remove(string name);

        bool Contains(string name);

    }

}