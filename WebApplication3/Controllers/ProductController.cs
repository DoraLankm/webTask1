using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WebApplication3.Abstaction;
using WebApplication3.Models.DTO;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Метод для возврата статистики кэша
        [HttpGet("GetCacheStats")]
        public ActionResult GetCacheStats()
        {
            var info = _productRepository.GetCacheStatistics();
            string text = $"Current Entry Count: {info.CurrentEntryCount}\n" +
                        $"Current Estimated Size: {info.CurrentEstimatedSize}\n" +
                        $"Total Hits: {info.TotalHits}\n" +
                        $"Total Misses: {info.TotalMisses}\n";
            string file = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "statistics.txt"); ;
            if (!System.IO.File.Exists(file))
            {
                return NotFound("Statistics file not found");
            }
            System.IO.File.WriteAllText(file, text);
            return PhysicalFile(file, "text/plain");
        }

        [HttpGet("getProducts")]
        public IActionResult GetProducts()
        {
            var products = _productRepository.GetProducts();
            return Ok(products);
        }

        [HttpPut("addProduct")]
        public IActionResult AddProduct([FromBody] ProductDTO productDTO)
        {
            var result = _productRepository.AddProduct(productDTO);
            return Ok(result);
        }

        [HttpGet("getCategory")]
        public IActionResult GetCategory()
        {
            var categories = _productRepository.GetCategories();
            return Ok(categories);
        }

        [HttpPut("addCategory")]
        public IActionResult AddCategory([FromBody] CategoryDTO categoryDTO)
        {
            var result = _productRepository.AddCategory(categoryDTO);
            return Ok(result);
        }

        // Метод для возврата товаров в формате CSV
        [HttpGet("GetProductsCsv")]
        public FileContentResult GetProductsCsv()
        {
            var products = _productRepository.GetProducts().ToList();
            var content = GetCsv(products);
            var fileContent = Encoding.UTF8.GetBytes(content);
            return File(fileContent, "text/csv", "products.csv");
        }

        private string GetCsv(IEnumerable<ProductDTO> products)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var product in products)
            {
                sb.AppendLine($"{product.Id};{product.Name};{product.Description}");
            }
            return sb.ToString();
        }
    }
}