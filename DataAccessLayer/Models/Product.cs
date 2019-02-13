using Core.Common;
using System.Collections.Generic;

namespace DataAccessLayer.Models
{
    public class Product : EfBaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<ProductInventory> ProductInventories { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
