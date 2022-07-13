namespace Gsemac.Reflection {

    public class CastOptions :
        ICastOptions {

        // Public members

        public bool IgnoreCase { get; set; } = true;
        public bool EnableConstructor { get; set; } = false;

        public static CastOptions Default => new CastOptions();

    }

}