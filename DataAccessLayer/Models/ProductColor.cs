using Core.Common;
using Infrastructure;
using System.Collections.Generic;

namespace DataAccessLayer.Models
{
    public class ProductColor : EfBaseModel
    {
        public EnumProductColor ColorType { get; set; }
        public string Description { get; set; }
        public virtual ICollection<ProductInventory> ProductInventories { get; set; }
    }
}
