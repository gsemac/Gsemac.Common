namespace Gsemac.Reflection {

    public class EnumParseOptions :
        IEnumParsingOptions {

        // Public members

        public bool IgnoreCase { get; set; } = false;

        public static EnumParseOptions Default => new EnumParseOptions();

    }

}