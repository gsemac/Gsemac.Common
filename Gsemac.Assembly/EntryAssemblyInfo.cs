namespace Gsemac.Assembly {

    public class EntryAssemblyInfo :
        FileSystemAssemblyInfo {

        // Public members

        public EntryAssemblyInfo() :
             base(System.Reflection.Assembly.GetEntryAssembly()) {
        }

    }

}