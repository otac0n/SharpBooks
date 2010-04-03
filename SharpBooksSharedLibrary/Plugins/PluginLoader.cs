﻿//-----------------------------------------------------------------------
// <copyright file="PluginLoader.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Plugins
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Security;

    /// <summary>
    /// Provides methods to load plugins from external assemblies.
    /// </summary>
    public class PluginLoader
    {
        /// <summary>
        /// Loads the plugins from an assembly specified by filename.
        /// </summary>
        /// <param name="fileName">The filename of the assembly to load.</param>
        /// <returns>The plugin factories contained in the assembly, if the load was successful; null, otherwise.</returns>
        public static IEnumerable<IPluginFactory> LoadPlugins(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new LoadPluginsFailureException("Unable to load plugins:  The file '" + fileName + "' could not be found.");
            }

            try
            {
                return LoadPlugins(File.ReadAllBytes(fileName));
            }
            catch (IOException ex)
            {
                throw new LoadPluginsFailureException("Loading of plugins failed.  Check the inner exception for more details.", ex);
            }
        }

        /// <summary>
        /// Loads the plugins from a specified assembly.
        /// </summary>
        /// <param name="rawAssembly">The raw assembly from which to load.</param>
        /// <returns>The plugin factories contained in the assembly, if the load was successful; null, otherwise.</returns>
        public static IEnumerable<IPluginFactory> LoadPlugins(byte[] rawAssembly)
        {
            try
            {
                var assemblyName = Assembly.ReflectionOnlyLoad(rawAssembly).GetName();
                var key = assemblyName.GetPublicKey();

                if (key.Length == 0)
                {
                    throw new LoadPluginsFailureException("Unable to load plugins: The assembly '" + assemblyName.FullName + "' was not signed.");
                }

                var assembly = Assembly.Load(rawAssembly);

                return LoadPlugins(assembly);
            }
            catch (BadImageFormatException ex)
            {
                throw new LoadPluginsFailureException("Loading of plugins failed.  Check the inner exception for more details.", ex);
            }
        }

        /// <summary>
        /// Loads the plugins from a specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly from which to load.</param>
        /// <returns>The plugin factories contained in the assembly, if the load was successful; null, otherwise.</returns>
        public static IEnumerable<IPluginFactory> LoadPlugins(Assembly assembly)
        {
            var factories = new List<IPluginFactory>();

            try
            {
                foreach (var type in assembly.GetTypes())
                {
                    IPluginEnumerator instance = null;

                    if (type.GetInterface("IPluginEnumerator") != null)
                    {
                        instance = (IPluginEnumerator)Activator.CreateInstance(type);
                    }

                    if (instance != null)
                    {
                        factories.AddRange(instance.EnumerateFactories());
                    }
                }
            }
            catch (SecurityException ex)
            {
                throw new LoadPluginsFailureException("Loading of plugins failed.  Check the inner exception for more details.", ex);
            }
            catch (ReflectionTypeLoadException ex)
            {
                throw new LoadPluginsFailureException("Loading of plugins failed.  Check the inner exception for more details.", ex);
            }

            return factories.AsReadOnly();
        }
    }
}
