namespace Gsemac.Text.Ini {

    public interface IIniOptions {

        bool AllowComments { get; }
        bool AllowEscapeSequences { get; }

        string CommentMarker { get; }
        string PropertyValueSeparator { get; }

    }

}