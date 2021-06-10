namespace Gsemac.Reflection.Plugins {

    public class PluginLoaderOptions :
        IPluginLoaderOptions {

        public IFileSystemAssemblyResolver AssemblyResolver { get; set; } = FileSystemAssemblyResolver.Default;
        public string PluginSearchPattern { get; set; }

        public static PluginLoaderOptions Default => new PluginLoaderOptions();

    }

}