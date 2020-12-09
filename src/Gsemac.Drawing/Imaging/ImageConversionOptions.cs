using System.Collections.Generic;

namespace Gsemac.Drawing.Imaging {

    public class ImageConversionOptions :
        IImageConversionOptions {

        public IImageEncoderOptions EncoderOptions { get; set; } = new ImageEncoderOptions();
        public ICollection<IImageFilter> Filters { get; } = new List<IImageFilter>();


    }

}