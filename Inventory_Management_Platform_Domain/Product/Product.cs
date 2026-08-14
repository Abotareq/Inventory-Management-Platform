using ErrorOr;
using Inventory_Management_Platform.Domain.Category.ValueObjects;
using Inventory_Management_Platform.Domain.Common.Models;
using Inventory_Management_Platform.Domain.DomainErrors;
using Inventory_Management_Platform.Domain.Product.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.Product
{
    public sealed class Product : AggregateRoot
    {
        public ProductId ProductId { get; private set; }
        public string Name { get; private set; }
        public string Sku { get; private set; }
        public string? Description { get; private set; }
        public CategoryId? CategoryId { get; private set; }

        private Product(
            ProductId productId,
            string name,
            string sku,
            string? description,
            CategoryId? categoryId)
            : base(productId.Value)
        {
            ProductId = productId;
            Name = name;
            Sku = sku;
            Description = description;
            CategoryId = categoryId;
        }

        private Product() { }

        public static ErrorOr<Product> Create(
            string name,
            string sku,
            string? description,
            CategoryId? categoryId)
        {
            var errors = Validate(name, sku);
            if (errors.Count > 0)
                return errors;

            return new Product(
                ProductId.CreateUnique(),
                name,
                sku,
                description,
                categoryId);
        }

        public ErrorOr<Updated> UpdateDetails(
            string name,
            string sku,
            string? description,
            CategoryId? categoryId)
        {
            var errors = Validate(name, sku);
            if (errors.Count > 0)
                return errors;

            Name = name;
            Sku = sku;
            Description = description;
            CategoryId = categoryId;

            return Result.Updated;
        }

        private static List<Error> Validate(string name, string sku)
        {
            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(name))
                errors.Add(Errors.Product.NameIsRequired);
            else if (name.Length > 200)
                errors.Add(Errors.Product.NameTooLong);

            if (string.IsNullOrWhiteSpace(sku))
                errors.Add(Errors.Product.SkuIsRequired);
            else if (sku.Length > 50)
                errors.Add(Errors.Product.SkuTooLong);

            return errors;
        }
    }
}
