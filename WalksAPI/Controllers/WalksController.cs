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
    }
}
