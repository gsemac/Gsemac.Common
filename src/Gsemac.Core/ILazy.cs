namespace Gsemac.Core {

    public interface ILazy<T> {

        bool IsValueCreated { get; }
        T Value { get; }

    }

}