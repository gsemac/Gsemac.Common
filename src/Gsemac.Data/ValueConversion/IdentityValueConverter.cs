using System;

namespace Gsemac.Data.ValueConversion {

    internal class IdentityValueConverter :
        ValueConverterBase {

        // Public members

        public IdentityValueConverter(Type type) :
            base(type, type) {
        }

        public override bool TryConvert(object value, out object result) {

            result = default;

            if (!value.GetType().Equals(SourceType))
                return false;

            result = value;

            return true;

        }

    }

}