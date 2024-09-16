namespace WebApplication3.Models
{
    public class Product: ProductBase
    {
        public int Cost { get; set; }

        public int CategoryId { get; set; }

        public virtual Category? Category { get; set; }
        
        public virtual List <ProductStorage> ProductStorage { get; set; } = new List<ProductStorage>();
        public Product ()
        {

        }


    }
}
