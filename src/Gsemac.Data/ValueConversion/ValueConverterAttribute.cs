using System;

namespace Gsemac.Data.ValueConversion {

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, AllowMultiple = true, Inherited = false)]
    public sealed class ValueConverterAttribute :
        Attribute {

        public Type ValueConverterType { get; }

        public ValueConverterAttribute(Type valueConverterType) {

            if (valueConverterType is null)
                throw new ArgumentNullException(nameof(valueConverterType));

            ValueConverterType = valueConverterType;

        }

    }

}