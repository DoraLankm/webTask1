namespace WebApplication3.Models
{
    public partial class ProductStorage
    {
        public int? ProductId { get; set; }
        public int? StorageId { get; set; }

        public virtual Product? Product { get; set; }

        public virtual Storage? Storage { get; set; }
    }

}

