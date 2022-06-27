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

                            lastProperty = new IniProperty(token.Value);

                            // Get the last section by name rather than using the object directly.
                            // This is because the section we added may have been merged with an existing section.

                            if (lastSection is object)
                                result[lastSection.Name].Properties.Add(lastProperty);
                            else
                                result.Global.Properties.Add(lastProperty);

                            break;

                        case IniLexerTokenType.PropertyValue:

                            // Get the last property by name rather than using the object directly.
                            // This is because the property we added may have been merged with an existing property.

                            if (lastProperty is object) {

                                if (lastSection is object)
                                    result[lastSection.Name].Properties.Add(lastProperty.Name, token.Value);
                                else
                                    result.Global.Properties.Add(lastProperty.Name, token.Value);

                            }

                            break;

                    }

                }

            }

            return result;

        }

    }

}