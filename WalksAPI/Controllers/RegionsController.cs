using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WalksAPI.CustomActionFilters;
using WalksAPI.Data;
using WalksAPI.Models.Domain;
using WalksAPI.Models.DTO;
using WalksAPI.Repositories;

namespace WalksAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper) {
            // dependency injection, we are passing dbContext as a parameter of ctor or method
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        // GET ALL REGIONS
        // GET: https://localhost:portnumber/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll() {
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
            var regionsDomain = await regionRepository.GetAllAsync();
            // instead we convert it to the DTO - data transfer object
            
            // mapping without automapper
            /*var regionsDTO = new List<RegionDto>();
            foreach (var region in regionsDomain) {
                regionsDTO.Add(new RegionDto {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl
                });
            }*/

            //mapping with automapper from regionsDomain to DTO
            var regionsDTO = mapper.Map<List<RegionDto>>(regionsDomain);
            return Ok(regionsDTO);
        }

        // GET SINGLE REGION (Get Region By ID)
        // GET: https://localhost:portnumber/api/regions/{id}
        [HttpGet("{id:Guid}")] // Guid is a data type
        public async Task<IActionResult> GetById([FromRoute] Guid id) {
            // first option
            //var regionDomain = _dbContext.Regions.Find(id);
            // second option using LINQ
            var regionDomain = await regionRepository.GetByIdAsync(id);
            if (regionDomain == null) {
                return NotFound();
            }
            var regionDTO = mapper.Map<RegionDto>(regionDomain);
            return Ok(regionDTO);
        }
        // POST To Create New Region
        // POST: https://localhost:portnumber/api/regions
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto) {
            // convert DTO to domain model
            var regionDomain = mapper.Map<Region>(addRegionRequestDto);

            // use domain model to create region
            regionDomain = await regionRepository.CreateAsync(regionDomain);

            // we send back, what was created
            var regionDto = mapper.Map<RegionDto>(regionDomain);

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto); // 201
        }
        // Update region
        // PUT: https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto) {

            // convert DTO do domain model
            var regionDomain = mapper.Map<Region>(updateRegionRequestDto);
            regionDomain = await regionRepository.UpdateAsync(id, regionDomain);
            if(regionDomain == null) {
                return NotFound();
            }
            // conert domain model to dto
            var regionDto = mapper.Map<RegionDto>(regionDomain);

            return Ok(regionDto); // we pass  back the updated data
        }
        // Delete Region
        // DELETE: https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) {
            // region domain model
            var regionDomain = await regionRepository.DeleteAsync(id);
            if(regionDomain == null) {  
                return NotFound();
            }

            var regionDto = mapper.Map<RegionDto>(regionDomain);
            return Ok(regionDto);
        }
    }

}
