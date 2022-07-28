using System;
using System.Linq;

namespace Gsemac.Data.ValueConversion {

    internal class CompositeValueConverter :
        ValueConverterBase {

        // Public members

        public CompositeValueConverter(IValueConverter[] converters) :
            base(GetSourceType(converters), GetDestinationType(converters)) {

            if (converters is null)
                throw new ArgumentNullException(nameof(converters));

            this.converters = converters;

        }

        public override bool TryConvert(object value, out object result) {

            result = default;

            foreach (IValueConverter converter in converters) {

                if (converter.TryConvert(value, out result))
                    return true;

            }

            return false;

        }

        // Private members

        private readonly IValueConverter[] converters;

        private static Type GetSourceType(IValueConverter[] converters) {

            if (converters is object && converters.Length > 0 && converters.All(converter => converter.SourceType.Equals(converters.First().SourceType)))
                return converters.First().SourceType;

            return typeof(object);

        }
        private static Type GetDestinationType(IValueConverter[] converters) {

            if (converters is object && converters.Length > 0 && converters.All(converter => converter.DestinationType.Equals(converters.Last().DestinationType)))
                return converters.Last().DestinationType;

            return typeof(object);

        }

    }

}