﻿using Gsemac.Collections;
using Gsemac.Logging.Extensions;
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

        public ICollection<string> Keys => dict.Keys;
        public ICollection<string> Values => new LazyReadOnlyCollection<string>(dict.Values.Select(f => f()));

        public int Count => dict.Count;
        public bool IsReadOnly => false;

        public LogHeader() :
            this(true) {
        }
        public LogHeader(bool addDefaultHeaders) {

            if (addDefaultHeaders) {

                this.Add(LogHeaderKey.ProductVersion, () => Assembly.GetEntryAssembly().GetName().Version.ToString());
                this.Add(LogHeaderKey.ClrVersion, () => Environment.Version.ToString());
                this.Add(LogHeaderKey.OSVersion, () => Environment.OSVersion.ToString() + string.Format(" ({0})", Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit"));
                this.Add(LogHeaderKey.Locale, () => System.Globalization.CultureInfo.InstalledUICulture.Name);
                this.Add(LogHeaderKey.Path, () => PathUtilities.AnonymizePath(Assembly.GetEntryAssembly().Location));
                this.Add(LogHeaderKey.WorkingDirectory, () => PathUtilities.AnonymizePath(System.IO.Directory.GetCurrentDirectory()));
                this.Add(LogHeaderKey.Timestamp, () => DateTime.Now.ToString());

            }

        }

        public void Add(string key, Func<string> getter) {

            dict[key] = getter;

        }
        public void Add(string key, string value) {

            Add(key, () => value);

        }
        public void Add(KeyValuePair<string, string> item) {

            Add(item.Key, () => item.Value);

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

        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        public static LogHeader Empty => new LogHeader(false);

        // Private members

        private readonly IDictionary<string, Func<string>> dict = new OrderedDictionary<string, Func<string>>();

    }

}