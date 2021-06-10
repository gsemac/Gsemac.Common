using System;

namespace Gsemac.Reflection.Plugins {

    public interface IRequirementAttribute {

        bool TestRequirement(IServiceProvider serviceProvider);

    }

}