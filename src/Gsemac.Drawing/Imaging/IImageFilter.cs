namespace Gsemac.Drawing.Imaging {

    public interface IImageFilter {

        IImage Apply(IImage sourceImage);

    }

}