using Core.Common;

namespace DataAccessLayer.Models
{
    public class Bar : EfBaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
