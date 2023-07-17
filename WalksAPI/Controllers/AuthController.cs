﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WalksAPI.CustomActionFilters;
using WalksAPI.Models.DTO;

namespace WalksAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly UserManager<IdentityUser> userManager;

        public AuthController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }
        // POST /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        [ValidateModel]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto) {
            var identityUser = new IdentityUser {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };
            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);
            if (!identityResult.Succeeded) {
                return BadRequest("Something went wrong");
            }
            if(registerRequestDto.Roles == null || !registerRequestDto.Roles.Any()) {
                return BadRequest("Something went wrong, you didnt provide role");
            }

            identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
            if (!identityResult.Succeeded) {
                return BadRequest("Something went wrong, couldnt asign roles to the user manager");
            }
            return Ok("User was registered, please log in");
        }
        
        // POST /api/Auth/Login
        [HttpPost]
        [Route("Login")]
        [ValidateModel]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto) {
            var identityUser = await userManager.FindByEmailAsync(loginRequestDto.Username);
            if (identityUser == null) {
                return BadRequest("Invalid credentials");
            }
            var passwordValid = await userManager.CheckPasswordAsync(identityUser, loginRequestDto.Password);
            if (!passwordValid) {
                return BadRequest("Invalid credentials");
            }
            // here we will create token in the next lecture
            return Ok();
        }
    }
}
