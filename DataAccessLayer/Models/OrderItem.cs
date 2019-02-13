using Core.Common;
using Infrastructure;
using System;

namespace DataAccessLayer.Models
{
    public class OrderItem : EfBaseModel
    {
        public EnumOrderItemType OrderItemType { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public virtual Product Product { get; set; }
    }
}
