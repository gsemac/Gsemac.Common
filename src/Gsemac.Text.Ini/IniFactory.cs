using Gsemac.Text.Ini.Lexers;
using System;
using System.IO;

namespace Gsemac.Text.Ini {

    public class IniFactory :
        IIniFactory {

        // Public members

        public static IniFactory Default => new IniFactory();

        public IIni FromStream(Stream stream, IIniOptions options) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            Ini result = new Ini(options);

            IIniSection lastSection = null;
            IIniProperty lastProperty = null;

            using (IIniLexer lexer = new IniLexer(stream, options)) {

                while (lexer.Read(out IIniLexerToken token)) {

                    switch (token.Type) {

                        case IniLexerTokenType.SectionName:

                            lastSection = new IniSection(token.Value);

                            result.Sections.Add(lastSection);

                            break;

                        case IniLexerTokenType.PropertyName:

                            lastProperty = new IniProperty(token.Value, string.Empty);

                            (lastSection ?? result.Global).Properties.Add(lastProperty);

                            break;

                        case IniLexerTokenType.PropertyValue:

                            if (lastProperty is object)
                                lastProperty.Value = token.Value;

                            break;

                    }

                }

            }

            return result;

        }

    }

}