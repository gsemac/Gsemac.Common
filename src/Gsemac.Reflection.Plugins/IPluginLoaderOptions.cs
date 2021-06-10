namespace Gsemac.Reflection.Plugins {

    public interface IPluginLoaderOptions {

        IFileSystemAssemblyResolver AssemblyResolver { get; }
        string PluginSearchPattern { get; }

    }

}