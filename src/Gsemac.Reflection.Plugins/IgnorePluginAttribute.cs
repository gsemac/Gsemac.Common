using System;

namespace Gsemac.Reflection.Plugins {

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class IgnorePluginAttribute :
        Attribute {
    }

}