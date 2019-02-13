using Core.Common;
using Infrastructure;

namespace DataAccessLayer.Models
{
    public class ProductSize : EfBaseModel
    {
        public EnumProductSize SizeType { get; set; }
        public string Description { get; set; }
        public virtual ProductInventory ProductInventory { get; set; }
    }
}
