// Copyright (c) 2025 .ORG Projects. All rights reserved.
// Licensed under the Microsoft Public License (Ms-PL).
// See LICENSE.txt in the project root for license information.
using DotOrgProjects.EntityPlatform.Finder.Sample.Entities;
using DotOrgProjects.EntityPlatform.Finder.Sample.Models;
using DotOrgProjects.EntityPlatform.Finder.Sample.Repositories;

namespace DotOrgProjects.EntityPlatform.Finder.Sample.Services
{
    public class ProductService
    {

        private readonly ProductRepository _repository = new();

        public List<ProductModel> FindAll() =>
            _repository.FindAll().Select(e => new ProductModel { Id = e.Id, Name = e.Name, Price = e.Price, Type = e.Type }).ToList();

        public ProductModel? FindById(ProductModel model)
        {
            ProductEntity? entity = _repository.FindById(model.Id!.Value);
            if (entity == null) return null;
            return new ProductModel { Id = entity.Id, Name = entity.Name, Price = entity.Price, Type = entity.Type };
        }

        public List<ProductModel> FindByType(ProductModel model) =>
            _repository.FindByType(model.Type!).Select(e => new ProductModel { Id = e.Id, Name = e.Name, Price = e.Price, Type = e.Type }).ToList();

        public void Save(ProductModel model) =>
            _repository.Save(new ProductEntity { Name = model.Name!, Price = model.Price!.Value, Type = model.Type! });

    }
}
