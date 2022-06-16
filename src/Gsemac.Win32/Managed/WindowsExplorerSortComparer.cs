using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.Win32.Managed {

    public class WindowsExplorerSortComparer :
        IComparer,
        IComparer<string>,
        IComparer<FileInfo> {

        public int Compare(string x, string y) {

            x = x ?? "";
            y = y ?? "";

            return Shlwapi.StrCmpLogicalW(x, y);

        }
        public int Compare(FileInfo x, FileInfo y) {

            return Compare(x.FullName, y.FullName);

        }
        public int Compare(object x, object y) {

            if (x is FileInfo fileInfoX && y is FileInfo fileInfoY)
                return Compare(fileInfoX, fileInfoY);

            return Compare(x.ToString(), y.ToString());

        }

    }

}