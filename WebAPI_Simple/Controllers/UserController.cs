using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Simple.Models.DTO;
using WebAPI_Simple.Repositories;

namespace WebAPI_Simple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser>? _userManager;
        private readonly ItokenRepository _tokenRepositoty;

        public UserController (UserManager<IdentityUser>? userManager, ItokenRepository tokenRepositoty)
        {
            _userManager = userManager;
            _tokenRepositoty = tokenRepositoty;
        }

        // POST:/api/auth/register  - Chức năng đăng kí cho user
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO) // khai báo kiểu model cho register
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDTO.UserName,
                Email = registerRequestDTO.UserName
            };
            var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDTO.Password);
            if (identityResult.Succeeded)
            {
                // add role to this user
                if (registerRequestDTO.Roles != null && registerRequestDTO.Roles.Any())
                {
                    identityResult = await _userManager.AddToRoleAsync(identityUser, registerRequestDTO.Roles.ToString());
                }
                if (identityResult.Succeeded)
                {
                    return Ok("Register Successful! Let login!");
                }
            }
            return BadRequest("Something wrong!");
        } // end action register

        //POST: /api/auth/login -Chức năng đăng nhập cho user
        [HttpPost]
        [Route ("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        //Khai bao model cho login
        {
            var user = await _userManager.FindByEmailAsync (loginRequestDTO.Username);
            if (user != null)
            {
                var checkPassworkResult = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password); // kiểm tra passwork nhập vào khớp với passwork nhập vào database
                if (checkPassworkResult)
                {
                    //get roles for this user - lấy quyền của user từ database
                     var roles = await _userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        // Create token - tạo token cho user này
                        var jwtToken = _tokenRepositoty.CreateJWTToken(user, roles.ToList());
                        var response = new LoginResponseDTO
                        {
                            JwtToken = jwtToken
                        };
                        return Ok(response); //trả về chuỗi token
                    }
                }
            }
            return BadRequest("User or password incorrect");
        } // end action login
    }// end class user controller
}
