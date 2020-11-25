using System.Collections.Generic;

namespace Gsemac.Text.Ini {

    public interface IIniData :
         IEnumerable<IIniSection> {

        IIniSection this[string key] { get; set; }

        void AddSection(IIniSection section);
        IIniSection GetSection(string name);
        bool RemoveSection(string name);

    }

}