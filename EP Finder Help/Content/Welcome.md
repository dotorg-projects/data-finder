---
uid: Welcome
title: Welcome to EP Finder
keywords: Welcome, EP Finder, Entity Platform, EF Core
---

# EP Finder — Entity Platform Finder

EP Finder is a module of the **Entity Platform (EP)** team of [.ORG Projects](https://github.com/dotorg-projects), designed to eliminate the need to manually declare `DbContext` and its `DbSet<T>` properties in applications that use Entity Framework Core.

EP Finder scans an assembly and root namespace looking for classes annotated with the `[Table]` data attribute and registers them automatically in EF Core by replacing its internal `IDbSetFinder` service.

> **Note:** EP Finder only works with entities configured via Data Attributes. Fluent API is not supported and there are no plans to support it.

## Installation

```
dotnet add package DotOrgProjects.EntityPlatform.Finder
```

## Usage

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DotOrgProjects.EntityPlatform.Finder;

namespace MyApp {
    class Program {
        static void Main(string[] args) {
            ServiceCollection services = new ServiceCollection();

            services.AddDbContext<DbFoundContext>(options => {
                options.UseSqlServer("Server=localhost;Database=MyDb;");
                options.UseFinderIn("MyApp.Assembly", "MyApp.Entities.Root.Namespace");
            });

            ServiceProvider provider = services.BuildServiceProvider();

            DbContext context = provider.GetRequiredService<DbFoundContext>();

            List<Product> products = context.Set<Product>().ToList();
        }
    }
}
```

The developer always works with `Microsoft.EntityFrameworkCore.DbContext` — the internal classes of EP Finder are never referenced directly.

## See Also

**Other Resources**  
[](@VersionHistory)