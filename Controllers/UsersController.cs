using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FreshmanCSForum.API.Data;
using FreshmanCSForum.API.Data.Interfaces;
using FreshmanCSForum.API.Dtos;
using FreshmanCSForum.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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


    public UsersController(IMapper mapper, IUsersService usersService, ICommentsService commentsService)
    {
      _mapper = mapper;
      _usersService = usersService;
      _commentsService = commentsService;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] UserForUpdateDto UserForUpdateDto)
    {
      if (id != User.FindFirst(ClaimTypes.NameIdentifier).Value)
        return Unauthorized();

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