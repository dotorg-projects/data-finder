// Copyright (c) 2025 .ORG Projects. All rights reserved.
// Licensed under the Microsoft Public License (Ms-PL).
// See LICENSE.txt in the project root for license information.
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DotOrgProjects.EntityPlatform.Finder
{
    [TestClass]
    public class DbSetFinderTest
    {

        [TestMethod]
        public void Constructor_WithNullAssemblyName_ThrowsArgumentNullException()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() => new DbSetFinder(null!, "MyNamespace"));
        }

        [TestMethod]
        public void Constructor_WithEmptyAssemblyName_ThrowsArgumentException()
        {
            Assert.ThrowsExactly<ArgumentException>(() => new DbSetFinder("", "MyNamespace"));
        }

        [TestMethod]
        public void Constructor_WithNullRootNamespace_ThrowsArgumentNullException()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() => new DbSetFinder("MyAssembly", null!));
        }

        [TestMethod]
        public void Constructor_WithEmptyRootNamespace_ThrowsArgumentException()
        {
            Assert.ThrowsExactly<ArgumentException>(() => new DbSetFinder("MyAssembly", ""));
        }

        [TestMethod]
        public void FindSets_WithNonExistentAssembly_ThrowsInvalidOperationException()
        {
            DbSetFinder finder = new("NonExistent.Assembly", "NonExistent.Namespace");

            Assert.ThrowsExactly<InvalidOperationException>(() => finder.FindSets(typeof(object)));
        }

        [TestMethod]
        public void FindSets_WithValidConfiguration_ReturnsTestEntity()
        {
            DbSetFinder finder = new("DotOrgProjects.EntityPlatform.Finder.Tests", "DotOrgProjects.EntityPlatform.Finder.Fakes.Entities");

            IReadOnlyList<DbSetProperty> sets = finder.FindSets(typeof(object));

            Assert.IsTrue(sets.Any(s => s.Type == typeof(Fakes.Entities.TestEntity)));
        }

    }
}