using Gsemac.Text.Ini.Lexers;
using System.IO;

namespace Gsemac.Text.Ini {

    public class IniFactory :
        IIniFactory {

        // Public members

        public IIni FromStream(Stream stream, IIniOptions options) {

            Ini result = new Ini(options);

            IIniSection lastSection = null;
            IIniProperty lastProperty = null;

            using (IIniLexer lexer = new IniLexer(stream)) {

                while (lexer.ReadToken(out IIniLexerToken token)) {

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

                            if (!(lastProperty is null))
                                lastProperty.Value = token.Value;

                            break;

                    }

                }

            }

            return result;

        }

    }

}