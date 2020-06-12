using Gsemac.Collections;
using Gsemac.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gsemac.Logging {

    public class LogHeader :
        ILogHeader {

        // Public members

        public string this[string key] {
            get => dict[key]();
            set => dict[key] = () => value;
        }
        public string this[LogHeaderKey key] {
            get => this[LogHeaderKeyToString(key)];
            set => this[LogHeaderKeyToString(key)] = value;
        }

        public ICollection<string> Keys => dict.Keys;
        public ICollection<string> Values => new LazyReadOnlyCollection<string>(dict.Values.Select(f => f()));

        public int Count => dict.Count;
        public bool IsReadOnly => false;

        public LogHeader() :
            this(true) {
        }
        public LogHeader(bool addDefaultHeaders) {

            if (addDefaultHeaders) {

                Add(LogHeaderKey.ProductVersion, () => Assembly.GetEntryAssembly().GetName().Version.ToString());
                Add(LogHeaderKey.ClrVersion, () => Environment.Version.ToString());
                Add(LogHeaderKey.OSVersion, () => Environment.OSVersion.ToString() + string.Format(" ({0})", Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit"));
                Add(LogHeaderKey.Locale, () => System.Globalization.CultureInfo.InstalledUICulture.Name);
                Add(LogHeaderKey.Path, () => PathUtilities.AnonymizePath(Assembly.GetEntryAssembly().Location));
                Add(LogHeaderKey.WorkingDirectory, () => PathUtilities.AnonymizePath(System.IO.Directory.GetCurrentDirectory()));
                Add(LogHeaderKey.Timestamp, () => DateTime.Now.ToString());

            }

        }

        public void Add(string key, Func<string> getter) {

            dict[key] = getter;

        }
        public void Add(LogHeaderKey key, Func<string> getter) {

            Add(LogHeaderKeyToString(key), getter);

        }
        public void Add(string key, string value) {

            Add(key, () => value);

        }
        public void Add(KeyValuePair<string, string> item) {

            Add(item.Key, () => item.Value);

        }
        public void Add(LogHeaderKey key, string value) {

            Add(key, () => value);

        }

        public void Clear() {

            dict.Clear();

        }

        public bool Contains(KeyValuePair<string, string> item) {

            return ContainsKey(item.Key);

        }

        public bool ContainsKey(string key) {

            return dict.ContainsKey(key);

        }
        public bool ContainsKey(LogHeaderKey key) {

            return ContainsKey(LogHeaderKeyToString(key));

        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex) => throw new NotImplementedException();

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() {

            return dict.Select(pair => new KeyValuePair<string, string>(pair.Key, pair.Value())).GetEnumerator();

        }

        public bool Remove(string key) {

            return dict.Remove(key);

        }
        public bool Remove(KeyValuePair<string, string> item) {

            return Remove(item.Key);

        }
        public bool Remove(LogHeaderKey key) {

            return Remove(LogHeaderKeyToString(key));

        }

        public bool TryGetValue(string key, out string value) {

            if (dict.TryGetValue(key, out Func<string> getter)) {

                value = getter();

                return true;

            }
            else {

                value = default;

                return false;

            }


        }
        public bool TryGetValue(LogHeaderKey key, out string value) {

            return TryGetValue(LogHeaderKeyToString(key), out value);

        }

        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        public static LogHeader Empty => new LogHeader(false);

        // Private members

        private readonly IDictionary<string, Func<string>> dict = new OrderedDictionary<string, Func<string>>();

        private string LogHeaderKeyToString(LogHeaderKey key) {

            switch (key) {

                case LogHeaderKey.ProductVersion:
                    return "Product Version";

                case LogHeaderKey.ClrVersion:
                    return "CLR Version";

                case LogHeaderKey.OSVersion:
                    return "OS Version";

                case LogHeaderKey.Locale:
                    return "Locale";

                case LogHeaderKey.Path:
                    return "Path";

                case LogHeaderKey.WorkingDirectory:
                    return "Working Directory";

                case LogHeaderKey.Timestamp:
                    return "Timestamp";

                default:
                    throw new ArgumentOutOfRangeException(nameof(key));

            }

        }

    }

}