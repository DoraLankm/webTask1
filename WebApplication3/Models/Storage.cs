namespace WebApplication3.Models
{
    public class Storage: ProductBase
    {
        public int ProductId { get; set; }  
        public virtual Product Product { get; set; }  

        public int Count { get; set; }
    }
}
