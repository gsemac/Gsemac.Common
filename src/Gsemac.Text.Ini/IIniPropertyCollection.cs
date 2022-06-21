using System.Collections.Generic;

namespace Gsemac.Text.Ini {

    public interface IIniPropertyCollection :
        ICollection<IIniProperty> {

        IIniProperty this[string propertyName] { get; }

        void Add(string propertyName, string propertyValue);
        bool Remove(string propertyName);

        bool Contains(string propertyName);

        IIniProperty Get(string propertyName);
        void Set(string propertyName, string propertyValue);

    }

}