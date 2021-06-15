using Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection;
using Gsemac.Reflection.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.IO.Compression.SevenZip {

    public sealed class RequiresSevenZipAttribute :
        RequirementAttributeBase {

        // Public members

        public override bool TestRequirement(IServiceProvider serviceProvider) {

            List<string> candidatePaths = new List<string>();

            IArchiveFactoryOptions archiveFactoryOptions = serviceProvider?.GetService<IArchiveFactoryOptions>();

            if (archiveFactoryOptions is object && !string.IsNullOrWhiteSpace(archiveFactoryOptions.WinrarDirectoryPath))
                candidatePaths.Add(Path.Combine(archiveFactoryOptions.WinrarDirectoryPath, SevenZipUtilities.SevenZipExecutableFilename));

            candidatePaths.Add(SevenZipUtilities.SevenZipExecutablePath);

            return candidatePaths.Any(path => File.Exists(path));

        }

    }

}