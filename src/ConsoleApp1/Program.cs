using Gsemac.Core;
using Gsemac.Net;
using Gsemac.Net.Curl;
using Gsemac.Net.Extensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace ConsoleApp1 {
    class Program {

        static void Main(string[] args) {

            using (WebClient client = new LibCurlWebClient()) {

                client.DownloadProgressChanged += (sender, e) => {
                    Console.WriteLine($"Bytes downloaded: {e.BytesReceived}");
                };

                client.DownloadFileCompleted += (sender, e) => {
                    Console.WriteLine("Download complete");
                };

                while (true) {

                    client.DownloadFile(new Uri("https://github.com/404/archive/master.zip"));

                    Console.ReadKey();

                }

                // ...

            }

            using (WebClient client = new BinCurlWebClient()) {

                client.DownloadProgressChanged += (sender, e) => {
                    Console.WriteLine($"Bytes downloaded: {e.BytesReceived}");
                };

                client.DownloadFileCompleted += (sender, e) => {
                    Console.WriteLine("Download complete");
                };

                client.DownloadFile(new Uri("https://github.com/gsemac/CurlWebClient/archive/master.zip"));

                Console.ReadKey();

                // ...

            }

            //BinCurlHttpWebRequest request = new BinCurlHttpWebRequest(new Uri("https://httpbin.org"));
            //Console.WriteLine(request.CurlArguments);
            //WebResponse response = request.GetResponse();

            //using (var sr = new StreamReader(response.GetResponseStream()))
            //    Console.WriteLine(sr.ReadToEnd());

            ////using (FileStream fs = new FileStream("config.ini", FileMode.Open))
            ////using (IIniLexer lexer = new IniLexer(fs) { AllowComments = false, Unescape = true })
            ////    while (lexer.ReadNextToken(out IIniLexerToken token))
            ////        Console.WriteLine(token.ToString());



            //IImageConverter imageConverter = new SystemDrawingImageConverter();
            //IImageConversionOptions options = new ImageConversionOptions();

            //options.Filters.Add(new ResizeImageFilter(width: 2000, options: ImageSizingMode.ResizeIfSmaller));
            //options.Filters.Add(new GrayscaleImageFilter());

            //imageConverter.ConvertImage("image0.jpg", "test.resized.webp", options);

            Console.WriteLine("OK");

            Console.ReadKey();


        }

    }
}