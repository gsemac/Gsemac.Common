using System;

namespace Gsemac.Reflection.Plugins {

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class PluginPriorityAttribute :
        Attribute {

        // Public members

        public int Priority { get; }

        public PluginPriorityAttribute(int priority) {

            Priority = priority;

        }
        public PluginPriorityAttribute(Priority priority) :
            this((int)priority) {
        }

    }

}