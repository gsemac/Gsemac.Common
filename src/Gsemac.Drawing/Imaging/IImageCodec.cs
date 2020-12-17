namespace Gsemac.Drawing.Imaging {

    public interface IImageCodec :
        IImageEncoder,
        IImageDecoder,
        IHasSupportedImageFormats {
    }

}