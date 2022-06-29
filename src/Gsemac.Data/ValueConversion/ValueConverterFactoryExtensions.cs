using System;

namespace Gsemac.Data.ValueConversion {

    public static class ValueConverterFactoryExtensions {

        // Public members

        public static IValueConverter Create<TSource, TDestination>(this IValueConverterFactory valueConverterFactory) {

            if (valueConverterFactory is null)
                throw new ArgumentNullException(nameof(valueConverterFactory));

            return valueConverterFactory.Create(typeof(TSource), typeof(TDestination));

        }

    }

}