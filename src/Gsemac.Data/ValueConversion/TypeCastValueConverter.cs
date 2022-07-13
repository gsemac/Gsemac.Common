using Gsemac.Reflection;
using System;

namespace Gsemac.Data.ValueConversion {

    internal class TypeCastValueConverter :
        ValueConverterBase {

        // Public members

        public TypeCastValueConverter(Type sourceType, Type destinationType, bool enforceSourceType) :
            base(sourceType, destinationType) {

            this.enforceSourceType = enforceSourceType;

        }

        public override bool TryConvert(object value, out object result) {

            result = default;

            // The source type is optionally checked before attempting the conversion.
            // This is so we can have converters that do things like converting any numeric type to another numeric type.

            if (enforceSourceType && value is object && !value.GetType().Equals(SourceType))
                return false;

            return TypeUtilities.TryCast(value, DestinationType, out result);

        }

        // Private members

        private readonly bool enforceSourceType;

    }

    public class TypeCastConverter<TDestination> :
       ValueConverterBase<object, TDestination> {

        // Public members

        public override bool TryConvert(object value, out TDestination result) {

            return TypeUtilities.TryCast(value, out result);

        }

    }

    public class TypeCastConverter<TSource, TDestination> :
        ValueConverterBase<TSource, TDestination> {

        // Public members

        public override bool TryConvert(TSource value, out TDestination result) {

            result = default;

            if (value is object && !value.GetType().Equals(SourceType))
                return false;

            return TypeUtilities.TryCast(value, out result);

        }

    }

}