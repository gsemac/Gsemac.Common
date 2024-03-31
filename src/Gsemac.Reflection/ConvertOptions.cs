namespace Gsemac.Reflection {

    public class ConvertOptions :
        IConvertOptions {

        // Public members

        public bool IgnoreCase { get; set; } = true;
        public bool UseConstructor { get; set; } = false;

        public static ConvertOptions Default => new ConvertOptions();

    }

}