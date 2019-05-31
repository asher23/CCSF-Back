using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FreshmanCSForum.API.Data;
using FreshmanCSForum.API.Data.Interfaces;
using FreshmanCSForum.API.Dtos;
using FreshmanCSForum.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FreshmanCSForum.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  public class UsersController : ControllerBase
  {
    private readonly IMapper _mapper;
    private readonly IUsersService _usersService;
    private readonly ICommentsService _commentsService;
    private readonly IConfiguration _config;
    private Cloudinary _cloudinary;

    public UsersController(IMapper mapper, IUsersService usersService, ICommentsService commentsService, IConfiguration config)
    {
      _mapper = mapper;
      _usersService = usersService;
      _commentsService = commentsService;
      _config = config;

      Account acc = new Account(
      _config.GetSection("CloudinarySettings:CloudName").Value,
      _config.GetSection("CloudinarySettings:ApiKey").Value,
      _config.GetSection("CloudinarySettings:ApiSecret").Value
    );

      _cloudinary = new Cloudinary(acc);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] UserForUpdateDto UserForUpdateDto)
    {
      if (id != User.FindFirst(ClaimTypes.NameIdentifier).Value)
        return Unauthorized();

      PhotoForCreationDto photo = UserForUpdateDto.Photo;
      if (photo != null && photo.File != null)
      {
        var file = photo.File;
        var uploadResult = new ImageUploadResult();
        if (file.Length > 0)
        {
          var uploadParams = new ImageUploadParams()
          {
            File = new FileDescription(file)
          };
          uploadResult = _cloudinary.Upload(uploadParams);
        }
        photo.Url = uploadResult.Uri.ToString();
        photo.PublicId = uploadResult.PublicId;
      }

      User user = _mapper.Map<User>(UserForUpdateDto);
      User updatedUser = await _usersService.Update(id, user);
      UserForReturnDto updatedUserToReturn = _mapper.Map<UserForReturnDto>(updatedUser);
      return Ok(updatedUserToReturn);

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
      if (id != User.FindFirst(ClaimTypes.NameIdentifier).Value)
        return Unauthorized();

      await _usersService.Delete(id);

      return Ok();
    }

    [AllowAnonymous]
    [HttpPut("getList")]
    public async Task<IActionResult> GetList([FromBody] List<string> userIds)
    {
      IEnumerable<User> users = await _usersService.GetList(userIds);
      IEnumerable<UserForReturnDto> usersToReturn = _mapper.Map<UserForReturnDto[]>(users);
      return Ok(usersToReturn);
    }

    [AllowAnonymous]
    [HttpGet("{id:length(24)}")]
    public async Task<IActionResult> Get(string id)
    {
      User user = await _usersService.GetOne(id);
      if (user == null) return NotFound();

      UserForReturnDto userToReturn = _mapper.Map<UserForReturnDto>(user);
      return Ok(userToReturn);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
      IEnumerable<User> users = await _usersService.GetAll();
      IEnumerable<UserForReturnDto> usersToReturn = _mapper.Map<UserForReturnDto[]>(users);
      return Ok(usersToReturn);
    }


    [HttpGet("{id:length(24)}/comments")]
    public async Task<IActionResult> GetComments(string id)
    {
      IEnumerable<Comment> comments = await _commentsService.GetAllForPost(id);
      IEnumerable<CommentForReturnDto> commentsToReturn = _mapper.Map<CommentForReturnDto[]>(comments);
      return Ok(comments);
    }

  }
}