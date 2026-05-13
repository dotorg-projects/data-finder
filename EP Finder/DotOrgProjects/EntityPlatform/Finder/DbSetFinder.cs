// Copyright (c) 2025 .ORG Projects. All rights reserved.
// Licensed under the Microsoft Public License (Ms-PL).
// See LICENSE.txt in the project root for license information.
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace DotOrgProjects.EntityPlatform.Finder
{
    /// <summary>
    /// Replaces EF Core's default <see cref="IDbSetFinder"/> to discover entity types automatically
    /// by scanning a configured assembly and root namespace for classes annotated with <see cref="TableAttribute"/>,
    /// instead of requiring explicit <c>DbSet&lt;T&gt;</c> property declarations on the <c>DbContext</c>.
    /// </summary>
    /// <remarks>
    /// <see cref="DbSetFinder"/> is registered into EF Core's internal service provider by
    /// <see cref="DbFinderContextOptionsExtension.ApplyServices"/> as a singleton replacement
    /// for <see cref="IDbSetFinder"/>. It is configured via <see cref="DbFinderContextOptionsExtensionMethods.UseFinderIn"/>.
    /// </remarks>
    internal class DbSetFinder : IDbSetFinder
    {

        /// <summary>
        /// Gets the name of the assembly to scan for entity types.
        /// </summary>
        /// <value>The assembly name provided via <see cref="DbFinderContextOptionsExtensionMethods.UseFinderIn"/>.</value>
        /// <remarks>
        /// Set in the constructor and used by <see cref="FindSets"/> to locate the target assembly
        /// in the current application domain.
        /// </remarks>
        private string AssemblyName { get; }

        /// <summary>
        /// Gets the root namespace within the assembly to scan for entity types.
        /// </summary>
        /// <value>The root namespace provided via <see cref="DbFinderContextOptionsExtensionMethods.UseFinderIn"/>.</value>
        /// <remarks>
        /// Set in the constructor and used by <see cref="FindSets"/> to filter types by namespace.
        /// Only types whose namespace matches or starts with this value are considered.
        /// </remarks>
        private string RootNamespace { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="DbSetFinder"/> with the assembly name and root namespace to scan.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly to scan for entity types.</param>
        /// <param name="rootNamespace">The root namespace within the assembly to scan.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="assemblyName"/> or <paramref name="rootNamespace"/> is null or empty.
        /// </exception>
        /// <remarks>
        /// Called internally by <see cref="DbFinderContextOptionsExtension.ApplyServices"/> when registering
        /// <see cref="DbSetFinder"/> into EF Core's internal service provider.
        /// </remarks>
        public DbSetFinder(string assemblyName, string rootNamespace)
        {
            ArgumentException.ThrowIfNullOrEmpty(assemblyName);
            ArgumentException.ThrowIfNullOrEmpty(rootNamespace);
            AssemblyName = assemblyName;
            RootNamespace = rootNamespace;
        }

        /// <summary>
        /// Scans the configured assembly and root namespace for entity types annotated with <see cref="TableAttribute"/>
        /// and returns them as <see cref="DbSetProperty"/> instances for EF Core to register in the model.
        /// </summary>
        /// <param name="contextType">The <c>DbContext</c> type. Not used by EP Finder — entity discovery is namespace-based.</param>
        /// <returns>A read-only list of <see cref="DbSetProperty"/> representing the discovered entity types.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the configured assembly name does not match any loaded assembly.
        /// </exception>
        /// <remarks>
        /// Called by EF Core during model building. Scans <see cref="AssemblyName"/> for all types
        /// whose namespace matches <see cref="RootNamespace"/> and are annotated with <see cref="TableAttribute"/>.
        /// </remarks>
        public IReadOnlyList<DbSetProperty> FindSets(Type contextType)
        {

            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == AssemblyName)
                ?? throw new InvalidOperationException($"Assembly '{AssemblyName}' was not found.");

            return assembly.GetTypes()
                .Where(type => type.Namespace != null
                            && (type.Namespace == RootNamespace || type.Namespace.StartsWith(RootNamespace + "."))
                            && type.GetCustomAttribute<TableAttribute>() != null)
                .Select(type => new DbSetProperty(type.Name, type, null))
                .ToList();

        }

    }
}