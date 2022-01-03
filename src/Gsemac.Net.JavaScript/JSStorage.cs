using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Net.JavaScript {

    public class JSStorage :
        IJSStorage {

        // Public members

        public int Length => items.Count;

        public string Key(int index) {

            if (index < 0 || index >= items.Keys.Count)
                return null;

            return items.Keys.ElementAt(index);

        }
        public string GetItem(string keyName) {

            if (items.TryGetValue(keyName, out string keyValue))
                return keyValue;

            return null;

        }
        public void SetItem(string keyName, string keyValue) {

            items[keyName] = keyValue;

        }
        public void RemoveItem(string keyName) {

            items.Remove(keyName);

        }
        public void Clear() {

            items.Clear();

        }

        // Private members

        private readonly IDictionary<string, string> items = new Dictionary<string, string>();

    }

}