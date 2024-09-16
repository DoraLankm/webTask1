namespace WebApplication3.Models
{
    public class Category: ProductBase
    {
        public virtual List<Product> Products { get; set; } = new List<Product>();
    }
}
