using ErrorOr;
using Inventory_Management_Platform.Domain.Category.ValueObjects;
using Inventory_Management_Platform.Domain.Common.Models;
using Inventory_Management_Platform.Domain.DomainErrors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.Category
{
    public sealed class Category : AggregateRoot
    {
        public CategoryId CategoryId { get; private set; }
        public string Name { get; private set; }

        private Category(CategoryId categoryId, string name)
            : base(categoryId.Value)
        {
            CategoryId = categoryId;
            Name = name;
        }

        private Category() { }

        public static ErrorOr<Category> Create(string name)
        {
            var errors = Validate(name);
            if (errors.Count > 0)
                return errors;

            return new Category(CategoryId.CreateUnique(), name);
        }

        public ErrorOr<Updated> Rename(string newName)
        {
            var errors = Validate(newName);
            if (errors.Count > 0)
                return errors;

            Name = newName;
            return Result.Updated;
        }

        private static List<Error> Validate(string name)
        {
            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(name))
                errors.Add(Errors.Category.NameIsRequired);
            else if (name.Length > 100)
                errors.Add(Errors.Category.NameTooLong);

            return errors;
        }
    }
}
