using System.Linq;

namespace Gsemac.Polyfills.System {

    public static class Array {

        public static T[] Empty<T>() {

            return Enumerable.Empty<T>().ToArray();

        }

    }

}