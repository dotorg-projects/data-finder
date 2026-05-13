// Copyright (c) 2025 .ORG Projects. All rights reserved.
// Licensed under the Microsoft Public License (Ms-PL).
// See LICENSE.txt in the project root for license information.
namespace DotOrgProjects.EntityPlatform.Finder.Sample.Settings
{
    public class DbSetting
    {

        public static string UseDatasource()
        {

            string dbFolder = Path.Combine(Directory.GetCurrentDirectory(), "DbSample");

            Directory.CreateDirectory(dbFolder);

            string dbPath = Path.Combine(dbFolder, "sample.db");

            return "Data Source=" + dbPath;

        }

    }
}
