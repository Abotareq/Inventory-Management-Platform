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

        public decimal Price { get; private set; }

        private Product(
            ProductId productId,
            string name,
            string sku,
            string? description,
            CategoryId? categoryId,
            decimal price)
            : base(productId.Value)
        {
            ProductId = productId;
            Name = name;
            Sku = sku;
            Description = description;
            CategoryId = categoryId;
            Price = price;
        }

        private Product() { }

        public static ErrorOr<Product> Create(
            string name,
            string sku,
            string? description,
            CategoryId? categoryId,
            decimal price)
        {
            var errors = Validate(name, sku, price);
            if (errors.Count > 0)
                return errors;

            return new Product(
                ProductId.CreateUnique(), name, sku, description, categoryId, price);
        }

        public ErrorOr<Updated> UpdateDetails(
            string name,
            string sku,
            string? description,
            CategoryId? categoryId,
            decimal price)
        {
            var errors = Validate(name, sku, price);
            if (errors.Count > 0)
                return errors;

            Name = name;
            Sku = sku;
            Description = description;
            CategoryId = categoryId;
            Price = price;

            return Result.Updated;
        }

        private static List<Error> Validate(string name, string sku, decimal price)
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

            if (price < 0)
                errors.Add(Errors.Product.InvalidPrice);

            return errors;
        }
    }
}
