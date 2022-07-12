using Gsemac.Reflection;
using System;

namespace Gsemac.Data.ValueConversion {

    public class TypeCastConverter :
        ValueConverterBase {

        // Public members

        public TypeCastConverter(Type destinationType) :
            base(typeof(object), destinationType) {
        }
        public TypeCastConverter(Type sourceType, Type destinationType) :
            base(sourceType, destinationType) {
        }

        public override bool TryConvert(object value, out object result) {

            result = default;

            if (value is object && !value.GetType().Equals(SourceType))
                return false;

            return TypeUtilities.TryCast(value, DestinationType, out result);

        }

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