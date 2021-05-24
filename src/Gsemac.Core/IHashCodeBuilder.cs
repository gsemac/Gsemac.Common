namespace Gsemac.Core {

    public interface IHashCodeBuilder {

        IHashCodeBuilder Add(object obj);

        int Build();

    }

}