using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pcman.Controllers;
using pcman.DTO;
using pcman.Models;
using pcman.Services;

namespace pcman.Controllers
{
    public class AccountController : DefaultController
    {
        private readonly UserManager<EntityUser> _userManager;
        private readonly TokenService _tokenService;
        private readonly SignInManager<EntityUser> _signInManager;
        public AccountController(
            UserManager<EntityUser> userManager,
            SignInManager<EntityUser> signInManager,
            TokenService tokenService
        )
        {
            _signInManager = signInManager;
            _tokenService = tokenService;
            _userManager = userManager;
        }
        /// <summary>
        /// POST api/account/register
        /// </summary>
        /// <param name="registerDTO"></param>
        /// <returns><see cref="UserAuthDTO" /></returns>
        [HttpPost("register")]
        public async Task<ActionResult<UserAuthDTO>> RegisterUser(RegisterDTO registerDTO)
        {
            var user = new EntityUser
            {
                Name = registerDTO.Name.Trim(),
                UserName = registerDTO.Email.ToLower().Trim(),
                Email = registerDTO.Email.ToLower().Trim(),
                EduId = registerDTO.EduId.ToLower().Trim()
            };
            var result = await _userManager.CreateAsync(user, password: registerDTO.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return UserToDto(user);
        }
        /// <summary>
        /// POST api/account/login
        /// </summary>
        /// <param name="loginDTO"></param>
        /// <returns><see cref="UserAuthDTO" /></returns>
        [HttpPost("login")]
        public async Task<ActionResult<UserAuthDTO>> LoginUser(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email.ToLower().Trim());
            // Return If user was not found
            if (user == null) return BadRequest("Invalid Email");

            var result = await _signInManager.CheckPasswordSignInAsync(user, password: loginDTO.Password, false);
            if (result.Succeeded)
                return UserToDto(user);
            return BadRequest("Invalid Password");
        }
        /// <summary>
        /// POST api/account/refresh
        /// </summary>
        /// <param name="supplierAuthDTO"></param>
        /// <returns><see cref="UserAuthDTO" /></returns>
        [Authorize]
        [HttpPost("refresh")]
        public async Task<ActionResult<UserAuthDTO>> RefreshToken(UserAuthDTO supplierAuthDTO)
        {
            var user = await _userManager.FindByIdAsync(supplierAuthDTO.Id);
            // Return If user was not found
            if (user == null) return BadRequest("Invalid User");
            return UserToDto(user);
        }

        /// <summary>
        /// Utility Method.
        /// Converts a WhotUser to an AuthUserDto
        /// </summary>
        /// <param name="user"></param>
        /// <returns><see cref="UserAuthDTO" /></returns>
        private UserAuthDTO UserToDto(EntityUser user)
        {
            return new UserAuthDTO
            {
                Name = user.Name,
                Token = _tokenService.GenerateToken(user),
                Id = user.Id
            };
        }
    }
}