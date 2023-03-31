using System.Collections.Generic;

namespace Gsemac.IO {

    public interface IFileDialogFilterStringBuilder {

        IFileDialogFilterStringBuilder WithFileFormat(IFileFormat format);
        IFileDialogFilterStringBuilder WithFileFormat(string name, string extension);
        IFileDialogFilterStringBuilder WithFileFormat(string name, IEnumerable<string> extensions);
        IFileDialogFilterStringBuilder WithAllFilesOption();

        string Build();

    }

}