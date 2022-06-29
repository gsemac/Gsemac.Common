using Gsemac.Data.Properties;
using System;

namespace Gsemac.Data.ValueConversion {

    public abstract class ValueConverterBase :
        IValueConverter {

        // Public members

        public Type SourceType { get; }
        public Type DestinationType { get; }

        public virtual object Convert(object value) {

            if (TryConvert(value, out object result))
                return result;

            throw new ArgumentException(string.Format(ExceptionMessages.CannotConvertObjectsOfType, value.GetType(), value), nameof(value));

        }
        public abstract bool TryConvert(object value, out object result);

        // Protected members

        protected ValueConverterBase() :
            this(typeof(object), typeof(object)) {
        }
        protected ValueConverterBase(Type sourceType, Type destinationType) {

            if (sourceType is null)
                throw new ArgumentNullException(nameof(sourceType));

            if (destinationType is null)
                throw new ArgumentNullException(nameof(destinationType));

            SourceType = sourceType;
            DestinationType = destinationType;

        }

    }

    public abstract class ValueConverterBase<TSource, TDestination> :
        ValueConverterBase,
        IValueConverter<TSource, TDestination> {

        // Public members

        public TDestination Convert(TSource value) {

            if (TryConvert(value, out TDestination result))
                return result;

            throw new ArgumentException(string.Format(ExceptionMessages.CannotConvertObjectToType, value.GetType(), typeof(TDestination), value), nameof(value));

        }

        public abstract bool TryConvert(TSource value, out TDestination result);
        public override bool TryConvert(object value, out object result) {

            result = default;

            if (value is TSource sourceValue) {

                bool success = TryConvert(sourceValue, out TDestination destinationResult);

                result = destinationResult;

                return success;

            }

            return false;

        }

        // Protected members

        protected ValueConverterBase() :
            base(typeof(TSource), typeof(TDestination)) {
        }

    }

}