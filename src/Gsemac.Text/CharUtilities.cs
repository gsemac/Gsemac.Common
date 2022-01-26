using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Gsemac.Text {

    public static class CharUtilities {

        // Public members

        public static bool IsLeftEnclosingPunctuation(char value) {

            return enclosingPunctuationDict.Value.ContainsKey(value);

        }
        public static bool IsRightEnclosingPunctuation(char value) {

            return enclosingPunctuationDict.Value.Values.Contains(value);

        }
        public static bool IsEnclosingPunctuation(char value) {

            return IsLeftEnclosingPunctuation(value) ||
                IsRightEnclosingPunctuation(value);

        }
        public static char GetOppositeEnclosingPunctuation(char value) {

            if (enclosingPunctuationDict.Value.TryGetValue(value, out char complementChar))
                return complementChar;

            if (enclosingPunctuationDict.Value.ToDictionary(t => t.Value, t => t.Key).TryGetValue(value, out complementChar))
                return complementChar;

            return value;

        }

        public static bool IsNewLine(char value) {

            return value == '\r' || value == '\n';

        }
        public static bool IsTerminalPunctuation(char value) {

            switch (value) {

                case '.':
                case '!':
                case '?':
                    return true;

                default:
                    return false;

            }

        }
        public static bool IsWordCharacter(char value) {

            // Returns true if the character is a word character as defined by the regex metacharacter "\w".
            // Solution adapted from the one given here: https://stackoverflow.com/a/33809007/5383169 (thargy)

            UnicodeCategory category = char.GetUnicodeCategory(value);

            switch (category) {

                case UnicodeCategory.ConnectorPunctuation:
                case UnicodeCategory.DecimalDigitNumber:
                case UnicodeCategory.LowercaseLetter:
                case UnicodeCategory.ModifierLetter:
                case UnicodeCategory.NonSpacingMark:
                case UnicodeCategory.OtherLetter:
                case UnicodeCategory.SpacingCombiningMark:
                case UnicodeCategory.TitlecaseLetter:
                case UnicodeCategory.UppercaseLetter:
                    return true;

                default:
                    return false;

            }

        }

        // Private members

        private static readonly Lazy<IDictionary<char, char>> enclosingPunctuationDict = new Lazy<IDictionary<char, char>>(CreateEnclosingPunctuationDict);

        private static IDictionary<char, char> CreateEnclosingPunctuationDict() {

            // The following types of enclosing punctuation were found here: https://en.wikipedia.org/wiki/Bracket#Unicode

            return new Dictionary<char, char> {
                // General purpose
                {'(', ')' },
                {'[', ']' },
                // Technical/mathematical(common)
                {'<', '>' },
                {'{', '}' },
                // Quotation (Western texts)
                {'\'', '\'' },
                {'"', '"' },
                {'«', '»' },
                {'‹', '›' },
                {'“', '”' },
                {'‘', '’' },
                {'‚', '‘' },
                {'„', '“' },
                // Floor and ceiling functions
                { '⌈', '⌉' },
                { '⌊', '⌋' },
                // Quine corners
                { '⌜', '⌝' },
                { '⌞', '⌟' },
                // Technical/mathematical (specialized)
                { '⁽', '⁾' },
                { '₍', '₎' },
                { '⎛', '⎞' },
                { '⎜', '⎟' },
                { '⎝', '⎠' },
                { '⎡', '⎤' },
                { '⎢', '⎥' },
                { '⎣', '⎦' },
                { '⎧', '⎫' },
                { '⎨', '⎬' },
                { '⎩', '⎭' },
                { '⎪', '⎪' },
                { '⎰', '⎱' },
                { '⎱', '⎰' },
                { '⎴', '⎵' },
                { '⎸', '⎹' },
                { '⏜', '⏝' },
                { '⏞', '⏟' },
                { '⏠', '⏡' },
                { '⟅', '⟆' },
                { '⟓', '⟔' },
                {'⟦', '⟧' },
                {'⟨', '⟩' },
                {'⟪', '⟫' },
                {'⟬', '⟭' },
                {'⟮', '⟯' },
                {'⦃', '⦄' },
                {'⦅', '⦆' },
                {'⦇', '⦈' },
                {'⦉', '⦊' },
                {'⦋', '⦌' },
                {'⦍', '⦐' },
                {'⦏', '⦎' },
                {'⦑', '⦒' },
                {'⦓', '⦔' },
                {'⦕', '⦖' },
                {'⦗', '⦘' },
                {'⧘', '⧙' },
                {'⧚', '⧛' },
                {'⧼', '⧽' },
                // Half brackets
                {'⸢', '⸣' },
                {'⸤', '⸥' },
                // Dingbats
                { '❨', '❩' },
                {'❪', '❫' },
                {'❬', '❭' },
                {'❰', '❱' },
                {'❮', '❯' },
                {'❲', '❳' },
                {'❴', '❵' },
                // Arabic (Quranic quotations)
                {'﴾', '﴿' },
                // N'Ko
                {'⸝', '⸜' },
                // Ogham
                {'᚛', '᚜' },
                // Tibetan
                {'༺', '༻' },
                {'༼', '༽' },
                // New Testament editorial marks
                { '⸂', '⸃' },
                { '⸄', '⸅' },
                { '⸉', '⸊' },
                { '⸌', '⸍' },
                // Medieval studies
                { '⁅', '⁆' },
                { '⸦', '⸧' },
                { '⸨', '⸩' },
                // Quotation (East-Asian texts)
                {'〔', '〕' },
                {'〖', '〗' },
                {'〘', '〙' },
                {'〚', '〛' },
                {'〝', '〞' },
                // Quotation (halfwidth East-Asian texts)
                {'〈', '〉' },
                {'｢', '｣' },
                // Quotation (fullwidth East-Asian texts)
                {'《', '》' },
                {'「', '」' },
                {'『', '』' },
                {'【', '】' },
                // General purpose (fullwidth East-Asian)
                { '（', '）' },
                {'［', '］' },
                {'＜', '＞' },
                // Technical/mathematical (fullwidth East-Asian)
                {'｛', '｝' },
                {'｟', '｠' },
            };

        }

    }

}