using Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection;
using Gsemac.Reflection.Plugins;
using System;
using System.IO;

namespace Gsemac.IO.Compression.Winrar {

    public sealed class RequiresWinrarExeAttribute :
        RequirementAttributeBase {

        // Public members

        public override bool TestRequirement(IServiceProvider serviceProvider) {

            IWinrarExeArchiveFactoryOptions options = serviceProvider?.GetService<IWinrarExeArchiveFactoryOptions>();

            return File.Exists(WinrarUtilities.GetWinrarExecutablePath(options?.WinrarDirectoryPath));

        }

    }

}