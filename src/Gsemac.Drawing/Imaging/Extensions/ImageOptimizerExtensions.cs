using System.IO;

namespace Gsemac.Drawing.Imaging.Extensions {

    public static class ImageOptimizerExtensions {

        public static bool Optimize(this IImageOptimizer imageOptimizer, string inputFilePath, string outputFilePath, ImageOptimizationMode optimizationMode) {

            using (MemoryStream ms = new MemoryStream()) {

                using (FileStream inputStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
                    inputStream.CopyTo(ms);

                ms.Seek(0, SeekOrigin.Begin);

                bool success = imageOptimizer.Optimize(ms, optimizationMode);

                if (success) {

                    ms.Seek(0, SeekOrigin.Begin);

                    using (FileStream outputStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
                        ms.CopyTo(outputStream);

                }

                return success;

            }


        }

    }

}