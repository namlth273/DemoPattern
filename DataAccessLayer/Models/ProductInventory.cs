using Core.Common;
using System;

namespace DataAccessLayer.Models
{
    public class ProductInventory : EfBaseModel
    {
        public Guid ProductId { get; set; }
        public Guid ProductColorId { get; set; }
        public Guid ProductSizeId { get; set; }
        public int Quantity { get; set; }
        public double BuyPrice { get; set; }
        public double SellPrice { get; set; }
        public virtual Product Product { get; set; }
        public virtual ProductColor ProductColor { get; set; }
        public virtual ProductSize ProductSize { get; set; }
    }
}
