using Core.Common;
using System;

namespace DataAccessLayer.Models
{
    public class ProductInventory : EfBaseModel
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public virtual Product Product { get; set; }
    }
}
