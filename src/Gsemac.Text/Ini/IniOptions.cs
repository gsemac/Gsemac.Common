namespace Gsemac.Text.Ini {

    public class IniOptions :
            IIniOptions {

        public bool Unescape { get; set; } = true;
        public bool AllowComments { get; set; } = true;

    }

}