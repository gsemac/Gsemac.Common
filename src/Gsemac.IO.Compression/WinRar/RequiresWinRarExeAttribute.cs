using Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection;
using Gsemac.Reflection.Plugins;
using System;
using System.IO;

namespace Gsemac.IO.Compression.WinRar {

    public sealed class RequiresWinRarExeAttribute :
        RequirementAttributeBase {

        // Public members

        public override bool TestRequirement(IServiceProvider serviceProvider) {

            IWinRarExeArchiveFactoryOptions options = serviceProvider?.GetService<IWinRarExeArchiveFactoryOptions>();

            return File.Exists(WinRarUtilities.GetWinRarExecutablePath(options?.WinRarDirectoryPath));

        }

    }

}