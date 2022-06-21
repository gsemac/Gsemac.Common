namespace Gsemac.Text.Ini {

    public interface IIniOptions {

        bool AllowComments { get; }
        string CommentMarker { get; }
        string PropertyValueSeparator { get; }
        bool Unescape { get; }

    }

}