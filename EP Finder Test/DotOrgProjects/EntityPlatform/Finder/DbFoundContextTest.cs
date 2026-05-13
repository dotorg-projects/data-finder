// Copyright (c) 2025 .ORG Projects. All rights reserved.
// Licensed under the Microsoft Public License (Ms-PL).
// See LICENSE.txt in the project root for license information.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace DotOrgProjects.EntityPlatform.Finder
{
    [TestClass]
    public class DbFoundContextTest
    {

        [TestMethod]
        public void Constructor_WithValidOptions_DoesNotThrow()
        {
            DbContextOptions options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase("TestDb")
                .Options;

            DbFoundContext context = new(options);

            Assert.IsNotNull(context);
        }

        [TestMethod]
        public void Constructor_WithNullOptions_ThrowsException()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() => new DbFoundContext(null!));
        }

        [TestMethod]
        public void OnConfiguring_ReplacesDbSetFinder_WithEpFinderDbSetFinder()
        {
            DbContextOptions options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase("TestDb")
                .UseFinderIn("DotOrgProjects.EntityPlatform.Finder.Tests", "DotOrgProjects.EntityPlatform.Finder.Fakes.Entities")
                .Options;

            DbFoundContext context = new(options);
            IDbSetFinder? dbSetFinder = context.GetInfrastructure().GetService<IDbSetFinder>();

            Assert.IsInstanceOfType(dbSetFinder, typeof(DbSetFinder));
        }

    }
}