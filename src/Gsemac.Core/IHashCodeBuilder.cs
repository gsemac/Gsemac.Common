namespace Gsemac.Core {

    public interface IHashCodeBuilder {

        IHashCodeBuilder WithValue<T>(T obj);

        int Build();

    }

}