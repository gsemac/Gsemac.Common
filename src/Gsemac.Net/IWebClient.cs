using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;

namespace Gsemac.Net {

    public interface IWebClient :
        IDisposable {

        /// <inheritdoc cref="WebClient.BaseAddress"/>
        string BaseAddress { get; set; }
        /// <inheritdoc cref="WebClient.Credentials"/>
        ICredentials Credentials { get; set; }
        /// <inheritdoc cref="WebClient.UseDefaultCredentials"/>
        bool UseDefaultCredentials { get; set; }
        /// <inheritdoc cref="WebClient.Headers"/>
        WebHeaderCollection Headers { get; set; }
        /// <inheritdoc cref="WebClient.QueryString"/>
        NameValueCollection QueryString { get; set; }
        /// <inheritdoc cref="WebClient.ResponseHeaders"/>
        WebHeaderCollection ResponseHeaders { get; }
        /// <inheritdoc cref="WebClient.Proxy"/>
        IWebProxy Proxy { get; set; }
        /// <inheritdoc cref="WebClient.CachePolicy"/>
        RequestCachePolicy CachePolicy { get; set; }
        /// <inheritdoc cref="WebClient.Encoding"/>
        Encoding Encoding { get; set; }
        /// <inheritdoc cref="WebClient.IsBusy"/>
        bool IsBusy { get; }

        /// <inheritdoc cref="WebClient.OpenWriteCompleted"/>
        event OpenWriteCompletedEventHandler OpenWriteCompleted;
        /// <inheritdoc cref="WebClient.DownloadStringCompleted"/>
        event DownloadStringCompletedEventHandler DownloadStringCompleted;
        /// <inheritdoc cref="WebClient.DownloadDataCompleted"/>
        event DownloadDataCompletedEventHandler DownloadDataCompleted;
        /// <inheritdoc cref="WebClient.DownloadFileCompleted"/>
        event AsyncCompletedEventHandler DownloadFileCompleted;
        /// <inheritdoc cref="WebClient.UploadStringCompleted"/>
        event UploadStringCompletedEventHandler UploadStringCompleted;
        /// <inheritdoc cref="WebClient.UploadDataCompleted"/>
        event UploadDataCompletedEventHandler UploadDataCompleted;
        /// <inheritdoc cref="WebClient.UploadFileCompleted"/>
        event UploadFileCompletedEventHandler UploadFileCompleted;
        /// <inheritdoc cref="WebClient.UploadValuesCompleted"/>
        event UploadValuesCompletedEventHandler UploadValuesCompleted;
        /// <inheritdoc cref="WebClient.OpenReadCompleted"/>
        event OpenReadCompletedEventHandler OpenReadCompleted;
        /// <inheritdoc cref="WebClient.DownloadProgressChanged"/>
        event System.Net.DownloadProgressChangedEventHandler DownloadProgressChanged;
        /// <inheritdoc cref="WebClient.UploadProgressChanged"/>
        event UploadProgressChangedEventHandler UploadProgressChanged;

        /// <inheritdoc cref="WebClient.CancelAsync"/>
        void CancelAsync();
        /// <inheritdoc cref="WebClient.DownloadData(string)"/>
        byte[] DownloadData(string address);
        /// <inheritdoc cref="WebClient.DownloadData(Uri)"/>
        byte[] DownloadData(Uri address);
        /// <inheritdoc cref="WebClient.DownloadDataAsync(Uri)"/>
        void DownloadDataAsync(Uri address);
        /// <inheritdoc cref="WebClient.DownloadDataAsync(Uri, object)"/>
        void DownloadDataAsync(Uri address, object userToken);
        /// <inheritdoc cref="WebClient.DownloadFile(string, string)"/>
        void DownloadFile(string address, string fileName);
        /// <inheritdoc cref="WebClient.DownloadFile(Uri, string)"/>
        void DownloadFile(Uri address, string fileName);
        /// <inheritdoc cref="WebClient.DownloadFileAsync(Uri, string, object)"/>
        void DownloadFileAsync(Uri address, string fileName, object userToken);
        /// <inheritdoc cref="WebClient.DownloadFileAsync(Uri, string)"/>
        void DownloadFileAsync(Uri address, string fileName);
        /// <inheritdoc cref="WebClient.DownloadString(string)"/>
        string DownloadString(string address);
        /// <inheritdoc cref="WebClient.DownloadString(Uri)"/>
        string DownloadString(Uri address);
        /// <inheritdoc cref="WebClient.DownloadStringAsync(Uri, object)"/>
        void DownloadStringAsync(Uri address, object userToken);
        /// <inheritdoc cref="WebClient.DownloadStringAsync(Uri)"/>
        void DownloadStringAsync(Uri address);
        /// <inheritdoc cref="WebClient.OpenRead(string)"/>
        Stream OpenRead(string address);
        /// <inheritdoc cref="WebClient.OpenRead(Uri)"/>
        Stream OpenRead(Uri address);
        /// <inheritdoc cref="WebClient.OpenReadAsync(Uri)"/>
        void OpenReadAsync(Uri address);
        /// <inheritdoc cref="WebClient.OpenReadAsync(Uri, object)"/>
        void OpenReadAsync(Uri address, object userToken);
        /// <inheritdoc cref="WebClient.OpenWrite(string)"/>
        Stream OpenWrite(string address);
        /// <inheritdoc cref="WebClient.OpenWrite(string, string)"/>
        Stream OpenWrite(string address, string method);
        /// <inheritdoc cref="WebClient.OpenWrite(Uri, string)"/>
        Stream OpenWrite(Uri address, string method);
        /// <inheritdoc cref="WebClient.OpenWrite(Uri)"/>
        Stream OpenWrite(Uri address);
        /// <inheritdoc cref="WebClient.OpenWriteAsync(Uri)"/>
        void OpenWriteAsync(Uri address);
        /// <inheritdoc cref="WebClient.OpenWriteAsync(Uri, string, object)"/>
        void OpenWriteAsync(Uri address, string method, object userToken);
        /// <inheritdoc cref="WebClient.OpenWriteAsync(Uri, string)"/>
        void OpenWriteAsync(Uri address, string method);
        /// <inheritdoc cref="WebClient.UploadData(string, byte[])"/>
        byte[] UploadData(string address, byte[] data);
        /// <inheritdoc cref="WebClient.UploadData(string, string, byte[])"/>
        byte[] UploadData(string address, string method, byte[] data);
        /// <inheritdoc cref="WebClient.UploadData(Uri, string, byte[])"/>
        byte[] UploadData(Uri address, string method, byte[] data);
        /// <inheritdoc cref="WebClient.UploadData(Uri, byte[])"/>
        byte[] UploadData(Uri address, byte[] data);
        /// <inheritdoc cref="WebClient.UploadDataAsync(Uri, string, byte[], object)"/>
        void UploadDataAsync(Uri address, string method, byte[] data, object userToken);
        /// <inheritdoc cref="WebClient.UploadDataAsync(Uri, byte[])"/>
        void UploadDataAsync(Uri address, byte[] data);
        /// <inheritdoc cref="WebClient.UploadDataAsync(Uri, string, byte[])"/>
        void UploadDataAsync(Uri address, string method, byte[] data);
        /// <inheritdoc cref="WebClient.UploadFile(string, string, string)"/>
        byte[] UploadFile(string address, string method, string fileName);
        /// <inheritdoc cref="WebClient.UploadFile(Uri, string, string)"/>
        byte[] UploadFile(Uri address, string method, string fileName);
        /// <inheritdoc cref="WebClient.UploadFile(Uri, string)"/>
        byte[] UploadFile(Uri address, string fileName);
        /// <inheritdoc cref="WebClient.UploadFile(string, string)"/>
        byte[] UploadFile(string address, string fileName);
        /// <inheritdoc cref="WebClient.UploadFileAsync(Uri, string)"/>
        void UploadFileAsync(Uri address, string fileName);
        /// <inheritdoc cref="WebClient.UploadFileAsync(Uri, string, string)"/>
        void UploadFileAsync(Uri address, string method, string fileName);
        /// <inheritdoc cref="WebClient.UploadFileAsync(Uri, string, string, object)"/>
        void UploadFileAsync(Uri address, string method, string fileName, object userToken);
        /// <inheritdoc cref="WebClient.UploadString(Uri, string, string)"/>
        string UploadString(Uri address, string method, string data);
        /// <inheritdoc cref="WebClient.UploadString(string, string, string)"/>
        string UploadString(string address, string method, string data);
        /// <inheritdoc cref="WebClient.UploadString(string, string)"/>
        string UploadString(string address, string data);
        /// <inheritdoc cref="WebClient.UploadString(Uri, string)"/>
        string UploadString(Uri address, string data);
        /// <inheritdoc cref="WebClient.UploadStringAsync(Uri, string, string, object)"/>
        void UploadStringAsync(Uri address, string method, string data, object userToken);
        /// <inheritdoc cref="WebClient.UploadStringAsync(Uri, string, string)"/>
        void UploadStringAsync(Uri address, string method, string data);
        /// <inheritdoc cref="WebClient.UploadStringAsync(Uri, string)"/>
        void UploadStringAsync(Uri address, string data);
        /// <inheritdoc cref="WebClient.UploadValues(string, string, NameValueCollection)"/>
        byte[] UploadValues(string address, string method, NameValueCollection data);
        /// <inheritdoc cref="WebClient.UploadValues(Uri, NameValueCollection)"/>
        byte[] UploadValues(Uri address, NameValueCollection data);
        /// <inheritdoc cref="WebClient.UploadValues(Uri, string, NameValueCollection)"/>
        byte[] UploadValues(Uri address, string method, NameValueCollection data);
        /// <inheritdoc cref="WebClient.UploadValues(string, NameValueCollection)"/>
        byte[] UploadValues(string address, NameValueCollection data);
        /// <inheritdoc cref="WebClient.UploadValuesAsync(Uri, string, NameValueCollection)"/>
        void UploadValuesAsync(Uri address, string method, NameValueCollection data);
        /// <inheritdoc cref="WebClient.UploadValuesAsync(Uri, NameValueCollection)"/>
        void UploadValuesAsync(Uri address, NameValueCollection data);
        /// <inheritdoc cref="WebClient.UploadValuesAsync(Uri, string, NameValueCollection, object)"/>
        void UploadValuesAsync(Uri address, string method, NameValueCollection data, object userToken);

    }

}