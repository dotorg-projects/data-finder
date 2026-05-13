// Copyright (c) 2025 .ORG Projects. All rights reserved.
// Licensed under the Microsoft Public License (Ms-PL).
// See LICENSE.txt in the project root for license information.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace DotOrgProjects.EntityPlatform.Finder
{
    [TestClass]
    public class DbFinderContextOptionsExtensionTest
    {

        [TestMethod]
        public void WithFinderIn_SetsAssemblyName()
        {
            DbFinderContextOptionsExtension extension = new();

            DbFinderContextOptionsExtension result = extension.WithFinderIn("MyAssembly", "MyNamespace");

            Assert.AreEqual("MyAssembly", result.AssemblyName);
        }

        [TestMethod]
        public void WithFinderIn_SetsRootNamespace()
        {
            DbFinderContextOptionsExtension extension = new();

            DbFinderContextOptionsExtension result = extension.WithFinderIn("MyAssembly", "MyNamespace");

            Assert.AreEqual("MyNamespace", result.RootNamespace);
        }

        [TestMethod]
        public void WithFinderIn_ReturnsNewInstance()
        {
            DbFinderContextOptionsExtension extension = new();

            DbFinderContextOptionsExtension result = extension.WithFinderIn("MyAssembly", "MyNamespace");

            Assert.AreNotSame(extension, result);
        }

        [TestMethod]
        public void ApplyServices_RegistersDbSetFinder()
        {
            DbFinderContextOptionsExtension extension = new DbFinderContextOptionsExtension()
                .WithFinderIn("DotOrgProjects.EntityPlatform.Finder.Tests", "DotOrgProjects.EntityPlatform.Finder.Fakes.Entities");

            ServiceCollection services = new();
            services.AddSingleton<IDbSetFinder, DbSetFinder>(sp => new DbSetFinder("Default", "Default"));
            extension.ApplyServices(services);

            ServiceProvider provider = services.BuildServiceProvider();
            IDbSetFinder dbSetFinder = provider.GetRequiredService<IDbSetFinder>();
            IReadOnlyList<DbSetProperty> sets = dbSetFinder.FindSets(typeof(object));

            Assert.IsTrue(sets.Any(s => s.Type == typeof(Fakes.Entities.TestEntity)));
        }

        [TestMethod]
        public void Info_IsDatabaseProvider_ReturnsFalse()
        {
            DbFinderContextOptionsExtension extension = new DbFinderContextOptionsExtension()
                .WithFinderIn("MyAssembly", "MyNamespace");

            DbContextOptionsExtensionInfo info = extension.Info;

            Assert.IsFalse(info.IsDatabaseProvider);
        }

        [TestMethod]
        public void Info_LogFragment_ContainsAssemblyName()
        {
            DbFinderContextOptionsExtension extension = new DbFinderContextOptionsExtension()
                .WithFinderIn("MyAssembly", "MyNamespace");

            DbContextOptionsExtensionInfo info = extension.Info;

            Assert.IsTrue(info.LogFragment.Contains("MyAssembly"));
        }

        [TestMethod]
        public void Info_LogFragment_ContainsRootNamespace()
        {
            DbFinderContextOptionsExtension extension = new DbFinderContextOptionsExtension()
                .WithFinderIn("MyAssembly", "MyNamespace");

            DbContextOptionsExtensionInfo info = extension.Info;

            Assert.IsTrue(info.LogFragment.Contains("MyNamespace"));
        }

        [TestMethod]
        public void Info_GetServiceProviderHashCode_ReturnsSameHashForSameConfiguration()
        {
            DbFinderContextOptionsExtension extension1 = new DbFinderContextOptionsExtension()
                .WithFinderIn("MyAssembly", "MyNamespace");

            DbFinderContextOptionsExtension extension2 = new DbFinderContextOptionsExtension()
                .WithFinderIn("MyAssembly", "MyNamespace");

            Assert.AreEqual(extension1.Info.GetServiceProviderHashCode(), extension2.Info.GetServiceProviderHashCode());
        }

        [TestMethod]
        public void Info_GetServiceProviderHashCode_ReturnsDifferentHashForDifferentConfiguration()
        {
            DbFinderContextOptionsExtension extension1 = new DbFinderContextOptionsExtension()
                .WithFinderIn("MyAssembly", "MyNamespace");

            DbFinderContextOptionsExtension extension2 = new DbFinderContextOptionsExtension()
                .WithFinderIn("OtherAssembly", "OtherNamespace");

            Assert.AreNotEqual(extension1.Info.GetServiceProviderHashCode(), extension2.Info.GetServiceProviderHashCode());
        }

        [TestMethod]
        public void Info_ShouldUseSameServiceProvider_ReturnsTrueForSameConfiguration()
        {
            DbFinderContextOptionsExtension extension1 = new DbFinderContextOptionsExtension()
                .WithFinderIn("MyAssembly", "MyNamespace");

            DbFinderContextOptionsExtension extension2 = new DbFinderContextOptionsExtension()
                .WithFinderIn("MyAssembly", "MyNamespace");

            Assert.IsTrue(extension1.Info.ShouldUseSameServiceProvider(extension2.Info));
        }

        [TestMethod]
        public void Info_ShouldUseSameServiceProvider_ReturnsFalseForDifferentConfiguration()
        {
            DbFinderContextOptionsExtension extension1 = new DbFinderContextOptionsExtension()
                .WithFinderIn("MyAssembly", "MyNamespace");

            DbFinderContextOptionsExtension extension2 = new DbFinderContextOptionsExtension()
                .WithFinderIn("OtherAssembly", "OtherNamespace");

            Assert.IsFalse(extension1.Info.ShouldUseSameServiceProvider(extension2.Info));
        }

        [TestMethod]
        public void Info_PopulateDebugInfo_ContainsAssemblyName()
        {
            DbFinderContextOptionsExtension extension = new DbFinderContextOptionsExtension()
                .WithFinderIn("MyAssembly", "MyNamespace");

            Dictionary<string, string> debugInfo = new();
            extension.Info.PopulateDebugInfo(debugInfo);

            Assert.AreEqual("MyAssembly", debugInfo["Finder:AssemblyName"]);
        }

        [TestMethod]
        public void Info_PopulateDebugInfo_ContainsRootNamespace()
        {
            DbFinderContextOptionsExtension extension = new DbFinderContextOptionsExtension()
                .WithFinderIn("MyAssembly", "MyNamespace");

            Dictionary<string, string> debugInfo = new();
            extension.Info.PopulateDebugInfo(debugInfo);

            Assert.AreEqual("MyNamespace", debugInfo["Finder:RootNamespace"]);
        }

        [TestMethod]
        public void Info_PopulateDebugInfo_WithNullValues_ContainsEmptyStrings()
        {
            DbFinderContextOptionsExtension extension = new();

            Dictionary<string, string> debugInfo = new();
            extension.Info.PopulateDebugInfo(debugInfo);

            Assert.AreEqual("", debugInfo["Finder:AssemblyName"]);
            Assert.AreEqual("", debugInfo["Finder:RootNamespace"]);
        }

    }
}