using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gsemac.IO.FileFormats {

    public static class DocumentFormat {

        public static IFileFormat Html => new HtmlFileFormat();
        public static IFileFormat Pdf => new PdfFileFormat();
        public static IFileFormat Text => new TextFileFormat();

        public static IEnumerable<IFileFormat> GetFormats() {

            return typeof(DocumentFormat).GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Select(property => property.GetValue(null, null))
                .Cast<IFileFormat>();

        }

    }

}