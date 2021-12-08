using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gsemac.IO.FileFormats {

    public static class TextFormat {

        public static IFileFormat Html => new HtmlFileFormat();

        public static IEnumerable<IFileFormat> GetFormats() {

            return typeof(TextFormat).GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Select(property => property.GetValue(null, null))
                .Cast<IFileFormat>();

        }

    }

}