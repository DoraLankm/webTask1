using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Caching.Memory;
using WebApplication3.Abstaction;
using WebApplication3.Models;
using WebApplication3.Models.DTO;

namespace WebApplication3.Repo
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        public ProductRepository(IMapper mapper, IMemoryCache memoryCache)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
        }
        public int AddCategory(CategoryDTO category)
        {
            
            using (var context = new ProductContext())
            {
                var group = context.Categories.FirstOrDefault(x => x.Name.ToLower() == category.Name.ToLower());
                if (group == null)
                {
                    group = _mapper.Map<Category>(category);
                    context.Categories.Add(group);
                    context.SaveChanges();
                    _memoryCache.Remove("categories");
                }
              
                
                return group.Id;
            }

            
        }

        public MemoryCacheStatistics GetCacheStatistics()
        {
            return _memoryCache.GetCurrentStatistics();
        }


        public int AddProduct(ProductDTO product)
        {
            using (var context = new ProductContext())
            {
                var product_entity = context.Products.FirstOrDefault(x => x.Name.ToLower() == product.Name.ToLower());
                if (product_entity == null)
                {
                    product_entity = _mapper.Map<Product>(product);
                    context.Products.Add(product_entity);
                    context.SaveChanges();
                    _memoryCache.Remove("products");
                }
                
                return product_entity.Id;
            }

            
        }

        public IEnumerable<CategoryDTO> GetCategories()
        {
            if (_memoryCache.TryGetValue("categories",out List<CategoryDTO> categories))
            {
                return categories;
            }
            using (var context = new ProductContext())
            {
                var categoriesList = context.Categories.Select(x => _mapper.Map<CategoryDTO>(x)).ToList();
                _memoryCache.Set("categories",categoriesList, TimeSpan.FromMinutes(30));
                return categoriesList;
            }

        }

        public IEnumerable<ProductDTO> GetProducts()
        {
            if (_memoryCache.TryGetValue("products", out List<ProductDTO> products))
            {
                return products;
            }
            using (var context = new ProductContext())
            {
                var productsList = context.Products.Select(x => _mapper.Map<ProductDTO>(x)).ToList();
                return productsList;
            }
        }
    }
}
