using System;
using System.Collections;
using System.Net;
using System.Reflection;

namespace Gsemac.Polyfills.System.Net {

    public static class CookieContainerExtensions {

        // Public members

        /// <summary>
        /// Gets a <see cref="CookieCollection"/> that contains all of the <see cref="Cookie"/> instances in the container.
        /// </summary>
        /// <param name="cookieContainer"></param>
        /// <returns>A <see cref="CookieCollection"/> that contains all of the <see cref="Cookie"/> instances in the container.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static CookieCollection GetAllCookies(this CookieContainer cookieContainer) {

            // Adapted from https://stackoverflow.com/a/50548676/5383169 (JJS)

            if (cookieContainer is null)
                throw new ArgumentNullException(nameof(cookieContainer));

            CookieCollection cookies = new CookieCollection();

            // We're not guaranteed that the implementation uses the following private members.
            // In the event we can't find a particular member of the class, we'll just return an empty CookieCollection.

            FieldInfo domainTableFieldInfo = typeof(CookieContainer)
                .GetField("m_domainTable", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);

            if (domainTableFieldInfo is null || !(domainTableFieldInfo.GetValue(cookieContainer) is Hashtable domainTable))
                return cookies;

            foreach (string key in domainTable.Keys) {

                var item = domainTable[key]; // System.Net.PathList

                PropertyInfo valuesPropertyInfo = item.GetType().GetProperty("Values");

                if (valuesPropertyInfo is object && valuesPropertyInfo.GetGetMethod().Invoke(item, null) is ICollection items) {

                    foreach (CookieCollection cookieCollection in items) {

                        foreach (Cookie cookie in cookieCollection)
                            cookies.Add(cookie);

                    }

                }

            }

            return cookies;

        } // .NET 6 and later

    }

}