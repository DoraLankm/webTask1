using Microsoft.Extensions.Caching.Memory;
using WebApplication3.Models.DTO;

namespace WebApplication3.Abstaction
{
    public interface IProductRepository
    {
        public int AddProduct(ProductDTO product);

        public IEnumerable<CategoryDTO> GetCategories();

        public int AddCategory(CategoryDTO category);

        public IEnumerable<ProductDTO> GetProducts();

        public MemoryCacheStatistics GetCacheStatistics();
    }
}
