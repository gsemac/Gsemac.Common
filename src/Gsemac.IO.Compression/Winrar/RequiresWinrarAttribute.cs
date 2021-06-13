using Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection;
using Gsemac.Reflection.Plugins;
using System;
using System.IO;

namespace Gsemac.IO.Compression.Winrar {

    public sealed class RequiresWinrarAttribute :
        RequirementAttributeBase {

        // Public members

        public override bool TestRequirement(IServiceProvider serviceProvider) {

            IArchiveFactoryOptions archiveFactoryOptions = serviceProvider?.GetService<IArchiveFactoryOptions>();

            if (archiveFactoryOptions is null)
                return false;

            return File.Exists(Path.Combine(archiveFactoryOptions.WinrarDirectoryPath, WinrarUtilities.WinrarExecutableFilename)) ||
                File.Exists(WinrarUtilities.WinrarExecutablePath);

        }

    }

}