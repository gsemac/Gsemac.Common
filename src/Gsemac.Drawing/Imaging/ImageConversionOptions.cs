﻿using System.Collections.Generic;

namespace Gsemac.Drawing.Imaging {

    public class ImageConversionOptions :
        IImageConversionOptions {

        public IImageEncoderOptions EncoderOptions { get; } = new ImageEncoderOptions();

#if NETFRAMEWORK
        public ICollection<IImageFilter> Filters { get; } = new List<IImageFilter>();
#endif

    }

}