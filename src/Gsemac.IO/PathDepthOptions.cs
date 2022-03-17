namespace Gsemac.IO {

    public class PathDepthOptions :
          IPathDepthOptions {

        // Public members

        public bool IgnoreTrailingDirectorySeparators { get; set; } = true;

        public static PathDepthOptions Default => new PathDepthOptions();

    }

}