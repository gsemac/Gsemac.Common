using System;
using System.Linq;

namespace Gsemac.Data.ValueConversion {

    internal class TransitiveValueConverter :
        ValueConverterBase {

        // Public members

        public TransitiveValueConverter(IValueConverter[] converters) :
            base(converters?.FirstOrDefault()?.SourceType, converters?.LastOrDefault()?.DestinationType) {

            if (converters is null)
                throw new ArgumentNullException(nameof(converters));

            this.converters = converters;

        }

        public override bool TryConvert(object value, out object result) {

            result = value;

            bool success = true;

            foreach (IValueConverter converter in converters) {

                success &= converter.TryConvert(result, out result);

                if (!success)
                    break;

            }

            return success;

        }

        // Private members

        private readonly IValueConverter[] converters;

    }

}