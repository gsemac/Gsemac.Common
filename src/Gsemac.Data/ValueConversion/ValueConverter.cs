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

                try {

                    result = factory(value);

                    return true;

                }
                catch (Exception) {

                    // We don't know if the user-provided delegate will throw or not, but we don't want TryConvert to throw.
                    // While we eat the exception here, it could be possible to make it accessible to the user by making Convert overridable.

                    result = default;

                    return false;

                }

            }

            // Private members

            private readonly Func<TSource, TDestination> factory;

        }

    }

}