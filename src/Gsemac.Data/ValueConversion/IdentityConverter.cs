using System;

namespace Gsemac.Data.ValueConversion {

    internal class IdentityConverter :
        ValueConverterBase {

        // Public members

        public IdentityConverter(Type type) :
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