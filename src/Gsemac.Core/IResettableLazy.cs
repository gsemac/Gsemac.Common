namespace Gsemac.Core {

    public interface IResettableLazy<T> :
        ILazy<T> {

        void Reset();

    }

}