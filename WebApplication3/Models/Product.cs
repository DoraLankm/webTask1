namespace WebApplication3.Models
{
    public class Product: ProductBase
    {
        public int Cost { get; set; }

        public int? CategoryId { get; set; }

        public virtual Category? Category { get; set; }
        
        public virtual List <Storage> Storages { get; set; } = new List<Storage>();
        public Product ()
        {

        }


    }
}
