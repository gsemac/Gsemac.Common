using System;

namespace Gsemac.Text.Ini {

    internal interface IIniWriter :
        IDisposable {

        void WriteSectionStart();
        void WriteSectionName(string value);
        void WriteSectionEnd();

        void WritePropertyName(string value);
        void WriteNameValueSeparator();
        void WritePropertyValue(string value);

        void WriteComment(string value);

    }

}