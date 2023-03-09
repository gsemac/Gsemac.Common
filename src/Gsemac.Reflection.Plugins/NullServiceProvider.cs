using System;

namespace Gsemac.Reflection.Plugins {

    internal class NullServiceProvider :
        IServiceProvider {

        // Public members

        public object GetService(Type serviceType) {

            return null;

        }

    }

}