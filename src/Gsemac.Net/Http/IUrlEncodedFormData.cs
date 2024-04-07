namespace Gsemac.Net.Http {

    public interface IUrlEncodedFormData {

        string this[string key] { get; set; }

        void Add(string key, string value);

        byte[] ToBytes();

    }

}