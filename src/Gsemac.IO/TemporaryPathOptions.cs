namespace Gsemac.IO {

    public class TemporaryPathOptions :
        ITemporaryPathOptions {

        // Public members

        public bool EnsureUnique { get; set; } = true;

        public static TemporaryPathOptions Default => new TemporaryPathOptions();

    }

}