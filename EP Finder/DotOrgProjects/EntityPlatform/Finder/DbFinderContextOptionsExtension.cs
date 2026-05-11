// Copyright (c) 2025 .ORG Projects. All rights reserved.
// Licensed under the Microsoft Public License (Ms-PL).
// See LICENSE.txt in the project root for license information.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace DotOrgProjects.EntityPlatform.Finder
{

    /// <summary>
    /// EF Core options extension that stores EP Finder configuration — the assembly name and root namespace
    /// to scan for entity types. Added to <see cref="DbContextOptionsBuilder"/> via
    /// <see cref="DbFinderContextOptionsExtensionMethods.UseFinderIn"/>.
    /// </summary>
    public class DbFinderContextOptionsExtension : IDbContextOptionsExtension
    {

        /// <summary>
        /// Gets the name of the assembly to scan for entity types.
        /// </summary>
        public string AssemblyName { get; private set; }

        /// <summary>
        /// Gets the root namespace within the assembly to scan for entity types.
        /// </summary>
        public string RootNamespace { get; private set; }

        private DbContextOptionsExtensionInfo _info;

        /// <summary>
        /// Gets the metadata for this extension, used by EF Core to identify and compare extensions.
        /// </summary>
        public DbContextOptionsExtensionInfo Info =>
            _info ??= new DbFinderContextOptionsExtensionInfo(this);

        /// <summary>
        /// Initializes a new empty instance of <see cref="DbFinderContextOptionsExtension"/>.
        /// </summary>
        public DbFinderContextOptionsExtension() { }

        /// <summary>
        /// Initializes a new instance of <see cref="DbFinderContextOptionsExtension"/> by copying
        /// the values from an existing instance. Used internally by <see cref="WithFinderIn"/>.
        /// </summary>
        /// <param name="options">The instance to copy values from.</param>
        private DbFinderContextOptionsExtension(DbFinderContextOptionsExtension options)
        {
            AssemblyName = options.AssemblyName;
            RootNamespace = options.RootNamespace;
        }

        /// <summary>
        /// Returns a new <see cref="DbFinderContextOptionsExtension"/> instance with the specified
        /// assembly name and root namespace, following the immutable pattern used by EF Core extensions.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly to scan for entity types.</param>
        /// <param name="rootNamespace">The root namespace within the assembly to scan.</param>
        /// <returns>A new configured instance of <see cref="DbFinderContextOptionsExtension"/>.</returns>
        public DbFinderContextOptionsExtension WithFinderIn(string assemblyName, string rootNamespace)
        {
            DbFinderContextOptionsExtension options = new DbFinderContextOptionsExtension(this);
            options.AssemblyName = assemblyName;
            options.RootNamespace = rootNamespace;
            return options;
        }

        /// <summary>
        /// Registers additional services required by this extension into EF Core's internal service provider.
        /// EP Finder does not require additional services.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        public void ApplyServices(IServiceCollection services) { }

        /// <summary>
        /// Validates the extension configuration against the current EF Core options.
        /// EP Finder does not perform additional validation.
        /// </summary>
        /// <param name="options">The current EF Core context options.</param>
        public void Validate(IDbContextOptions options) { }

        /// <summary>
        /// Provides metadata about <see cref="DbFinderContextOptionsExtension"/> to EF Core,
        /// used for logging, service provider caching, and extension comparison.
        /// </summary>
        private sealed class DbFinderContextOptionsExtensionInfo : DbContextOptionsExtensionInfo
        {

            /// <summary>
            /// Initializes a new instance of <see cref="DbFinderContextOptionsExtensionInfo"/>.
            /// </summary>
            /// <param name="extension">The extension this info belongs to.</param>
            public DbFinderContextOptionsExtensionInfo(IDbContextOptionsExtension extension) : base(extension)
            {
            }

            /// <summary>
            /// Gets a value indicating that EP Finder is not a database provider.
            /// </summary>
            public override bool IsDatabaseProvider => false;

            /// <summary>
            /// Gets the text fragment included in EF Core logs to describe the EP Finder configuration.
            /// </summary>
            public override string LogFragment =>
                $"AssemblyName={((DbFinderContextOptionsExtension)Extension).AssemblyName} " +
                $"RootNamespace={((DbFinderContextOptionsExtension)Extension).RootNamespace} ";

            /// <summary>
            /// Returns a hash code based on <see cref="DbFinderContextOptionsExtension.AssemblyName"/>
            /// and <see cref="DbFinderContextOptionsExtension.RootNamespace"/>, used by EF Core
            /// to determine whether its internal service provider can be reused.
            /// </summary>
            /// <returns>A hash code representing the current EP Finder configuration.</returns>
            public override int GetServiceProviderHashCode() => HashCode.Combine(
                ((DbFinderContextOptionsExtension)Extension).AssemblyName,
                ((DbFinderContextOptionsExtension)Extension).RootNamespace
            );

            /// <summary>
            /// Determines whether two EP Finder extensions share the same configuration
            /// and can therefore share EF Core's internal service provider.
            /// </summary>
            /// <param name="other">The other extension info to compare against.</param>
            /// <returns><c>true</c> if both extensions have the same assembly name and root namespace; otherwise <c>false</c>.</returns>
            public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) =>
                other is DbFinderContextOptionsExtensionInfo otherInfo &&
                ((DbFinderContextOptionsExtension)Extension).AssemblyName ==
                ((DbFinderContextOptionsExtension)otherInfo.Extension).AssemblyName &&
                ((DbFinderContextOptionsExtension)Extension).RootNamespace ==
                ((DbFinderContextOptionsExtension)otherInfo.Extension).RootNamespace;

            /// <summary>
            /// Populates the EF Core debug diagnostics dictionary with the current EP Finder configuration.
            /// </summary>
            /// <param name="debugInfo">The dictionary to populate with diagnostic information.</param>
            public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
            {
                debugInfo["Finder:AssemblyName"] =
                    ((DbFinderContextOptionsExtension)Extension).AssemblyName ?? "";
                debugInfo["Finder:RootNamespace"] =
                    ((DbFinderContextOptionsExtension)Extension).RootNamespace ?? "";
            }

        }

    }
}
