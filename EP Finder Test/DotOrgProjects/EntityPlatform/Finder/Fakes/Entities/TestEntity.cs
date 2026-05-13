// Copyright (c) 2025 .ORG Projects. All rights reserved.
// Licensed under the Microsoft Public License (Ms-PL).
// See LICENSE.txt in the project root for license information.
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotOrgProjects.EntityPlatform.Finder.Fakes.Entities
{

    [Table("test")]
    internal class TestEntity
    {

        [Key]
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Type { get; set; }

    }

}
