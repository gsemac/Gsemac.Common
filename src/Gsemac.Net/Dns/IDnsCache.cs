namespace Gsemac.Net.Dns {

    internal interface IDnsCache {

        bool TryGetRecords(string name, out IDnsAnswer[] records);
        void AddRecords(string name, IDnsAnswer[] records);

    }

}