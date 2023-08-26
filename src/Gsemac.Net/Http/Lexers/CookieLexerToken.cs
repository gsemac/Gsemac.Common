using Gsemac.Text.Lexers;

namespace Gsemac.Net.Http.Lexers {

    internal enum CookieLexerTokenType {
        Name,
        NameValueSeparator, // =
        Value,
        AttributeSeparator, // ;
        AttributeName,
        AttributeValueSeparator, // =
        AttributeValue,
        CookieSeparator, // ,
    }

    internal class CookieLexerToken :
        LexerTokenBase<CookieLexerTokenType> {

        // Public members

        public CookieLexerToken(CookieLexerTokenType type, string value) :
            base(type, value) {
        }

    }

}