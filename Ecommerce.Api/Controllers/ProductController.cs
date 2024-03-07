using Ecommerce.Application.Services;
using Ecommerce.Context;
using Ecommerce.Dtos.Product;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
        private EcommerceContext _ecommerceContext;

        public ProductController(IProductService productService, ICategoryService categoryService, EcommerceContext ecommerceContext)
        {
            _productService = productService;
            _categoryService = categoryService;
            _ecommerceContext = ecommerceContext;

        }


        [HttpGet]
       // [Authorize]
        public async Task<IActionResult> GetAll()
        {
            try 
            { 

                var products = await (_productService.GetAllPagination(50, 1));
               if(products.Count > 0)
                
                    return Ok(products.Entities.ToList());
               else
                    return NoContent(); 

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //  [HttpGet]
        //[Route("{id}")]
        //public async Task<IActionResult>Get(int id)
        //{
        //    var product=await (_productService.GetOne(id));
        //    if (product == null)
        //        return BadRequest("The Id Not Found In the Database");

        //    else return Ok(product);
        //}

        [HttpGet("{id:int}", Name = "GetID")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var product = await _productService.GetOne(id);

                if (product == null)
                {
                    return NotFound($"Product with ID {id} not found in the database");
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }
        }

        //[HttpGet("{name:string}")]
        [HttpGet("{name}")] // the name can contain numbers and chars
        //[HttpGet("{name:regex(^[a-zA-Z0-9]+$)}")]
        public IActionResult GetByName(string name)
        {
            var product=_ecommerceContext.Products.FirstOrDefault(x => x.Name==name);
            if (product == null)
                return NotFound($"Product with Name {name} not found in the database");
            return Ok(product);
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateOrUpdateProductDTO product)
        {
            if (ModelState.IsValid)
            {
                var pro = await _productService.Create(product);

                if (pro == null)
                    return BadRequest("Error in Creation");

                else
                {
                    // create location name
                    var createdUrl = Url.Link("GetID", new { id = pro.Enttiy.Id });
                    return Created(createdUrl, pro);
                }
                
            }

            return BadRequest(ModelState);
        }


        [HttpPut]
        public async Task<IActionResult>Update(CreateOrUpdateProductDTO product)
        {
            if (ModelState.IsValid)
            {
                var pro = await _productService.Update(product);

                if (pro == null)
                    return BadRequest("Error in Creation");

                else
                {
                    // create location name
                    var createdUrl = Url.Link("GetID", new { id = pro.Enttiy.Id });
                    return Created(createdUrl, pro);
                }

            }

            return BadRequest(ModelState);
        }

        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult>Delete(int id)
        {
            var product = await _productService.GetOne(id);

            if (product.Enttiy == null)
            {
                return NotFound($"Product with ID {id} not found in the database");
            }
            else
            {
                var p=_productService.HardDelete(product.Enttiy);
                return Ok("Product Deleted sucessfully");
            }

            
        }


    }
}
