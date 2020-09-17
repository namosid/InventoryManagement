using System;
using System.Collections.Generic;

namespace Inventory.Common
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
