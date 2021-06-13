namespace Gsemac.Reflection {

    public interface ICopyPropertiesOptions {

        bool CopyNonPublicProperties { get; }
        bool CopyNullProperties { get; }
        bool IgnoreExceptions { get; }

    }

}