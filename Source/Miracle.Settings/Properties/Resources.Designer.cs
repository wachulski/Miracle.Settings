﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Miracle.Settings.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Miracle.Settings.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to TypeConverters must implement {0}.
        /// </summary>
        internal static string BadExplicitTypeConverterTypeFormat {
            get {
                return ResourceManager.GetString("BadExplicitTypeConverterTypeFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to convert value [{0}] to type {1}.
        /// </summary>
        internal static string ConvertValueErrorFormat {
            get {
                return ResourceManager.GetString("ConvertValueErrorFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to convert values [{0}] into type {1}.
        /// </summary>
        internal static string ConvertValuesErrorFormat {
            get {
                return ResourceManager.GetString("ConvertValuesErrorFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to create settings instance of type {0} while loading settings prefixed by: [{1}].
        /// </summary>
        internal static string CreateErrorFormat {
            get {
                return ResourceManager.GetString("CreateErrorFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to create instance of type converter: {0}.
        /// </summary>
        internal static string CreateTypeConverterErrorFormat {
            get {
                return ResourceManager.GetString("CreateTypeConverterErrorFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to convert values: [{0}] into type {1} using specified type converter.
        /// </summary>
        internal static string ExplicitTypeConverterErrorFormat {
            get {
                return ResourceManager.GetString("ExplicitTypeConverterErrorFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A value has to be provided for referenced Setting: [{0}].
        /// </summary>
        internal static string MissingReferenceValueFormat {
            get {
                return ResourceManager.GetString("MissingReferenceValueFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A value of type {0} has to be provided for Setting: [{1}].
        /// </summary>
        internal static string MissingValueFormat {
            get {
                return ResourceManager.GetString("MissingValueFormat", resourceCulture);
            }
        }
    }
}
