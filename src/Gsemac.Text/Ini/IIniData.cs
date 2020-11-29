using System.Collections.Generic;
using System.IO;

namespace Gsemac.Text.Ini {

    public interface IIniData :
         IEnumerable<IIniSection> {

        IIniSection this[string key] { get; set; }

        IIniSection DefaultSection { get; }

        void AddSection(IIniSection section);
        IIniSection GetSection(string name);
        bool RemoveSection(string name);

        void Save(string filePath);
        void Save(Stream stream);

    }

}