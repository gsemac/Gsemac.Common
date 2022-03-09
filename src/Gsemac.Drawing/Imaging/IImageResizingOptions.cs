namespace Gsemac.Drawing.Imaging {

    public interface IImageResizingOptions {

        int? Width { get; }
        int? Height { get; }
        float? HorizontalScale { get; }
        float? VerticalScale { get; }
        ImageSizingMode SizingMode { get; }

    }

}