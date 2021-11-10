using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;

namespace Gsemac.Net {

    internal sealed class WebClientAdapter :
        IWebClient {

        // Public members

        public string BaseAddress {
            get => webClient.BaseAddress;
            set => webClient.BaseAddress = value;
        }
        public ICredentials Credentials {
            get => webClient.Credentials;
            set => webClient.Credentials = value;
        }
        public bool UseDefaultCredentials {
            get => webClient.UseDefaultCredentials;
            set => webClient.UseDefaultCredentials = value;
        }
        public WebHeaderCollection Headers {
            get => webClient.Headers;
            set => webClient.Headers = value;
        }
        public NameValueCollection QueryString {
            get => webClient.QueryString;
            set => webClient.QueryString = value;
        }
        public WebHeaderCollection ResponseHeaders => webClient.ResponseHeaders;
        public IWebProxy Proxy {
            get => webClient.Proxy;
            set => webClient.Proxy = value;
        }
        public RequestCachePolicy CachePolicy {
            get => webClient.CachePolicy;
            set => webClient.CachePolicy = value;
        }
        public Encoding Encoding {
            get => webClient.Encoding;
            set => webClient.Encoding = value;
        }
        public bool IsBusy => webClient.IsBusy;

        public event OpenWriteCompletedEventHandler OpenWriteCompleted {
            add => webClient.OpenWriteCompleted += value;
            remove => webClient.OpenWriteCompleted -= value;
        }
        public event DownloadStringCompletedEventHandler DownloadStringCompleted {
            add => webClient.DownloadStringCompleted += value;
            remove => webClient.DownloadStringCompleted -= value;
        }
        public event DownloadDataCompletedEventHandler DownloadDataCompleted {
            add => webClient.DownloadDataCompleted += value;
            remove => webClient.DownloadDataCompleted -= value;
        }
        public event AsyncCompletedEventHandler DownloadFileCompleted {
            add => webClient.DownloadFileCompleted += value;
            remove => webClient.DownloadFileCompleted -= value;
        }
        public event UploadStringCompletedEventHandler UploadStringCompleted {
            add => webClient.UploadStringCompleted += value;
            remove => webClient.UploadStringCompleted -= value;
        }
        public event UploadDataCompletedEventHandler UploadDataCompleted {
            add => webClient.UploadDataCompleted += value;
            remove => webClient.UploadDataCompleted -= value;
        }
        public event UploadFileCompletedEventHandler UploadFileCompleted {
            add => webClient.UploadFileCompleted += value;
            remove => webClient.UploadFileCompleted -= value;
        }
        public event UploadValuesCompletedEventHandler UploadValuesCompleted {
            add => webClient.UploadValuesCompleted += value;
            remove => webClient.UploadValuesCompleted -= value;
        }
        public event OpenReadCompletedEventHandler OpenReadCompleted {
            add => webClient.OpenReadCompleted += value;
            remove => webClient.OpenReadCompleted -= value;
        }
        public event System.Net.DownloadProgressChangedEventHandler DownloadProgressChanged {
            add => webClient.DownloadProgressChanged += value;
            remove => webClient.DownloadProgressChanged -= value;
        }
        public event UploadProgressChangedEventHandler UploadProgressChanged {
            add => webClient.UploadProgressChanged += value;
            remove => webClient.UploadProgressChanged -= value;
        }

        public WebClientAdapter(WebClient webClient) {

            this.webClient = webClient;

        }

        public void CancelAsync() => webClient.CancelAsync();
        public void Dispose() => webClient.Dispose();
        public byte[] DownloadData(string address) => webClient.DownloadData(address);
        public byte[] DownloadData(Uri address) => webClient.DownloadData(address);
        public void DownloadDataAsync(Uri address) => webClient.DownloadDataAsync(address);
        public void DownloadDataAsync(Uri address, object userToken) => webClient.DownloadDataAsync(address, userToken);
        public void DownloadFile(string address, string fileName) => webClient.DownloadFile(address, fileName);
        public void DownloadFile(Uri address, string fileName) => webClient.DownloadFile(address, fileName);
        public void DownloadFileAsync(Uri address, string fileName, object userToken) => webClient.DownloadFileAsync(address, fileName, userToken);
        public void DownloadFileAsync(Uri address, string fileName) => webClient.DownloadFileAsync(address, fileName);
        public string DownloadString(string address) => webClient.DownloadString(address);
        public string DownloadString(Uri address) => webClient.DownloadString(address);
        public void DownloadStringAsync(Uri address, object userToken) => webClient.DownloadStringAsync(address, userToken);
        public void DownloadStringAsync(Uri address) => webClient.DownloadStringAsync(address);
        public Stream OpenRead(string address) => webClient.OpenRead(address);
        public Stream OpenRead(Uri address) => webClient.OpenRead(address);
        public void OpenReadAsync(Uri address) => webClient.OpenReadAsync(address);
        public void OpenReadAsync(Uri address, object userToken) => webClient.OpenReadAsync(address, userToken);
        public Stream OpenWrite(string address) => webClient.OpenWrite(address);
        public Stream OpenWrite(string address, string method) => webClient.OpenWrite(address, method);
        public Stream OpenWrite(Uri address, string method) => webClient.OpenWrite(address, method);
        public Stream OpenWrite(Uri address) => webClient.OpenWrite(address);
        public void OpenWriteAsync(Uri address) => webClient.OpenWriteAsync(address);
        public void OpenWriteAsync(Uri address, string method, object userToken) => webClient.OpenWriteAsync(address, method, userToken);
        public void OpenWriteAsync(Uri address, string method) => webClient.OpenWriteAsync(address, method);
        public byte[] UploadData(string address, byte[] data) => webClient.UploadData(address, data);
        public byte[] UploadData(string address, string method, byte[] data) => webClient.UploadData(address, method, data);
        public byte[] UploadData(Uri address, string method, byte[] data) => webClient.UploadData(address, method, data);
        public byte[] UploadData(Uri address, byte[] data) => webClient.UploadData(address, data);
        public void UploadDataAsync(Uri address, string method, byte[] data, object userToken) => webClient.UploadDataAsync(address, method, data, userToken);
        public void UploadDataAsync(Uri address, byte[] data) => webClient.UploadDataAsync(address, data);
        public void UploadDataAsync(Uri address, string method, byte[] data) => webClient.UploadDataAsync(address, method, data);
        public byte[] UploadFile(string address, string method, string fileName) => webClient.UploadFile(address, method, fileName);
        public byte[] UploadFile(Uri address, string method, string fileName) => webClient.UploadFile(address, method, fileName);
        public byte[] UploadFile(Uri address, string fileName) => webClient.UploadFile(address, fileName);
        public byte[] UploadFile(string address, string fileName) => webClient.UploadFile(address, fileName);
        public void UploadFileAsync(Uri address, string fileName) => webClient.UploadFileAsync(address, fileName);
        public void UploadFileAsync(Uri address, string method, string fileName) => webClient.UploadFileAsync(address, method, fileName);
        public void UploadFileAsync(Uri address, string method, string fileName, object userToken) => webClient.UploadFileAsync(address, method, fileName, userToken);
        public string UploadString(Uri address, string method, string data) => webClient.UploadString(address, method, data);
        public string UploadString(string address, string method, string data) => webClient.UploadString(address, method, data);
        public string UploadString(string address, string data) => webClient.UploadString(address, data);
        public string UploadString(Uri address, string data) => webClient.UploadString(address, data);
        public void UploadStringAsync(Uri address, string method, string data, object userToken) => webClient.UploadStringAsync(address, method, data, userToken);
        public void UploadStringAsync(Uri address, string method, string data) => webClient.UploadStringAsync(address, method, data);
        public void UploadStringAsync(Uri address, string data) => webClient.UploadStringAsync(address, data);
        public byte[] UploadValues(string address, string method, NameValueCollection data) => webClient.UploadValues(address, method, data);
        public byte[] UploadValues(Uri address, NameValueCollection data) => webClient.UploadValues(address, data);
        public byte[] UploadValues(Uri address, string method, NameValueCollection data) => webClient.UploadValues(address, method, data);
        public byte[] UploadValues(string address, NameValueCollection data) => webClient.UploadValues(address, data);
        public void UploadValuesAsync(Uri address, string method, NameValueCollection data) => webClient.UploadValuesAsync(address, method, data);
        public void UploadValuesAsync(Uri address, NameValueCollection data) => webClient.UploadValuesAsync(address, data);
        public void UploadValuesAsync(Uri address, string method, NameValueCollection data, object userToken) => webClient.UploadValuesAsync(address, method, data, userToken);

        // Internal members

        internal WebClient InnerWebClient => webClient;

        // Private members

        private readonly WebClient webClient;

    }

}