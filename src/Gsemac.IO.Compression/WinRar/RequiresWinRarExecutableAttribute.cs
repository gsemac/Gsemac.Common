using Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection;
using Gsemac.Reflection.Plugins;
using System;
using System.IO;

namespace Gsemac.IO.Compression.WinRar {

    public sealed class RequiresWinRarExecutableAttribute :
        RequirementAttributeBase {

        // Public members

        public override bool TestRequirement(IServiceProvider serviceProvider) {

            IWinRarProcessArchiveFactoryOptions options = serviceProvider?.GetService<IWinRarProcessArchiveFactoryOptions>();

            return File.Exists(WinRarUtilities.GetWinRarExecutablePath(options?.WinRarDirectoryPath));

        }

    }

}