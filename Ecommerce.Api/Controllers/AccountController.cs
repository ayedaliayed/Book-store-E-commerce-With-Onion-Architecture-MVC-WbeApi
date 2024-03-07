using Ecommerce.Context;
using Ecommerce.Dtos.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
       // private RoleManager<EcommerceUser> _roleManager;
        private readonly UserManager<EcommerceUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly EcommerceContext _ecommerceContext;

        public AccountController(UserManager<EcommerceUser> userManager,EcommerceContext ecommerceContext,IConfiguration configuration) 
        {
            _userManager= userManager;
           // _roleManager = roleManager;
           _ecommerceContext= ecommerceContext;
            _configuration = configuration;
        }



        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDTO.Email);

            if (existingUser != null)
            {
                return StatusCode(500,"User with this email already exists");
            }
            else
            {
                //password hashing
                var hashedPassword = HashPassword(registerDTO.Password);
                var newUser = new EcommerceUser()
                {
                    Email = registerDTO.Email,
                    UserName = registerDTO.Email, 
                    age=registerDTO.age,
                    Address= registerDTO.Address,
                    city=registerDTO.city,
                    PasswordHash = hashedPassword,
                    
                };

                 var result = await _userManager.CreateAsync(newUser, registerDTO.Password);
               // var result = await _userManager.CreateAsync(newUser);

                if (!result.Succeeded)
                    return StatusCode(500, result.Errors);
                
                //var r= _ecommerceContext.Roles.Select(s => s.Name).ToList();
                await _userManager.AddToRoleAsync(newUser, registerDTO.Role);
                return Ok("User created successfully");
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult>Loing(LoginDTO loginDTO)
        {
            var existingUser = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (existingUser == null) 
            {
                return Unauthorized("Not Found");
            }

            var checkpass=await _userManager.CheckPasswordAsync(existingUser, loginDTO.Password);

            if (!checkpass) 
                return Unauthorized("Incorrect Password");

            var userRoles = await _userManager.GetRolesAsync(existingUser);

            var Claims = new List<Claim>
            {
                
                new Claim(ClaimTypes.Name, existingUser.Email),
                new Claim(ClaimTypes.Email, existingUser.Email),
                new Claim(ClaimTypes.Country, existingUser.city),
                //new Claim(ClaimTypes.Role,userRoles[0])

            };
            // Add each role as a separate claim
            foreach (var role in userRoles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, role));
            }


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:Key"]));


            

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:Key"]));
            var token = new JwtSecurityToken(
               issuer: _configuration["jwt:Issuer"],
               audience: _configuration["jwt:Audiences"],
               expires: DateTime.Now.AddDays(1),
               claims: Claims,
               signingCredentials: new SigningCredentials(Key, SecurityAlgorithms.HmacSha256Signature)
               );
            var StringToken = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { StringToken, Expire = token.ValidTo });




            return Ok(new { token, Expire=token.ValidTo});






        }






    }
}

