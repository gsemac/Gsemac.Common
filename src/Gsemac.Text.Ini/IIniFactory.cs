using System.IO;

namespace Gsemac.Text.Ini {

    public interface IIniFactory {

        IIni FromStream(Stream stream, IIniOptions options);

    }

}