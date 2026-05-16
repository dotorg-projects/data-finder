# EP Finder — Entity Platform Finder

EP Finder is a module of the **Entity Platform (EP)** team of [.ORG Projects](https://github.com/dotorg-projects), designed to eliminate the need to manually declare `DbContext` and its `DbSet<T>` properties in applications that use Entity Framework Core.

EP Finder scans an assembly and root namespace looking for classes annotated with the `[Table]` data attribute and registers them automatically in EF Core by replacing its internal `IDbSetFinder` service.

> **Note:** EP Finder only works with entities configured via Data Attributes. Fluent API is not supported and there are no plans to support it.

---
## Help

Full Help is available at [EP Finder Help](https://dotorg-projects.github.io/data-finder/).

---

## Installation

```
dotnet add package DotOrgProjects.EntityPlatform.Finder
```

---

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
                options.UseSqlServer(DbSettings.GetDataSource());
                options.UseFinderIn("MyApp.Assembly", "MyApp.Entities.Root.Namespace");
            });

            ServiceProvider provider = services.BuildServiceProvider();

            DbFoundContext context = provider.GetRequiredService<DbFoundContext>();

            context.Database.EnsureCreated();

            List<Product> products = context.Set<Product>().ToList();

        }
    }
}
```

The developer uses `DbFoundContext` from EP Finder to register the context — the rest of the internal classes are never referenced directly.

---

## How It Works

`DbSetFinder` replaces EF Core's internal `IDbSetFinder` via `ReplaceService<IDbSetFinder, DbSetFinder>()`. Instead of looking for `DbSet<T>` properties declared in the `DbContext`, it scans the assembly and root namespace configured in `DbFinderContextOptionsExtension` looking for classes annotated with `[Table]` and registers them as model entities.

EF Core guarantees that `OnConfiguring` always runs regardless of how the `DbFoundContext` instance is created, so `DbSetFinder` is always correctly registered.

> **Warning:** `IDbSetFinder` is an internal EF Core API subject to changes between versions without prior notice. It has not changed between EF Core 8, 9, and 10.

---

## Migrations

EF Core discovers `DbFoundContext` at design time through DI — when running the app startup it finds `DbFoundContext` registered with `AddDbContext<DbFoundContext>()` and uses it to generate migrations. No additional design time class is required.

---

## License

EP Finder is licensed under the [Microsoft Public License (Ms-PL)](LICENSE.txt).