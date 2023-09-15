using Gsemac.Reflection.Plugins;

namespace Gsemac.Drawing.Imaging {

    public sealed class RequiresNQuantAttribute :
        RequirementAttributeBase {

        // Public members

        public RequiresNQuantAttribute() :
            base(new RequiresAssemblyOrTypesAttribute("nQuant.Core", "nQuant.WuQuantizer")) {
        }

    }

}