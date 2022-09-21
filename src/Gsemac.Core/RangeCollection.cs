using Gsemac.Core.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Core {

    public class RangeCollection<T> :
        IRangeCollection<T> where T :
        struct, IComparable, IFormattable {

        // Public members

        public int Count => items.Count;
        public bool IsReadOnly => false;

        public void Add(IRange<T> item) {

            if (item is null)
                throw new ArgumentNullException(nameof(item));

            items.Add(item);

        }

        public bool Contains(T value) {

            return items.Any(r => r.Contains(value));

        }
        public bool Contains(IRange<T> range) {

            // Unlike IRange<T>'s Contains method, this method checks for a specific, equal range.


            return items.Contains(range);

        }
        public bool IntersectsWith(IRange<T> range) {

            return items.Any(r => r.IntersectsWith(range));

        }

        public bool Remove(IRange<T> item) {

            return items.Remove(item);

        }
        public void Clear() {

            items.Clear();

        }

        public void Sort() {

            Sort(defaultComparer);

        }
        public void Sort(IComparer<IRange<T>> comparer) {

            items.Sort(comparer);

        }

        public void Condense() {

            List<IRange<T>> newItems = new List<IRange<T>>();

            IEnumerator<IRange<T>> enumerator = items.OrderBy(r => r, defaultComparer)
                .GetEnumerator();

            if (!enumerator.MoveNext())
                return;

            IRange<T> currentRange = enumerator.Current;

            while (true) {

                // If there are no other ranges, add the current range to the list.

                if (!enumerator.MoveNext()) {

                    newItems.Add(currentRange);

                    break;

                }

                // See if we can combine the current range with the following range.

                IRange<T> nextRange = enumerator.Current;

                if (currentRange.IntersectsWith(nextRange)) {

                    currentRange = Range.Combine(currentRange, nextRange);

                }
                else {

                    newItems.Add(currentRange);

                    currentRange = nextRange;

                }

            }

            items.Clear();
            items.AddRange(newItems);

        }

        public void CopyTo(IRange<T>[] array, int arrayIndex) {

            items.CopyTo(array, arrayIndex);

        }

        public override string ToString() {

            return this.ToString(RangeFormatter.Default);

        }

        public IEnumerator<IRange<T>> GetEnumerator() {

            return items.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return items.GetEnumerator();

        }

        public static RangeCollection<T> Parse(string value) {

            return Parse(value, RangeParsingOptions.Default);

        }
        public static RangeCollection<T> Parse(string value, IRangeParsingOptions options) {

            if (!TryParse(value, options, out RangeCollection<T> result))
                throw new FormatException(string.Format(ExceptionMessages.InvalidRangeCollectionString, value));

            return result;

        }
        public static bool TryParse(string value, out RangeCollection<T> result) {

            return TryParse(value, RangeParsingOptions.Default, out result);

        }
        public static bool TryParse(string value, IRangeParsingOptions options, out RangeCollection<T> result) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            result = null;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            value = value.Trim();

            // Split the ranges using commas as a delimiter, but ignoring commas inside of bounded ranges.

            RangeCollection<T> collection = new RangeCollection<T>();

            foreach (string rangeStr in SplitDelimitedRangesString(value)) {

                if (string.IsNullOrWhiteSpace(rangeStr))
                    continue;

                if (!Range<T>.TryParse(rangeStr, options, out var parsedRange)) {

                    if (!options.IgnoreInvalidRanges)
                        return false;

                }
                else {

                    collection.Add(parsedRange);

                }

            }

            result = collection;

            return true;

        }

        // Private members

        private readonly IComparer<IRange<T>> defaultComparer = RangeComparer<T>.Default;
        private readonly List<IRange<T>> items = new List<IRange<T>>();

        private static IEnumerable<string> SplitDelimitedRangesString(string value) {

            List<string> rangeStrs = new List<string>();
            StringBuilder rangeStrBuilder = new StringBuilder();
            bool insideBounds = false;

            foreach (char c in value) {

                if (c == '[' || c == ']' || c == '(' || c == ')')
                    insideBounds = !insideBounds;

                if (c == ',' && !insideBounds) {

                    yield return rangeStrBuilder.ToString();

                    rangeStrBuilder.Clear();

                }
                else {

                    rangeStrBuilder.Append(c);

                }

            }

            if (rangeStrBuilder.Length > 0)
                yield return rangeStrBuilder.ToString();

        }

    }

}