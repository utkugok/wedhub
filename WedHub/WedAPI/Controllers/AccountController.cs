using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Mono.TextTemplating;
using MySqlX.XDevAPI.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WedAPI.Data;
using WedAPI.Models;

namespace WedAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AccountController(UserManager<AppUser> userManger, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _userManger = userManger;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Register model)
    {
        var user = new AppUser
        {
            UserName = model.Username,
            Email = model.Email,
            State = (int)Enums.State.Active,
            CreatedDT = DateTime.Now,
            LastUpdatedDT = DateTime.Now
        };
        var result = await _userManger.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            return Ok(new { message = "User registered Succesfully" });
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Login model)
    {
        var user = await _userManger.FindByNameAsync(model.Username);

        if (user != null && await _userManger.CheckPasswordAsync(user, model.Password))
        {
            var userRoles = await _userManger.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"]!)),
                claims: authClaims,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)) , SecurityAlgorithms.HmacSha256));

            return Ok(new {Token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        return Unauthorized();
    }

    [HttpPost("add-role")]
    public async Task<IActionResult> AddRole([FromBody] string role)
    {
        if(!await _roleManager.RoleExistsAsync(role))
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(role));

            if(result.Succeeded)
            {
                return Ok(new { message = "Role Added successfully" });
            }

            return BadRequest(result.Errors);
        }

        return BadRequest("Role already Exists");
    }

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole([FromBody] UserRole model)
    {
        var user = await _userManger.FindByNameAsync(model.Username);

        if(user == null)
        {
            return BadRequest("User not found");
        }

        var role = await _roleManager.FindByNameAsync(model.Role);

        if (role == null)
        {
            return BadRequest("Role not found");
        }

        var result = await _userManger.AddToRoleAsync(user, model.Role);

        if(result.Succeeded)
        {
            return Ok(new { message = "Role assigned successfully" });
        }

        return BadRequest(result.Errors);
    }
}
