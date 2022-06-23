namespace Gsemac.Text.Ini {

    public class IniOptions :
            IIniOptions {

        // Public members

        public bool AllowComments { get; set; } = true;
        public string CommentMarker { get; set; } = ";";
        public string PropertyValueSeparator { get; set; } = "=";
        public bool Unescape { get; set; } = true;

        public static IniOptions Default => new IniOptions();

    }

}