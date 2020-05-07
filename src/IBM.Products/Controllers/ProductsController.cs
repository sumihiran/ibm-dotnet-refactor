using System;
using IBM.Products.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBM.Products.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase 
    {
        [HttpGet]
        public Models.Products Index([FromQuery] string name)
        {
            return name is null ? new Models.Products() : new Models.Products(name);
        }

        [HttpGet]
        [Route("{id}")]
        public Product GetProduct(Guid id)
        {
            var product = new Product(id);
            if (product.IsNew)
                throw new ApplicationException("Product not found");

            return product;
        }

        [HttpPost]
        public void Create(Product product)
        {
            product.Save();
        }

        [HttpPut]
        [Route("{id}")]
        public void Update(Guid id, Product product)
        {
            var orig = new Product(id)
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DeliveryPrice = product.DeliveryPrice
            };

            if (!orig.IsNew)
                orig.Save();
        }

        [HttpDelete]
        [Route("{id}")]
        public void Delete(Guid id)
        {
            var product = new Product(id);
            product.Delete();
        }

        [HttpGet]
        [Route("{productId}/options")]
        public ProductOptions GetOptions(Guid productId)
        {
            return new ProductOptions(productId);
        }

        [HttpGet]
        [Route("{productId}/options/{id}")]
        public ProductOption GetOption(Guid productId, Guid id)
        {
            var option = new ProductOption(id);
            if (option.IsNew)
                throw new ApplicationException("Product option not found");

            return option;
        }

        [HttpPost]
        [Route("{productId}/options")]
        public void CreateOption(Guid productId, ProductOption option)
        {
            option.ProductId = productId;
            option.Save();
        }

        [HttpPut]
        [Route("{productId}/options/{id}")]
        public void UpdateOption(Guid id, ProductOption option)
        {
            var orig = new ProductOption(id)
            {
                Name = option.Name,
                Description = option.Description
            };

            if (!orig.IsNew)
                orig.Save();
        }

        [HttpDelete]
        [Route("{productId}/options/{id}")]
        public void DeleteOption(Guid id)
        {
            var opt = new ProductOption(id);
            opt.Delete();
        }
    }
}