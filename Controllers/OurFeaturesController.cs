using KhadeFarm_Web_API.Common;
using KhadeFarm_Web_API.DBContexts;
using KhadeFarm_Web_API.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KhadeFarm_Web_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OurFeaturesController : ControllerBase
    {
        private readonly IGenericRepository<OurFeatures> _repository;

        public OurFeaturesController(IGenericRepository<OurFeatures> repository)
        {
            _repository = repository;
        }

        [HttpGet("GetAllFeatures", Name = "GetAllFeatures")]
        public async Task<IEnumerable<OurFeatures>> GetAll()
        {
            try
            {
                var featurs = await _repository.GetAllAsync();
                return featurs;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetFeatureByID/{id}", Name = "GetFeatureByID")]
        public async Task<OurFeatures> GetByID(int id)
        {
            try
            {
                var feature = await _repository.GetByIdAsync(id);
                return feature;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [Authorize(Roles = "User")]
        [HttpPost(Name = "AddFeature")]
        public async Task<IActionResult> AddFeature([FromBody] OurFeatures features)
        {
            if (GlobalMethods.IsNullOrEmpty(features.ImgUrl) || GlobalMethods.IsNullOrEmpty(features.Title) || GlobalMethods.IsNullOrEmpty(features.Description) || GlobalMethods.IsNullOrEmpty(features.RedirectUrl))
            {
                return BadRequest(new { message = "All fields are required" });
            }

            await _repository.AddAsync(features); // async insert

            return Ok(new { message = "features added successfully" });
        }
    }
}