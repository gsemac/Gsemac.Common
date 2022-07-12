using Gsemac.Reflection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Data.ValueConversion {

    public abstract class ValueConverterFactoryBase :
        IValueConverterFactory {

        // Public members

        public IValueConverter Create(Type sourceType, Type destinationType) {

            if (sourceType is null)
                throw new ArgumentNullException(nameof(sourceType));

            if (destinationType is null)
                throw new ArgumentNullException(nameof(destinationType));

            // If the types are the same, we don't need to do any conversion at all.

            var key = CreateKey(sourceType, destinationType);

            if (converters.TryGetValue(key, out var valueConverters) && valueConverters.Any()) {

                // Reverse the order of value converters, so ones added later are tried before ones added earlier.
                // They are combined into a CompositeValueConverter, which will try all converters for the given types until one works.

                IValueConverter[] convertersArray = valueConverters.Cast<IValueConverter>()
                    .Reverse()
                    .ToArray();

                return new CompositeValueConverter(convertersArray);

            }
            else if (options.EnableDefaultConverters && IsTriviallyCastableType(sourceType) && IsTriviallyCastableType(destinationType)) {

                // Avoid costly transitive conversions along the path number -> string -> number if both types are numeric types that can easily be casted.
                // Also, don't be strict about the input type for numeric conversions, because the conversion can then fail when passing incompatible literals.

                return new TypeCastConverter(sourceType, destinationType, enforceSourceType: false);

            }
            else if (sourceType.Equals(destinationType)) {

                // The two types are the same, so we don't need to do any conversion at all.
                // This is done after checking for numeric types so we can use a less-strict converter for those types.

                return new IdentityConverter(sourceType);

            }
            else if (options.EnableTransitiveConversion) {

                // We didn't find any converters matching our source/destination types.
                // However, maybe we can find a transitive path from one to the other.

                IValueConverter[] transitiveConversionPath = GetTransitiveConversionPath(converters.SelectMany(p => p.Value), sourceType, destinationType)
                    .ToArray();

                return new TransitiveConverter(transitiveConversionPath);

            }

            return new TypeCastConverter(sourceType, destinationType, enforceSourceType: true);

        }

        // Protected members

        protected ValueConverterFactoryBase() :
            this(ValueConverterFactoryOptions.Default) {
        }
        protected ValueConverterFactoryBase(IValueConverterFactoryOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            if (options.EnableDefaultConverters) {

                AddValueConverter(new BoolToStringConverter());
                AddValueConverter(new StringToBoolConverter());

                AddValueConverter(new NumberToStringConverter<byte>());
                AddValueConverter(new NumberToStringConverter<sbyte>());
                AddValueConverter(new NumberToStringConverter<decimal>());
                AddValueConverter(new NumberToStringConverter<double>());
                AddValueConverter(new NumberToStringConverter<float>());
                AddValueConverter(new NumberToStringConverter<int>());
                AddValueConverter(new NumberToStringConverter<uint>());
                AddValueConverter(new NumberToStringConverter<long>());
                AddValueConverter(new NumberToStringConverter<ulong>());
                AddValueConverter(new NumberToStringConverter<short>());
                AddValueConverter(new NumberToStringConverter<ushort>());

                AddValueConverter(new StringToNumberConverter<byte>());
                AddValueConverter(new StringToNumberConverter<sbyte>());
                AddValueConverter(new StringToNumberConverter<decimal>());
                AddValueConverter(new StringToNumberConverter<double>());
                AddValueConverter(new StringToNumberConverter<float>());
                AddValueConverter(new StringToNumberConverter<int>());
                AddValueConverter(new StringToNumberConverter<uint>());
                AddValueConverter(new StringToNumberConverter<long>());
                AddValueConverter(new StringToNumberConverter<ulong>());
                AddValueConverter(new StringToNumberConverter<short>());
                AddValueConverter(new StringToNumberConverter<ushort>());

            }

            this.options = options;

        }

        protected void AddValueConverter(IValueConverter valueConverter) {

            if (valueConverter is null)
                throw new ArgumentNullException(nameof(valueConverter));

            var key = CreateKey(valueConverter.SourceType, valueConverter.DestinationType);

            if (converters.TryGetValue(key, out var valueConverters))
                valueConverters.Add(valueConverter);
            else
                converters.Add(key, new List<IValueConverter> { valueConverter });

        }

        // Private members

        private readonly IValueConverterFactoryOptions options;
        private readonly IDictionary<Tuple<Type, Type>, List<IValueConverter>> converters = new Dictionary<Tuple<Type, Type>, List<IValueConverter>>();

        private IEnumerable<IValueConverter> GetTransitiveConversionPath(IEnumerable<IValueConverter> source, Type sourceType, Type destinationType) {

            // Get all converters that can convert from the source type.

            IEnumerable<IValueConverter> matchingConverters = source.Where(converter => converter.SourceType.Equals(sourceType));

            foreach (IValueConverter matchingConverter in matchingConverters) {

                // Find the next steps in the conversion chain.

                if (matchingConverter.DestinationType.Equals(destinationType))
                    return new[] { matchingConverter };

                IEnumerable<IValueConverter> nextMatchingConverters = GetTransitiveConversionPath(source.Except(matchingConverters), matchingConverter.DestinationType, destinationType);

                if (nextMatchingConverters.Any())
                    return new[] { matchingConverter }.Concat(nextMatchingConverters);

            }

            // We did not find any matching converters.

            return Enumerable.Empty<IValueConverter>();

        }

        private static Tuple<Type, Type> CreateKey(Type sourceType, Type destinationType) {

            return Tuple.Create(sourceType, destinationType);

        }
        private static bool IsTriviallyCastableType(Type type) {

            return type.IsNumericType() || type.Equals(typeof(bool));

        }

    }

}