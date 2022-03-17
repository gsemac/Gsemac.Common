namespace Gsemac.IO {

    public class TemporaryPathOptions :
        ITemporaryPathOptions {

        // Public members

        public bool EnsureUnique { get; set; } = false;

        public static TemporaryPathOptions Default => new TemporaryPathOptions();

    }

}