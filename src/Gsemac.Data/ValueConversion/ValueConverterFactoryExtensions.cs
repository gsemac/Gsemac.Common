using System;

namespace Gsemac.Data.ValueConversion {

    public static class ValueConverterFactoryExtensions {

        // Public members

        public static IValueConverter<TSource, TDestination> Create<TSource, TDestination>(this IValueConverterFactory valueConverterFactory) {

            if (valueConverterFactory is null)
                throw new ArgumentNullException(nameof(valueConverterFactory));

            IValueConverter baseConverter = valueConverterFactory.Create(typeof(TSource), typeof(TDestination));

            if (baseConverter is null)
                return null;

            return new TypedValueConverterAdapter<TSource, TDestination>(baseConverter);

        }

    }

}