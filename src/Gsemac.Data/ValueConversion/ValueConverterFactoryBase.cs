using Gsemac.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Data.ValueConversion {

    public abstract class ValueConverterFactoryBase :
        IValueConverterFactory {

        // Public members

        public IValueConverter Create(Type sourceType, Type destinationType) {

            var key = CreateKey(sourceType, destinationType);

            if (converters.TryGetValue(key, out var valueConverters) && valueConverters.Any()) {

                // Reverse the order of value converters, so ones added later are tried before ones added earlier.
                // They are combined into a CompositeValueConverter, which will try all converters for the given types until one works.

                IValueConverter[] convertersArray = valueConverters.Cast<IValueConverter>()
                    .Reverse()
                    .ToArray();

                return new CompositeValueConverter(convertersArray);

            }

            return new TypeCastConverter(sourceType, destinationType);

        }

        // Protected members

        protected ValueConverterFactoryBase() :
            this(useDefaultConverters: true) {
        }
        protected ValueConverterFactoryBase(bool useDefaultConverters) {

            if (useDefaultConverters) {

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

        private readonly IDictionary<Tuple<Type, Type>, List<IValueConverter>> converters = new Dictionary<Tuple<Type, Type>, List<IValueConverter>>();

        private static Tuple<Type, Type> CreateKey(Type sourceType, Type destinationType) {

            return Tuple.Create(sourceType, destinationType);

        }

    }

}