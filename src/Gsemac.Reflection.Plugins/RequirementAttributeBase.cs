using System;

namespace Gsemac.Reflection.Plugins {

    public abstract class RequirementAttributeBase :
        Attribute,
        IRequirementAttribute {

        public abstract bool TestRequirement(IServiceProvider serviceProvider);

    }

}