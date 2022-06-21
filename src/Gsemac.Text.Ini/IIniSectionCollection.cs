using System.Collections.Generic;

namespace Gsemac.Text.Ini {

    public interface IIniSectionCollection :
        ICollection<IIniSection> {

        IIniSection this[string sectionName] { get; }

        void Add(string sectionName);
        bool Remove(string sectionName);

        bool Contains(string sectionName);

        IIniSection Get(string sectionName);

    }

}