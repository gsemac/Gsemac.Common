namespace Gsemac.Reflection {

    public class CopyPropertiesOptions :
        ICopyPropertiesOptions {

        public bool CopyNonPublicProperties { get; set; } = false;
        public bool IgnoreExceptions { get; set; } = false;

        public static CopyPropertiesOptions Default => new CopyPropertiesOptions();

    }

}