using System;
using System.IO;
using System.Text;

namespace Gsemac.Text.Ini.Extensions {

    public static class IniFactoryExtensions {

        // Public members

        public static IIni Parse(this IIniFactory iniFactory, string iniString) {

            if (iniFactory is null)
                throw new ArgumentNullException(nameof(iniFactory));

            return Parse(iniFactory, iniString, IniOptions.Default);

        }
        public static IIni Parse(this IIniFactory iniFactory, string iniString, IIniOptions options) {

            if (iniFactory is null)
                throw new ArgumentNullException(nameof(iniFactory));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(iniString)))
                return iniFactory.FromStream(ms, options);

        }
        public static IIni FromFile(this IIniFactory iniFactory, string filePath) {

            if (iniFactory is null)
                throw new ArgumentNullException(nameof(iniFactory));

            return FromFile(iniFactory, filePath, IniOptions.Default);

        }
        public static IIni FromFile(this IIniFactory iniFactory, string filePath, IIniOptions options) {

            if (iniFactory is null)
                throw new ArgumentNullException(nameof(iniFactory));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            using (FileStream fs = new FileStream(filePath, FileMode.Open))
                return iniFactory.FromStream(fs, options);

        }
        public static IIni FromStream(this IIniFactory iniFactory, Stream stream) {

            if (iniFactory is null)
                throw new ArgumentNullException(nameof(iniFactory));

            return iniFactory.FromStream(stream, IniOptions.Default);

        }

    }

}