namespace Gsemac.Drawing.Imaging {

    public interface IImageFilter {

        /// <summary>
        /// Applies the filter to the given image and returns a new <see cref="IImage"/>.<para></para>The source image is not modified or disposed.
        /// </summary>
        /// <param name="image">The image to which the filter is applied. The source image is not modified or disposed.</param>
        /// <returns>A new <see cref="IImage"/> with the filter applied.</returns>
        IImage Apply(IImage image);

    }

}