namespace Gsemac.Reflection {

    public class CastOptions :
        ICastOptions {

        // Public members

        public bool IgnoreCase { get; set; } = true;
        public bool EnableConstructorInitialization { get; set; } = false;

        public static CastOptions Default => new CastOptions();

    }

}