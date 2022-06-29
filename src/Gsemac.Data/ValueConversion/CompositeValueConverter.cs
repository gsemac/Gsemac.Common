using System;
using System.Collections.Generic;

namespace Gsemac.Data.ValueConversion {

    public class CompositeValueConverter :
        ValueConverterBase {

        // Public members

        public CompositeValueConverter(IEnumerable<IValueConverter> converters) {

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

        private readonly IEnumerable<IValueConverter> converters;

    }

}