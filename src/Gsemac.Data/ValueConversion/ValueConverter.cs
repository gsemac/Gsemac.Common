using System;

namespace Gsemac.Data.ValueConversion {

    public static class ValueConverter {

        // Public members

        public static IValueConverter<TSource, TDestination> Create<TSource, TDestination>(Func<TSource, TDestination> factory) {

            return new FactoryValueConverter<TSource, TDestination>(factory);

        }

        // Private members

        private class FactoryValueConverter<TSource, TDestination> :
            ValueConverterBase<TSource, TDestination> {

            // Public members

            public FactoryValueConverter(Func<TSource, TDestination> factory) {

                if (factory is null)
                    throw new ArgumentNullException(nameof(factory));

                this.factory = factory;

            }

            public override bool TryConvert(TSource value, out TDestination result) {

                result = factory(value);

                return true;

            }

            // Private members

            private readonly Func<TSource, TDestination> factory;

        }

    }

}