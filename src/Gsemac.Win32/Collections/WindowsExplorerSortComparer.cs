using Gsemac.Win32;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.Collections {

    public class WindowsExplorerSortComparer :
        IComparer<string>,
        IComparer<FileInfo> {

        public int Compare(string x, string y) {

            x = x ?? "";
            y = y ?? "";

            return SHLWAPI.StrCmpLogicalW(x, y);

        }
        public int Compare(FileInfo x, FileInfo y) {

            return Compare(x.FullName, y.FullName);

        }

    }

}