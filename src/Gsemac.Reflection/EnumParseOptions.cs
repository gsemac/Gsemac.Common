namespace Gsemac.Reflection {

    public class EnumParseOptions :
        IEnumParseOptions {

        // Public members

        public bool IgnoreCase { get; set; } = false;

        public static EnumParseOptions Default => new EnumParseOptions();

    }

}