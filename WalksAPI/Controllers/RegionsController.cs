using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WalksAPI.Data;
using WalksAPI.Models.Domain;
using WalksAPI.Models.DTO;

namespace WalksAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase {
        private readonly WalksDbContext _dbContext;
        public RegionsController(WalksDbContext dbContext) {
            // dependency injection, we are passing dbContext as a parameter of ctor or method
            _dbContext = dbContext;
        }
        // GET ALL REGIONS
        // GET: https://localhost:portnumber/api/regions
        [HttpGet]
        public IActionResult GetAll() {
            // hard coded way
            /*var regions = new List<Region> {
                new Region {
                    Id = Guid.NewGuid(),
                    Name = "South West",
                    Code = "SW",
                    RegionImageUrl = "https://images.pexels.com/photos/258118/pexels-photo-258118.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region {
                    Id = Guid.NewGuid(),
                    Name = "South East",
                    Code = "SE",
                    RegionImageUrl = "https://images.pexels.com/photos/3822230/pexels-photo-3822230.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                }
            };*/

            // gets a domain model - we dont want to send it to the user (security, coupling)
            var regionsDomain = _dbContext.Regions.ToList();
            // instead we convert it to the DTO - data transfer object
            var regionsDTO = new List<RegionDto>();
            foreach (var region in regionsDomain) {
                regionsDTO.Add(new RegionDto {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl
                });
            }

            return Ok(regionsDTO);
        }

        // GET SINGLE REGION (Get Region By ID)
        // GET: https://localhost:portnumber/api/regions/{id}
        [HttpGet("{id:Guid}")] // Guid is a data type
        public IActionResult GetById([FromRoute] Guid id) {
            // first option
            //var regionDomain = _dbContext.Regions.Find(id);
            // second option using LINQ
            var regionDomain = _dbContext.Regions.FirstOrDefault(r => r.Id == id);

            if (regionDomain == null) {
                return NotFound();
            }
            var regionDTO = new RegionDto {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl
            };
            return Ok(regionDTO);
        }
        // POST To Create New Region
        // POST: https://localhost:portnumber/api/regions
        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto) {
            // convert DTO to domain model
            var regionDomain = new Region {
                Id = Guid.NewGuid(),
                Name = addRegionRequestDto.Name,
                Code = addRegionRequestDto.Code,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            // use domain model to create region
            _dbContext.Regions.Add(regionDomain); // tracking changes
            _dbContext.SaveChanges(); // commit changes to the database

            // we send back, what was created
            var regionDto = new RegionDto {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl
            };
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto); // 201
        }
        // Update region
        // PUT: https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto) {
            // region domain model
            var regionDomain = _dbContext.Regions.FirstOrDefault(r => r.Id == id);
            if (regionDomain == null) {
                return NotFound();
            }
            // map DTO to domain model
            regionDomain.Code = updateRegionRequestDto.Code;
            regionDomain.Name = updateRegionRequestDto.Name;
            regionDomain.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            _dbContext.SaveChanges(); // commit changes to the database

            // conert domain model to dto
            var regionDto = new RegionDto {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl
            };
            return Ok(regionDto); // we pass  back the updated data
        }
        // Delete Region
        // DELETE: https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id) {
            // region domain model
            var regionDomain = _dbContext.Regions.FirstOrDefault(r => r.Id == id);
            if(regionDomain == null) {  
                return NotFound();
            }
            _dbContext.Regions.Remove(regionDomain);
            _dbContext.SaveChanges(); // commit changes to the database

            var regionDto = new RegionDto {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl
            };
            return Ok(regionDto);
        }
    }

}
