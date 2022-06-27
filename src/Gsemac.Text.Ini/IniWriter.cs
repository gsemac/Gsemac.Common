using System;
using System.IO;
using System.Text;
using static Gsemac.Text.Ini.IniConstants;

namespace Gsemac.Text.Ini {

    public class IniWriter :
        IIniWriter {

        // Public members

        public IniWriter() :
            this(IniOptions.Default) {
        }
        public IniWriter(IIniOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            stream = new MemoryStream();
            writer = new StreamWriter(stream, Encoding.UTF8);
            this.options = options;

            ownsWriter = true;

        }
        public IniWriter(Stream stream) :
            this(new StreamWriter(stream, Encoding.UTF8)) {
        }
        public IniWriter(Stream stream, IIniOptions options) :
            this(new StreamWriter(stream, Encoding.UTF8), options) {
        }
        public IniWriter(TextWriter writer) :
            this(writer, IniOptions.Default) {
        }
        public IniWriter(TextWriter writer, IIniOptions options) {

            if (writer is null)
                throw new ArgumentNullException(nameof(writer));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            this.writer = writer;
            this.options = options;

        }

        public void WriteSectionStart() {

            if (lastAction == LastAction.WroteProperty)
                writer.WriteLine();

            writer.Write(SectionNameStart);

        }
        public void WriteSectionName(string value) {

            if (options.EnableEscapeSequences)
                IniUtilities.Escape(value);

            writer.Write(value);

        }
        public void WriteSectionEnd() {

            writer.WriteLine(SectionNameEnd);

            lastAction = LastAction.WroteSection;

        }

        public void WritePropertyName(string value) {

            if (options.EnableEscapeSequences)
                IniUtilities.Escape(value);

            writer.Write(value);

        }
        public void WriteNameValueSeparator() {

            writer.Write(options.NameValueSeparator);

        }
        public void WritePropertyValue(string value) {

            if (options.EnableEscapeSequences)
                IniUtilities.Escape(value);

            writer.WriteLine(value);

            lastAction = LastAction.WroteProperty;

        }

        public void WriteComment(string value) {

            writer.Write(options.CommentMarker);
            writer.Write(' ');
            writer.WriteLine(value);

        }

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

        public override string ToString() {

            writer.Flush();

            long prevPosition = stream.Position;

            stream.Seek(0, SeekOrigin.Begin);

            string result = StringUtilities.StreamToString(stream);

            stream.Seek(prevPosition, SeekOrigin.Begin);

            return result;

        }

        // Protected members

        protected virtual void Dispose(bool disposing) {

            if (!disposedValue) {

                if (disposing) {

                    if (ownsWriter) {

                        writer.Dispose();
                        stream.Dispose();

                    }

                }

                disposedValue = true;

            }

        }

        // Private members

        private enum LastAction {
            None,
            WroteSection,
            WroteProperty,
        }

        private readonly Stream stream;
        private readonly TextWriter writer;
        private readonly IIniOptions options;
        private readonly bool ownsWriter;

        private LastAction lastAction = LastAction.None;
        private bool disposedValue;

    }

}