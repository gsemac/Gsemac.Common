using System.Collections.Generic;

namespace Gsemac.Text.Ini {

    public interface IIniDocument :
         IEnumerable<IIniSection> {

        IIniSection this[string key] { get; set; }

        IIniSection DefaultSection { get; }

        void AddSection(IIniSection section);
        IIniSection GetSection(string name);
        bool RemoveSection(string name);

    }

}