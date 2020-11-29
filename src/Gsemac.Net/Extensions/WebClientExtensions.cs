using Gsemac.IO;
using System;
using System.Net;

namespace Gsemac.Net.Extensions {

    public static class WebClientExtensions {

        public static void DownloadFile(this WebClient client, Uri address) {

            client.DownloadFile(address, PathUtilities.GetFileName(address.AbsoluteUri));

        }
        public static void DownloadFileAsync(this WebClient client, Uri address) {

            client.DownloadFileAsync(address, PathUtilities.GetFileName(address.AbsoluteUri));

        }

    }

}
