// Copyright (c) 2025 .ORG Projects. All rights reserved.
// Licensed under the Microsoft Public License (Ms-PL).
// See LICENSE.txt in the project root for license information.
using Microsoft.Extensions.DependencyInjection;
using DotOrgProjects.EntityPlatform.Finder.Sample.Entities;
using DotOrgProjects.EntityPlatform.Finder.Sample.Settings;

namespace DotOrgProjects.EntityPlatform.Finder.Sample.Repositories
{

    public class ProductRepository
    {

        private readonly DbFoundContext _context = ProgramSetting.GetInstance().GetType<DbFoundContext>();

        public List<ProductEntity> FindAll() => _context.Set<ProductEntity>().ToList();

        public ProductEntity? FindById(int id) => _context.Set<ProductEntity>().Find(id);

        public List<ProductEntity> FindByType(string type) => _context.Set<ProductEntity>().Where(p => p.Type == type).ToList();

        public void Save(ProductEntity product)
        {
            _context.Set<ProductEntity>().Add(product);
            _context.SaveChanges();
        }

    }

}
