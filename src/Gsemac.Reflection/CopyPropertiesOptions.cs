namespace Gsemac.Reflection {

    public class CopyPropertiesOptions :
        ICopyPropertiesOptions {

        public bool CopyCollectionItems { get; set; } = false;
        public bool CopyNonPublicProperties { get; set; } = false;
        public bool CopyNullValues { get; set; } = true;
        public bool IgnoreExceptions { get; set; } = false;

        public static CopyPropertiesOptions Default => new CopyPropertiesOptions();

    }

}