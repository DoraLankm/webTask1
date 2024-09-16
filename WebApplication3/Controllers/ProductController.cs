using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetProducts()
        {
            try
            {
                using (var context = new ProductContext())
                {
                    var products = context.Products.Select(x => new Product()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description
                    });
                    return Ok(products);
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPut("putProduct")]
        public IActionResult PutProducts([FromQuery] string name, string description, int id, int cost)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    if (!context.Products.Any(x => x.Name.ToLower().Equals(name)))
                    {
                        context.Add(new Product()
                        {
                            Name = name,
                            Description = description,
                            Cost = cost,
                            CategoryId = id
                        });
                        context.SaveChanges();
                        return Ok(new Responce { Status = true, Message = "The product added successful" });
                    }
                    else
                    {
                        return StatusCode(409);
                    }

                }
            }
            catch
            {
                return StatusCode(500);
            }
        }


        [HttpPatch("updateProductPrice")]
        public IActionResult UpdateProductPrice([FromQuery] int id, [FromQuery] int newCost)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    var product = context.Products.Find(id);
                    if (product == null)
                    {
                        return NotFound(new Responce { Status = false, Message = "Product not found." });
                    }

                    product.Cost = newCost;
                    context.SaveChanges();

                    return Ok(new Responce { Status = true, Message = "Price updated successfull." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("deleteProduct")]
        public IActionResult DeleteProduct([FromQuery] int id)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    var product = context.Products.Find(id);
                    if (product == null)
                    {
                        return NotFound(new Responce { Status = false, Message = "Product did'nt found." });
                    }

                    context.Products.Remove(product);
                    context.SaveChanges();

                    return Ok(new Responce { Status = true, Message = "Product deleted successfull." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("deleteCategory")]
        public IActionResult DeleteCategory([FromQuery] int id)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    var category = context.Categories.Find(id);
                    if (category == null)
                    {
                        return NotFound(new Responce { Status = false, Message = "Category did'nt found." });
                    }

                    context.Categories.Remove(category);
                    context.SaveChanges();

                    return Ok(new Responce { Status = true, Message = "Category deleted successfull." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
    }
}


