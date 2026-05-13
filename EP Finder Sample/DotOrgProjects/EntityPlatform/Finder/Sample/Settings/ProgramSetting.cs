// Copyright (c) 2025 .ORG Projects. All rights reserved.
// Licensed under the Microsoft Public License (Ms-PL).
// See LICENSE.txt in the project root for license information.
using DotOrgProjects.EntityPlatform.Finder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DotOrgProjects.EntityPlatform.Finder.Sample.Settings
{
    public class ProgramSetting
    {

        private static ProgramSetting? _settings;

        public ServiceCollection Services { get; private set; }

        public ServiceProvider Provider { get; private set; }

        public T GetType<T>() where T : class
        {
            return Provider.GetRequiredService<T>();
        }

        private ProgramSetting(ServiceCollection services)
        {
            Services = services;
            Provider = services.BuildServiceProvider();
        }

        public static ProgramSetting GetInstance()
        {

            ProgramSetting.Instantiate();

            return _settings;

        }

        public static void Instantiate() {

            if (_settings != null) { return; }

            ServiceCollection services = new ServiceCollection();

            services.AddDbContext<DbFoundContext>(options => {
                options.UseSqlite(DbSetting.UseDatasource());
                options.UseFinderIn("DotOrgProjects.EntityPlatform.Finder.Sample", "DotOrgProjects.EntityPlatform.Finder.Sample.Entities");
            });

            _settings = new(services);

            DbFoundContext context = _settings.GetType<DbFoundContext>();
            context.Database.EnsureCreated();

        }

    }

}
