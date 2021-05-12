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

        string BaseAddress { get; set; }
        ICredentials Credentials { get; set; }
        bool UseDefaultCredentials { get; set; }
        WebHeaderCollection Headers { get; set; }
        NameValueCollection QueryString { get; set; }
        WebHeaderCollection ResponseHeaders { get; }
        IWebProxy Proxy { get; set; }
        RequestCachePolicy CachePolicy { get; set; }
        Encoding Encoding { get; set; }
        bool IsBusy { get; }

        event OpenWriteCompletedEventHandler OpenWriteCompleted;
        event DownloadStringCompletedEventHandler DownloadStringCompleted;
        event DownloadDataCompletedEventHandler DownloadDataCompleted;
        event AsyncCompletedEventHandler DownloadFileCompleted;
        event UploadStringCompletedEventHandler UploadStringCompleted;
        event UploadDataCompletedEventHandler UploadDataCompleted;
        event UploadFileCompletedEventHandler UploadFileCompleted;
        event UploadValuesCompletedEventHandler UploadValuesCompleted;
        event OpenReadCompletedEventHandler OpenReadCompleted;
        event System.Net.DownloadProgressChangedEventHandler DownloadProgressChanged;
        event UploadProgressChangedEventHandler UploadProgressChanged;

        void CancelAsync();
        byte[] DownloadData(string address);
        byte[] DownloadData(Uri address);
        void DownloadDataAsync(Uri address);
        void DownloadDataAsync(Uri address, object userToken);
        void DownloadFile(string address, string fileName);
        void DownloadFile(Uri address, string fileName);
        void DownloadFileAsync(Uri address, string fileName, object userToken);
        void DownloadFileAsync(Uri address, string fileName);
        string DownloadString(string address);
        string DownloadString(Uri address);
        void DownloadStringAsync(Uri address, object userToken);
        void DownloadStringAsync(Uri address);
        Stream OpenRead(string address);
        Stream OpenRead(Uri address);
        void OpenReadAsync(Uri address);
        void OpenReadAsync(Uri address, object userToken);
        Stream OpenWrite(string address);
        Stream OpenWrite(string address, string method);
        Stream OpenWrite(Uri address, string method);
        Stream OpenWrite(Uri address);
        void OpenWriteAsync(Uri address);
        void OpenWriteAsync(Uri address, string method, object userToken);
        void OpenWriteAsync(Uri address, string method);
        byte[] UploadData(string address, byte[] data);
        byte[] UploadData(string address, string method, byte[] data);
        byte[] UploadData(Uri address, string method, byte[] data);
        byte[] UploadData(Uri address, byte[] data);
        void UploadDataAsync(Uri address, string method, byte[] data, object userToken);
        void UploadDataAsync(Uri address, byte[] data);
        void UploadDataAsync(Uri address, string method, byte[] data);
        byte[] UploadFile(string address, string method, string fileName);
        byte[] UploadFile(Uri address, string method, string fileName);
        byte[] UploadFile(Uri address, string fileName);
        byte[] UploadFile(string address, string fileName);
        void UploadFileAsync(Uri address, string fileName);
        void UploadFileAsync(Uri address, string method, string fileName);
        void UploadFileAsync(Uri address, string method, string fileName, object userToken);
        string UploadString(Uri address, string method, string data);
        string UploadString(string address, string method, string data);
        string UploadString(string address, string data);
        string UploadString(Uri address, string data);
        void UploadStringAsync(Uri address, string method, string data, object userToken);
        void UploadStringAsync(Uri address, string method, string data);
        void UploadStringAsync(Uri address, string data);
        byte[] UploadValues(string address, string method, NameValueCollection data);
        byte[] UploadValues(Uri address, NameValueCollection data);
        byte[] UploadValues(Uri address, string method, NameValueCollection data);
        byte[] UploadValues(string address, NameValueCollection data);
        void UploadValuesAsync(Uri address, string method, NameValueCollection data);
        void UploadValuesAsync(Uri address, NameValueCollection data);
        void UploadValuesAsync(Uri address, string method, NameValueCollection data, object userToken);

    }

}