// Copyright (c) 2025 .ORG Projects. All rights reserved.
// Licensed under the Microsoft Public License (Ms-PL).
// See LICENSE.txt in the project root for license information.
using Microsoft.EntityFrameworkCore;

namespace DotOrgProjects.EntityPlatform.Finder
{

    /// <summary>
    /// The EP Finder <see cref="DbContext"/> that automatically discovers and registers entity types
    /// without requiring explicit <c>DbSet&lt;T&gt;</c> property declarations.
    /// Entity discovery is configured via <see cref="DbFinderContextOptionsExtensionMethods.UseFinderIn"/>.
    /// </summary>
    /// <remarks>
    /// <see cref="DbFoundContext"/> delegates entity discovery to <see cref="DbSetFinder"/>,
    /// which is registered into EF Core's internal service provider by
    /// <see cref="DbFinderContextOptionsExtension.ApplyServices"/>.
    /// The Dev never interacts with <see cref="DbSetFinder"/> directly — all configuration
    /// is done via <see cref="DbFinderContextOptionsExtensionMethods.UseFinderIn"/>.
    /// </remarks>
    public class DbFoundContext : DbContext
    {

        /// <summary>
        /// Initializes a new instance of <see cref="DbFoundContext"/> with the specified EF Core options.
        /// </summary>
        /// <param name="options">The options to configure the context, including the EP Finder settings.</param>
        /// <remarks>
        /// The <paramref name="options"/> must be configured with
        /// <see cref="DbFinderContextOptionsExtensionMethods.UseFinderIn"/> to enable automatic entity discovery.
        /// </remarks>
        public DbFoundContext(DbContextOptions options) : base(options) { }

    }

}