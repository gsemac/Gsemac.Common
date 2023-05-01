using Gsemac.IO;
using Gsemac.IO.Extensions;
using Gsemac.Reflection.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    public class ImageCodecFactory :
        IImageCodecFactory {

        // Public members

        public static ImageCodecFactory Default => new ImageCodecFactory();

        public ImageCodecFactory() :
            this(null) {
        }
        public ImageCodecFactory(IPluginLoader pluginLoader) {

            if (pluginLoader is null)
                this.pluginLoader = new Lazy<IPluginLoader>(CreateDefaultPluginLoader);
            else
                this.pluginLoader = new Lazy<IPluginLoader>(() => pluginLoader);

        }

        public IEnumerable<ICodecCapabilities> GetSupportedFileFormats() {

            return GetSupportedImageFormats();

        }

        public IImageCodec FromFileFormat(IFileFormat imageFormat) {

            return GetImageCodecs(imageFormat).FirstOrDefault(codec => codec.IsSupportedFileFormat(imageFormat));

        }

        // Private members

        private readonly Lazy<IPluginLoader> pluginLoader;

        private IPluginLoader CreateDefaultPluginLoader() {

            return new PluginLoader<IImageCodec>(new PluginLoaderOptions() {
                PluginSearchPattern = "Gsemac.Drawing.Imaging.*.dll",
            });

        }
        private IEnumerable<IImageCodec> GetImageCodecs() {

            return GetImageCodecs(null);

        }
        private IEnumerable<ICodecCapabilities> GetSupportedImageFormats() {

            return CodecCapabilities.Flatten(GetImageCodecs().SelectMany(codec => codec.GetSupportedFileFormats()))
                .OrderBy(type => type);

        }
        private IEnumerable<IImageCodec> GetImageCodecs(IFileFormat imageFormat) {

            List<IImageCodec> imageCodecs = new List<IImageCodec>();

            foreach (IImageCodec imageCodec in pluginLoader.Value.GetPlugins<IImageCodec>()) {

                IImageCodec nextImageCodec = imageCodec;
                Type nextImageCodecType = imageCodec.GetType();

                if (!(nextImageCodec is null) && !(imageFormat is null) && nextImageCodec.IsSupportedFileFormat(imageFormat) && nextImageCodecType.GetConstructor(new[] { typeof(IFileFormat) }) != null)
                    nextImageCodec = (IImageCodec)Activator.CreateInstance(nextImageCodecType, new object[] { imageFormat });

                imageCodecs.Add(nextImageCodec);

            }

            return imageCodecs;

        }

    }

}