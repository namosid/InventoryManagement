using Inventory.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InventoryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private InventoryContext inventoryContext;
        public UserController(InventoryContext inventory)
        {
            inventoryContext = inventory;
        }

        [HttpGet]
        public IActionResult GetUser(string loginId, string password)
        {
            var status = inventoryContext.GetLoginUser(loginId, password);
            if (status.Equals("Success"))
            {
                return Ok(JsonConvert.SerializeObject("Login Successful"));
            }
            else
            {
                return NotFound("User Not Found. Please enter correct credentials");
            }
        }
    }
}