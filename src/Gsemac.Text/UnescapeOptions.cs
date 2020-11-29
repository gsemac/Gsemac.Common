namespace Gsemac.Text {

    public enum UnescapeOptions {
        RepairTextEncoding = 1,
        UnescapeEscapeSequences = 2,
        UnescapeUriEncoding = 4,
        UnescapeHtmlEntities = 8,
        Default = RepairTextEncoding | UnescapeEscapeSequences | UnescapeUriEncoding | UnescapeHtmlEntities
    }

}