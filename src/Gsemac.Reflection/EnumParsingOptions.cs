namespace Gsemac.Reflection {

    public class EnumParsingOptions :
        IEnumParsingOptions {

        // Public members

        public bool IgnoreCase { get; set; } = false;

        public static EnumParsingOptions Default => new EnumParsingOptions();

    }

}