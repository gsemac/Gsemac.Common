﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gsemac.Net.GitHub.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class QueryStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal QueryStrings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Gsemac.Net.GitHub.Properties.QueryStrings", typeof(QueryStrings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .//ul//li.
        /// </summary>
        internal static string ReleaseAssetsXPath {
            get {
                return ResourceManager.GetString("ReleaseAssetsXPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .//a.
        /// </summary>
        internal static string ReleaseAssetTitleXPath {
            get {
                return ResourceManager.GetString("ReleaseAssetTitleXPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .//*[@datetime].
        /// </summary>
        internal static string ReleaseDateTimeXPath {
            get {
                return ResourceManager.GetString("ReleaseDateTimeXPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .//div[@*[contains(.,&apos;body-content&apos;)]].
        /// </summary>
        internal static string ReleaseDescriptionXPath {
            get {
                return ResourceManager.GetString("ReleaseDescriptionXPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to //section[h2[contains(@class,&quot;sr-only&quot;)]].
        /// </summary>
        internal static string ReleasesXPath {
            get {
                return ResourceManager.GetString("ReleasesXPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .//svg[contains(@class,&apos;octicon-tag&apos;)]/following-sibling::span.
        /// </summary>
        internal static string ReleaseTagXPath {
            get {
                return ResourceManager.GetString("ReleaseTagXPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .//a[contains(@class,&apos;Link--primary&apos;)].
        /// </summary>
        internal static string ReleaseTitleXPath {
            get {
                return ResourceManager.GetString("ReleaseTitleXPath", resourceCulture);
            }
        }
    }
}
