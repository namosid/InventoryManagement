using System.Collections.Generic;
using System.Linq;
using Inventory.Common;
using Inventory.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private InventoryContext inventoryContext;
        public InventoryController(InventoryContext inventory)
        {
            inventoryContext = inventory;
        }

        [HttpGet]
        public IEnumerable<Product> GetAllProducts()
        {
            IList<Product> products = inventoryContext.Product.ToList();
            return products;

        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(long id)
        {
            Product product = inventoryContext.Product.FirstOrDefault(x => x.ProductId == id);
            if (product == null)
            {
                return NotFound("The Product couldn't be found.");
            }
            return Ok(product);
        }

        [HttpPost]
        public IActionResult AddProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product is null.");
            }
            else
            {
                inventoryContext.Product.Add(product);
                inventoryContext.SaveChanges();
                return Ok();
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(long id, [FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product is null.");
            }
            Product productToUpdate = inventoryContext.Product.FirstOrDefault(x => x.ProductId == id);
            if (productToUpdate == null)
            {
                return NotFound("Product record couldn't be found with specified ID");
            }

            productToUpdate.CreatedDate = product.CreatedDate;
            productToUpdate.ModifiedBy = product.ModifiedBy;
            productToUpdate.Price = product.Price;
            productToUpdate.ProductName = product.ProductName;
            productToUpdate.Quantity = product.Quantity;
            productToUpdate.CreatedDate = System.DateTime.Now;
            inventoryContext.SaveChanges();
            return NoContent();
        }
    }
}