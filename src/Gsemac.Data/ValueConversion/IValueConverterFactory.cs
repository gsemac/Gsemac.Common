using System;

namespace Gsemac.Data.ValueConversion {

    public interface IValueConverterFactory {

        IValueConverter Create(Type sourceType, Type destinationType);

    }

}