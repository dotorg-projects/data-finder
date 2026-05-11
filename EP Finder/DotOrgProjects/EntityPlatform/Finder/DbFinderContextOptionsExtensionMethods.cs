// Copyright (c) 2025 .ORG Projects. All rights reserved.
// Licensed under the Microsoft Public License (Ms-PL).
// See LICENSE.txt in the project root for license information.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DotOrgProjects.EntityPlatform.Finder
{

    /// <summary>
    /// Provides extension methods on <see cref="DbContextOptionsBuilder"/> to configure EP Finder.
    /// </summary>
    public static class DbFinderContextOptionsExtensionMethods
    {

        /// <summary>
        /// Configures EP Finder to scan the specified assembly and root namespace for entity types
        /// annotated with <see cref="System.ComponentModel.DataAnnotations.Schema.TableAttribute"/>.
        /// </summary>
        /// <param name="optionsBuilder">The <see cref="DbContextOptionsBuilder"/> being configured.</param>
        /// <param name="assemblyName">The name of the assembly to scan for entity types.</param>
        /// <param name="rootNamespace">The root namespace within the assembly to scan.</param>
        /// <returns>The same <see cref="DbContextOptionsBuilder"/> instance to allow further configuration chaining.</returns>
        public static DbContextOptionsBuilder UseFinderIn(
            this DbContextOptionsBuilder optionsBuilder,
            string assemblyName,
            string rootNamespace)
        {

            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(
                new DbFinderContextOptionsExtension().WithFinderIn(assemblyName, rootNamespace));

            return optionsBuilder;
        }

    }
}
