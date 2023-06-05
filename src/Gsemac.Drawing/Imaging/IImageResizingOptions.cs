namespace Gsemac.Drawing.Imaging {

    public interface IImageResizingOptions {

        int? Width { get; }
        int? Height { get; }
        double? HorizontalScale { get; }
        double? VerticalScale { get; }
        ImageSizingMode SizingMode { get; }
        bool MaintainAspectRatio { get; }

    }

}