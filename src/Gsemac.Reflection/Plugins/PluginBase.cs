namespace Gsemac.Reflection.Plugins {

    public abstract class PluginBase :
        IPlugin {

        // Public members

        int IPlugin.Priority => priority;

        // Protected members

        protected PluginBase() :
            this(0) {
        }
        protected PluginBase(int priority) {

            this.priority = priority;

        }

        // Private members

        private readonly int priority;

    }

}