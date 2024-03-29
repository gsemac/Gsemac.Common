﻿namespace Gsemac.IO {

    public class PathInfo :
        IPathInfo {

        public bool? IsUrl { get; set; }
        public bool? IsRooted { get; set; }

        public static PathInfo Default => new PathInfo();

    }

}