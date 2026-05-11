// Copyright (c) 2025 .ORG Projects. All rights reserved.
// Licensed under the Microsoft Public License (Ms-PL).
// See LICENSE.txt in the project root for license information.
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace DotOrgProjects.EntityPlatform.Finder
{
    internal class DbSetFinder : IDbSetFinder
    {

        /// <summary>
        /// Replaces EF Core's default <see cref="IDbSetFinder"/> to discover entities dynamically
        /// by scanning a configured assembly and root namespace for classes annotated with <see cref="TableAttribute"/>,
        /// instead of requiring explicit <c>DbSet&lt;T&gt;</c> property declarations on the <c>DbContext</c>.
        /// </summary>
        private readonly IDbContextOptions _options;

        /// <summary>
        /// Initializes a new instance of <see cref="DbSetFinder"/> with the EF Core context options.
        /// </summary>
        /// <param name="options">The EF Core context options containing the <see cref="DbFinderContextOptionsExtension"/> configuration.</param>
        public DbSetFinder(IDbContextOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// Scans the configured assembly and root namespace for entity types annotated with <see cref="TableAttribute"/>
        /// and returns them as <see cref="DbSetProperty"/> instances for EF Core to register in the model.
        /// </summary>
        /// <param name="contextType">The <c>DbContext</c> type. Not used by EP Finder — entity discovery is namespace-based.</param>
        /// <returns>A read-only list of <see cref="DbSetProperty"/> representing the discovered entity types.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when EP Finder has not been configured via <c>UseFinderIn()</c>,
        /// or when the configured assembly name does not match any loaded assembly.
        /// </exception>
        public IReadOnlyList<DbSetProperty> FindSets(Type contextType)
        {

            DbFinderContextOptionsExtension extension = _options.FindExtension<DbFinderContextOptionsExtension>()
                ?? throw new InvalidOperationException("EP Finder is not configured. Use UseFinderIn() to configure it.");

            string assemblyName = extension.AssemblyName;
            string rootNamespace = extension.RootNamespace;

            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == assemblyName)
                ?? throw new InvalidOperationException($"Assembly '{assemblyName}' was not found.");

            return assembly.GetTypes()
                .Where(type => type.Namespace != null
                            && (type.Namespace == rootNamespace || type.Namespace.StartsWith(rootNamespace + "."))
                            && type.GetCustomAttribute<TableAttribute>() != null)
                .Select(type => new DbSetProperty(type.Name, type, null))
                .ToList();

        }

    }
}
