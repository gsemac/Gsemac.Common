namespace Gsemac.Net.JavaScript {

    public interface IJSStorage {

        int Length { get; }

        string Key(int index);
        string GetItem(string keyName);
        void SetItem(string keyName, string keyValue);
        void RemoveItem(string keyName);
        void Clear();

    }

}