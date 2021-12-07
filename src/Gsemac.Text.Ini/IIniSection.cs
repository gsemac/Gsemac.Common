using System.Collections.Generic;

namespace Gsemac.Text.Ini {

    public interface IIniSection :
        IEnumerable<IIniProperty> {

        string this[string key] { get; set; }

        string Name { get; }

        void AddProperty(IIniProperty property);
        IIniProperty GetProperty(string name);
        bool RemoveProperty(string name);

    }

}