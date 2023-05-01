using Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection;
using Gsemac.Reflection.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.IO.Compression.SevenZip {

    public sealed class RequiresSevenZipExeAttribute :
        RequirementAttributeBase {

        // Public members

        public override bool TestRequirement(IServiceProvider serviceProvider) {

            List<string> candidatePaths = new List<string>();

            ISevenZipExeArchiveFactoryOptions archiveFactoryOptions = serviceProvider?.GetService<ISevenZipExeArchiveFactoryOptions>();

            if (archiveFactoryOptions is object && !string.IsNullOrWhiteSpace(archiveFactoryOptions.SevenZipDirectoryPath))
                candidatePaths.Add(Path.Combine(archiveFactoryOptions.SevenZipDirectoryPath, SevenZipUtilities.SevenZipExecutableFileName));

            candidatePaths.Add(SevenZipUtilities.SevenZipExecutablePath);

            return candidatePaths.Any(path => File.Exists(path));

        }

    }

}