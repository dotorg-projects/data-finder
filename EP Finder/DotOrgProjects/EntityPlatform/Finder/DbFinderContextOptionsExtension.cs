// Copyright (c) 2025 .ORG Projects. All rights reserved.
// Licensed under the Microsoft Public License (Ms-PL).
// See LICENSE.txt in the project root for license information.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DotOrgProjects.EntityPlatform.Finder
{

    /// <summary>
    /// EF Core options extension that stores EP Finder configuration — the assembly name and root namespace
    /// to scan for entity types. Added to <see cref="DbContextOptionsBuilder"/> via
    /// <see cref="DbFinderContextOptionsExtensionMethods.UseFinderIn"/>.
    /// </summary>
    /// <remarks>
    /// Implements <see cref="IDbContextOptionsExtension"/> following EF Core's immutable extension pattern.
    /// Instances are created via <see cref="WithFinderIn"/> and registered into the
    /// <see cref="DbContextOptionsBuilder"/> by <see cref="DbFinderContextOptionsExtensionMethods.UseFinderIn"/>.
    /// The extension registers <see cref="DbSetFinder"/> into EF Core's internal service provider
    /// via <see cref="ApplyServices"/>.
    /// </remarks>
    public class DbFinderContextOptionsExtension : IDbContextOptionsExtension
    {

        /// <summary>
        /// Gets the name of the assembly to scan for entity types.
        /// </summary>
        /// <value>The assembly name provided via <see cref="WithFinderIn"/>, or <c>null</c> if not configured.</value>
        /// <remarks>
        /// Used by <see cref="ApplyServices"/> to configure <see cref="DbSetFinder"/> and by
        /// <see cref="DbFinderContextOptionsExtensionInfo"/> for logging and service provider caching.
        /// </remarks>
        public string? AssemblyName { get; private set; }

        /// <summary>
        /// Gets the root namespace within the assembly to scan for entity types.
        /// </summary>
        /// <value>The root namespace provided via <see cref="WithFinderIn"/>, or <c>null</c> if not configured.</value>
        /// <remarks>
        /// Used by <see cref="ApplyServices"/> to configure <see cref="DbSetFinder"/> and by
        /// <see cref="DbFinderContextOptionsExtensionInfo"/> for logging and service provider caching.
        /// </remarks>
        public string? RootNamespace { get; private set; }

        /// <summary>
        /// Backing field for the <see cref="Info"/> property.
        /// </summary>
        /// <remarks>
        /// Lazily initialized on first access via the <see cref="Info"/> property getter.
        /// </remarks>
        private DbContextOptionsExtensionInfo? _info;

        /// <summary>
        /// Gets the metadata for this extension, used by EF Core to identify and compare extensions.
        /// </summary>
        /// <value>A <see cref="DbFinderContextOptionsExtensionInfo"/> instance for this extension.</value>
        /// <remarks>
        /// Lazily initialized on first access. Used by EF Core for logging, service provider caching,
        /// and extension comparison.
        /// </remarks>
        public DbContextOptionsExtensionInfo Info =>
            _info ??= new DbFinderContextOptionsExtensionInfo(this);

        /// <summary>
        /// Initializes a new empty instance of <see cref="DbFinderContextOptionsExtension"/>.
        /// </summary>
        /// <remarks>
        /// This constructor is used by EF Core internally. To configure EP Finder,
        /// use <see cref="WithFinderIn"/> after construction.
        /// </remarks>
        public DbFinderContextOptionsExtension() { }

        /// <summary>
        /// Initializes a new instance of <see cref="DbFinderContextOptionsExtension"/> with the specified
        /// assembly name and root namespace. Used internally by <see cref="WithFinderIn"/>.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly to scan for entity types.</param>
        /// <param name="rootNamespace">The root namespace within the assembly to scan.</param>
        /// <remarks>
        /// Private constructor following EF Core's immutable extension pattern.
        /// Always called through <see cref="WithFinderIn"/>.
        /// </remarks>
        private DbFinderContextOptionsExtension(string assemblyName, string rootNamespace)
        {
            AssemblyName = assemblyName;
            RootNamespace = rootNamespace;
        }

        /// <summary>
        /// Returns a new <see cref="DbFinderContextOptionsExtension"/> instance with the specified
        /// assembly name and root namespace, following the immutable pattern used by EF Core extensions.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly to scan for entity types.</param>
        /// <param name="rootNamespace">The root namespace within the assembly to scan.</param>
        /// <returns>A new configured instance of <see cref="DbFinderContextOptionsExtension"/>.</returns>
        /// <remarks>
        /// Called internally by <see cref="DbFinderContextOptionsExtensionMethods.UseFinderIn"/>
        /// to produce a configured extension instance following EF Core's immutable extension pattern.
        /// </remarks>
        public DbFinderContextOptionsExtension WithFinderIn(string assemblyName, string rootNamespace)
        {
            DbFinderContextOptionsExtension options = new(assemblyName, rootNamespace);
            options.AssemblyName = assemblyName;
            options.RootNamespace = rootNamespace;
            return options;
        }

        /// <summary>
        /// Registers <see cref="DbSetFinder"/> into EF Core's internal service provider,
        /// replacing the default <see cref="IDbSetFinder"/> with the EP Finder implementation
        /// configured with the current assembly name and root namespace.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <remarks>
        /// Called by EF Core when building its internal service provider.
        /// Replaces the default <see cref="IDbSetFinder"/> singleton with a <see cref="DbSetFinder"/>
        /// instance configured with <see cref="AssemblyName"/> and <see cref="RootNamespace"/>.
        /// </remarks>
        public void ApplyServices(IServiceCollection services)
        {
            services.Replace(ServiceDescriptor.Singleton<IDbSetFinder>(
                new DbSetFinder(AssemblyName!, RootNamespace!)));
        }

        /// <summary>
        /// Validates the extension configuration against the current EF Core options.
        /// EP Finder does not perform additional validation.
        /// </summary>
        /// <param name="options">The current EF Core context options.</param>
        /// <remarks>
        /// No validation is performed by EP Finder. Configuration errors surface at runtime
        /// when <see cref="DbSetFinder.FindSets"/> is invoked by EF Core.
        /// </remarks>
        public void Validate(IDbContextOptions options) { }

        /// <summary>
        /// Provides metadata about <see cref="DbFinderContextOptionsExtension"/> to EF Core,
        /// used for logging, service provider caching, and extension comparison.
        /// </summary>
        /// <remarks>
        /// Sealed nested class implementing <see cref="DbContextOptionsExtensionInfo"/> for
        /// <see cref="DbFinderContextOptionsExtension"/>. Exposes <see cref="AssemblyName"/>
        /// and <see cref="DbFinderContextOptionsExtension.RootNamespace"/> to EF Core's infrastructure.
        /// </remarks>
        private sealed class DbFinderContextOptionsExtensionInfo : DbContextOptionsExtensionInfo
        {

            /// <summary>
            /// Initializes a new instance of <see cref="DbFinderContextOptionsExtensionInfo"/>.
            /// </summary>
            /// <param name="extension">The extension this info belongs to.</param>
            /// <remarks>
            /// Called lazily by <see cref="DbFinderContextOptionsExtension.Info"/> on first access.
            /// </remarks>
            public DbFinderContextOptionsExtensionInfo(IDbContextOptionsExtension extension) : base(extension)
            {
            }

            /// <summary>
            /// Gets a value indicating that EP Finder is not a database provider.
            /// </summary>
            /// <value>Always <c>false</c>.</value>
            /// <remarks>
            /// EP Finder is a configuration extension, not a database provider.
            /// EF Core uses this value to determine service provider sharing behavior.
            /// </remarks>
            public override bool IsDatabaseProvider => false;

            /// <summary>
            /// Gets the text fragment included in EF Core logs to describe the EP Finder configuration.
            /// </summary>
            /// <value>A string containing the configured <see cref="DbFinderContextOptionsExtension.AssemblyName"/> and <see cref="DbFinderContextOptionsExtension.RootNamespace"/>.</value>
            /// <remarks>
            /// Included in EF Core's debug log output to identify the EP Finder configuration.
            /// </remarks>
            public override string LogFragment =>
                $"AssemblyName={((DbFinderContextOptionsExtension)Extension).AssemblyName} " +
                $"RootNamespace={((DbFinderContextOptionsExtension)Extension).RootNamespace} ";

            /// <summary>
            /// Returns a hash code based on <see cref="DbFinderContextOptionsExtension.AssemblyName"/>
            /// and <see cref="DbFinderContextOptionsExtension.RootNamespace"/>, used by EF Core
            /// to determine whether its internal service provider can be reused.
            /// </summary>
            /// <returns>A hash code representing the current EP Finder configuration.</returns>
            /// <remarks>
            /// EF Core uses this hash code to determine whether a cached internal service provider
            /// can be reused across <see cref="DbFoundContext"/> instances with the same configuration.
            /// </remarks>
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
            /// <remarks>
            /// Returns <c>true</c> only when <paramref name="other"/> is also a
            /// <see cref="DbFinderContextOptionsExtensionInfo"/> with identical
            /// <see cref="DbFinderContextOptionsExtension.AssemblyName"/> and
            /// <see cref="DbFinderContextOptionsExtension.RootNamespace"/> values.
            /// </remarks>
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
            /// <remarks>
            /// Adds <c>Finder:AssemblyName</c> and <c>Finder:RootNamespace</c> entries to the
            /// <paramref name="debugInfo"/> dictionary for EF Core diagnostic purposes.
            /// </remarks>
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