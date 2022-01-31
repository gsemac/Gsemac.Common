namespace Gsemac.Reflection {

    public interface ICopyPropertiesOptions {

        bool CopyCollectionItems { get; }
        bool CopyNonPublicProperties { get; }
        bool CopyNullValues { get; }
        bool IgnoreExceptions { get; }

    }

}