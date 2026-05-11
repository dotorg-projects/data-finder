// Copyright (c) 2025 .ORG Projects. All rights reserved.
// Licensed under the Microsoft Public License (Ms-PL).
// See LICENSE.txt in the project root for license information.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DotOrgProjects.EntityPlatform.Finder
{

    /// <summary>
    /// The EP Finder <see cref="DbContext"/> that replaces EF Core's default entity discovery mechanism
    /// with <see cref="DbSetFinder"/>, enabling automatic entity registration without explicit
    /// <c>DbSet&lt;T&gt;</c> property declarations.
    /// </summary>
    public class DbFoundContext : DbContext
    {

        /// <summary>
        /// Initializes a new instance of <see cref="DbFoundContext"/> with the specified EF Core options.
        /// </summary>
        /// <param name="options">The options to configure the context, including the EP Finder settings.</param>
        public DbFoundContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Replaces EF Core's internal <see cref="IDbSetFinder"/> with <see cref="DbSetFinder"/>
        /// to enable namespace-based entity discovery.
        /// </summary>
        /// <param name="optionsBuilder">The builder used to configure the context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ReplaceService<IDbSetFinder, DbSetFinder>();
        }

    }
}
