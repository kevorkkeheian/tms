using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Application.Server.Controllers;


[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public LoginController(IConfiguration configuration,
                           SignInManager<ApplicationUser> signInManager,
                           UserManager<ApplicationUser> userManager)
    {
        _configuration = configuration;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] ClientLoginModel login)
    {
        var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);

        if (!result.Succeeded) return BadRequest(new LoginResult { Successful = false, Error = "Username and password are invalid." });

        var user = await _signInManager.UserManager.FindByNameAsync(login.Email);
        var roles = await _signInManager.UserManager.GetRolesAsync(user);

        var claims = new List<Claim>();

        claims.Add(new Claim(ClaimTypes.Name, login.Email));
        // add user id
        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtExpiryInDays"]));

        var token = new JwtSecurityToken(
            _configuration["JwtIssuer"],
            _configuration["JwtAudience"],
            claims,
            expires: expiry,
            signingCredentials: creds
        );

        return Ok(new LoginResult { Successful = true, Token = $"Bearer {new JwtSecurityTokenHandler().WriteToken(token)}" });
    }
}