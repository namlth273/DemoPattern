using Core.Common;
using Infrastructure;

namespace DataAccessLayer.Models
{
    public class ProductColor : EfBaseModel
    {
        public EnumProductColor ColorType { get; set; }
        public string Description { get; set; }
        public virtual ProductInventory ProductInventory { get; set; }
    }
}
