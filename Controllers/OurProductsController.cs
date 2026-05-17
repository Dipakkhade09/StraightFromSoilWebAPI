using KhadeFarm_Web_API.Common;
using KhadeFarm_Web_API.DBContexts;
using KhadeFarm_Web_API.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KhadeFarm_Web_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OurProductsController : ControllerBase
    {
        private readonly IGenericRepository<OurProducts> _repository;

        public OurProductsController(IGenericRepository<OurProducts> repository)
        {
            _repository = repository;
        }


        [HttpGet("GetOurProducts", Name = "GetOurProducts")]
        public async Task<IEnumerable<OurProducts>> Get()
        {
            try
            {
                var products = await _repository.GetAllAsync();
                return products;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost(Name = "AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody] OurProducts Product)
        {
            if (GlobalMethods.IsNullOrEmpty(Product.ProductName) || Product.ProductOriginalPrice == 0 || Product.ProductDiscountPrice == 0 || GlobalMethods.IsNullOrEmpty(Product.ProductDescription))
            {
                return BadRequest(new { message = "All fields are required" });
            }

            await _repository.AddAsync(Product); // async insert

            return Ok(Product); // ✅ return actual product
        }
    }
}
