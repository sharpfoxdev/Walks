using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WalksAPI.Models.Domain;
using WalksAPI.Models.DTO;
using WalksAPI.Repositories;

namespace WalksAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }
        // CREATE - POST
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto) {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            // map DTO to domain model
            var walkDomain = mapper.Map<Walk>(addWalkRequestDto);
            walkDomain = await walkRepository.CreateAsync(walkDomain);
            return Ok(mapper.Map<WalkDto>(walkDomain));
        }

        // GET - ALL WALKS
        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var walksDomain = await walkRepository.GetAllAsync();
            
            // map to list of DTOs
            return Ok(mapper.Map<List<WalkDto>>(walksDomain));
            
        }
        // GET - BY ID
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) {
            var walkDomain = await walkRepository.GetByIdAsync(id);
            if (walkDomain == null) {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(walkDomain));
        }

        //UPDATE - BY ID
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto) {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var walkDomain = mapper.Map<Walk>(updateWalkRequestDto);
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);
            if(walkDomain == null) {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(walkDomain));
        }
        // DELETE
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) {
            var walkDomain = await walkRepository.DeleteAsync(id);
            if (walkDomain == null) {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(walkDomain));
        }

    }
}
