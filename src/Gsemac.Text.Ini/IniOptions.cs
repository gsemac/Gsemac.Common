namespace Gsemac.Text.Ini {

    public class IniOptions :
            IIniOptions {

        // Public members

        public bool AllowComments { get; } = true;
        public string CommentMarker { get; } = ";";
        public string PropertyValueSeparator { get; } = "=";
        public bool Unescape { get; } = true;

        public static IniOptions Default => new IniOptions();

    }

}