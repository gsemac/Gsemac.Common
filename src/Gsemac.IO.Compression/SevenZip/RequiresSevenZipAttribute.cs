using Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection;
using Gsemac.Reflection.Plugins;
using System;
using System.IO;

namespace Gsemac.IO.Compression.SevenZip {

    public sealed class RequiresSevenZipAttribute :
        RequirementAttributeBase {

        // Public members

        public override bool TestRequirement(IServiceProvider serviceProvider) {

            IArchiveFactoryOptions archiveFactoryOptions = serviceProvider?.GetService<IArchiveFactoryOptions>();

            if (archiveFactoryOptions is null)
                return false;

            return File.Exists(archiveFactoryOptions.SevenZipExecutablePath);

        }

    }

}