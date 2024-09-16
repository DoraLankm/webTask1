namespace WebApplication3.Models
{
    public class Storage: ProductBase
    {
        public virtual Product Product { get; set; }
        public int ProductId { get; set; }

        public virtual List<ProductStorage> ProductStorage { get; set; } = new List<ProductStorage>();
    }
}
