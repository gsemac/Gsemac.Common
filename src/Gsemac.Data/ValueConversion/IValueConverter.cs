using System;

namespace Gsemac.Data.ValueConversion {

    public interface IValueConverter {

        Type SourceType { get; }
        Type DestinationType { get; }

        object Convert(object value);
        bool TryConvert(object value, out object result);

    }

    public interface IValueConverter<TSource, TDestination> :
        IValueConverter {

        TDestination Convert(TSource value);
        bool TryConvert(TSource value, out TDestination result);

    }

}