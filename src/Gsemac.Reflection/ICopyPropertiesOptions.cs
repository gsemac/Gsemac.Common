namespace Gsemac.Reflection {

    public interface ICopyPropertiesOptions {

        bool CopyNonPublicProperties { get; }
        bool IgnoreExceptions { get; }

    }

}