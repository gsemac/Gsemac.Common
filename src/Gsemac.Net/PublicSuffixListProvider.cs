namespace Gsemac.Net {

    public static class PublicSuffixListProvider {

        // Public members

        public static IPublicSuffixListProvider Default { get; set; } = new ResourcePublicSuffixListProvider();

    }

}