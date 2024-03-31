using Gsemac.Reflection;
using System;

namespace Gsemac.Data.ValueConversion {

    internal class TypedValueConverterAdapter<TSource, TDestination> :
        ValueConverterBase<TSource, TDestination> {

        // Public members

        public TypedValueConverterAdapter(IValueConverter valueConverter) {

            if (valueConverter is null)
                throw new ArgumentNullException(nameof(valueConverter));

            this.valueConverter = valueConverter;

        }

        public override bool TryConvert(TSource value, out TDestination result) {

            result = default;

            return valueConverter.TryConvert(value, out object objectResult) &&
                TypeUtilities.TryConvert(objectResult, out result);

        }

        // Private members

        private readonly IValueConverter valueConverter;

    }

}