// Copyright (c) 2025 .ORG Projects. All rights reserved.
// Licensed under the Microsoft Public License (Ms-PL).
// See LICENSE.txt in the project root for license information.
using Microsoft.EntityFrameworkCore;

namespace DotOrgProjects.EntityPlatform.Finder
{
    [TestClass]
    public class DbFinderContextOptionsExtensionMethodsTest
    {

        [TestMethod]
        public void UseFinderIn_WithNullOptionsBuilder_ThrowsArgumentNullException()
        {
            DbContextOptionsBuilder builder = null!;

            Assert.ThrowsExactly<ArgumentNullException>(() =>
                builder.UseFinderIn("MyAssembly", "MyNamespace"));
        }

        [TestMethod]
        public void UseFinderIn_WithNullAssemblyName_ThrowsArgumentNullException()
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();

            Assert.ThrowsExactly<ArgumentNullException>(() =>
                builder.UseFinderIn(null!, "MyNamespace"));
        }

        [TestMethod]
        public void UseFinderIn_WithEmptyAssemblyName_ThrowsArgumentException()
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();

            Assert.ThrowsExactly<ArgumentException>(() =>
                builder.UseFinderIn("", "MyNamespace"));
        }

        [TestMethod]
        public void UseFinderIn_WithNullRootNamespace_ThrowsArgumentNullException()
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();

            Assert.ThrowsExactly<ArgumentNullException>(() =>
                builder.UseFinderIn("MyAssembly", null!));
        }

        [TestMethod]
        public void UseFinderIn_WithEmptyRootNamespace_ThrowsArgumentException()
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();

            Assert.ThrowsExactly<ArgumentException>(() =>
                builder.UseFinderIn("MyAssembly", ""));
        }

        [TestMethod]
        public void UseFinderIn_ReturnsSameOptionsBuilder()
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder()
                .UseInMemoryDatabase("TestDb");

            DbContextOptionsBuilder result = builder.UseFinderIn("MyAssembly", "MyNamespace");

            Assert.AreSame(builder, result);
        }

        [TestMethod]
        public void UseFinderIn_AddsDbFinderContextOptionsExtension()
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder()
                .UseInMemoryDatabase("TestDb")
                .UseFinderIn("MyAssembly", "MyNamespace");

            DbFinderContextOptionsExtension? extension = builder.Options.FindExtension<DbFinderContextOptionsExtension>();

            Assert.IsNotNull(extension);
        }

        [TestMethod]
        public void UseFinderIn_SetsAssemblyName()
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder()
                .UseInMemoryDatabase("TestDb")
                .UseFinderIn("MyAssembly", "MyNamespace");

            DbFinderContextOptionsExtension? extension = builder.Options.FindExtension<DbFinderContextOptionsExtension>();

            Assert.AreEqual("MyAssembly", extension?.AssemblyName);
        }

        [TestMethod]
        public void UseFinderIn_SetsRootNamespace()
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder()
                .UseInMemoryDatabase("TestDb")
                .UseFinderIn("MyAssembly", "MyNamespace");

            DbFinderContextOptionsExtension? extension = builder.Options.FindExtension<DbFinderContextOptionsExtension>();

            Assert.AreEqual("MyNamespace", extension?.RootNamespace);
        }

    }
}
