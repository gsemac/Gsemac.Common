﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gsemac.Text.Properties {
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
    internal class ExceptionMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ExceptionMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Gsemac.Text.Properties.ExceptionMessages", typeof(ExceptionMessages).Assembly);
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
        ///   Looks up a localized string similar to Length must be equal to or greater than 0..
        /// </summary>
        internal static string LengthMustBeGreaterThanOrEqualToZero {
            get {
                return ResourceManager.GetString("LengthMustBeGreaterThanOrEqualToZero", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to String cannot be of zero length..
        /// </summary>
        internal static string StringCannotBeOfZeroLength {
            get {
                return ResourceManager.GetString("StringCannotBeOfZeroLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The lexer encountered an unexpected character..
        /// </summary>
        internal static string UnexpectedLexerChar {
            get {
                return ResourceManager.GetString("UnexpectedLexerChar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The lexer encountered an unexpected character: &apos;{0}&apos;.
        /// </summary>
        internal static string UnexpectedLexerCharWithValue {
            get {
                return ResourceManager.GetString("UnexpectedLexerCharWithValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The lexer encountered an unexpected token..
        /// </summary>
        internal static string UnexpectedLexerToken {
            get {
                return ResourceManager.GetString("UnexpectedLexerToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The lexer encountered an unexpected token: &apos;{0}&apos;.
        /// </summary>
        internal static string UnexpectedLexerTokenWithValue {
            get {
                return ResourceManager.GetString("UnexpectedLexerTokenWithValue", resourceCulture);
            }
        }
    }
}
