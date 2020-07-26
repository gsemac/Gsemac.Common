using System;

namespace Gsemac.Utilities {

    public static class FileUtilities {

        public static bool TryDelete(string filePath) {

            if (!System.IO.File.Exists(filePath))
                return true;

            try {

                System.IO.File.Delete(filePath);

                return true;

            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception) {

                return false;

            }
#pragma warning restore CA1031 // Do not catch general exception types

        }

    }

}