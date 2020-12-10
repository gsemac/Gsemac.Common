namespace Gsemac.Reflection {

    public static class AssemblyResolver {

        public static IAssemblyResolver Default { get; } = new AnyCpuFileSystemAssemblyResolver();

    }

}