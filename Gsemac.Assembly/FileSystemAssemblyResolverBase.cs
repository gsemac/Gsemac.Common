﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Gsemac.Assembly {

    public abstract class FileSystemAssemblyResolverBase :
        IAssemblyResolver {

        // Public members

        public bool AddExtension { get; set; } = true;
        public ICollection<string> ProbingPaths { get; } = new List<string>();
        public bool ProbeCurrentDirectory { get; set; } = true;
        public bool Unsafe { get; set; } = false;

        public System.Reflection.Assembly ResolveAssembly(string assemblyName) {

            string assemblyPath = GetAssemblyPath(assemblyName);

            if (!string.IsNullOrEmpty(assemblyPath)) {

                if (Unsafe)
                    return System.Reflection.Assembly.UnsafeLoadFrom(assemblyPath);
                else
                    return System.Reflection.Assembly.LoadFrom(assemblyPath);

            }

            return null;

        }
        public System.Reflection.Assembly ResolveAssembly(object sender, ResolveEventArgs e) {

            return ResolveAssembly(new System.Reflection.AssemblyName(e.Name).Name);

        }

        public bool AssemblyExists(string assemblyName) {

            return !string.IsNullOrEmpty(GetAssemblyPath(assemblyName));

        }
        public abstract string GetAssemblyPath(string assemblyName);

        // Protected members

        protected virtual IEnumerable<string> GetProbingPaths() {

            return ProbingPaths;

        }

    }

}