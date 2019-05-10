using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using FreshmanCSForum.API.Dtos;
using FreshmanCSForum.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using FreshmanCSForum.API.Data;
using FreshmanCSForum.API.Data.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace FreshmanCSForum.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IAuthService _authService;
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;
    private readonly IUsersService _usersService;

    public AuthController(IAuthService authService, IConfiguration config, IMapper mapper, IUsersService usersService)
    {
      _authService = authService;
      _config = config;
      _mapper = mapper;
      _usersService = usersService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserForRegisterDto userForRegisterDto)
    {
      userForRegisterDto.UserName = userForRegisterDto.UserName.ToLower();
      if (await _authService.UserExists(userForRegisterDto.UserName)) return BadRequest("UserName already exits");
      var userToCreate = _mapper.Map<User>(userForRegisterDto);
      User createdUserName = await _authService.Register(userToCreate, userForRegisterDto.Password);
      return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
    {
      var userFromRepo = await _authService.Login(userForLoginDto.UserName.ToLower(), userForLoginDto.Password);

      if (userFromRepo == null) return Unauthorized();

      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
        new Claim(ClaimTypes.Name, userForLoginDto.UserName.ToString())
      };

      var identity = new ClaimsIdentity(claims, "Cookies");
      var principal = new ClaimsPrincipal(identity);
      var authProperties = new AuthenticationProperties();

      await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

      // var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

      // var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

      // var tokenDescriptor = new SecurityTokenDescriptor
      // {
      //   Subject = new ClaimsIdentity(claims),
      //   Expires = DateTime.Now.AddDays(1),
      //   SigningCredentials = creds
      // };

      // var tokenHandler = new JwtSecurityTokenHandler();

      // var token = tokenHandler.CreateToken(tokenDescriptor);


      // var checkthisout = User.FindFirst(ClaimTypes.NameIdentifier);

      // return Ok(new
      // {
      //   token = tokenHandler.WriteToken(token)
      // });
      return RedirectToRoute("IsAuthenticated");

    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
      await HttpContext.SignOutAsync();
      return Ok();
    }

    [HttpGet("isAuthenticated", Name = "IsAuthenticated")]
    [Authorize]
    public async Task<IActionResult> IsAuthenticated()
    {
      // if (id != User.FindFirst(ClaimTypes.NameIdentifier).Value)
      //   return Unauthorized();
      User user = await _usersService.GetOne(User.FindFirst(ClaimTypes.NameIdentifier).Value);
      if (user == null) return NotFound();

      UserForReturnDto userToReturn = _mapper.Map<UserForReturnDto>(user);
      return Ok(userToReturn);
    }
  }
}