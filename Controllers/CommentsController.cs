using System.Collections;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
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
  public class CommentsController : ControllerBase
  {
    private readonly IMapper _mapper;
    private readonly ICommentsService _commentsService;

    public CommentsController(IMapper mapper, ICommentsService commentsService)
    {
      _mapper = mapper;
      _commentsService = commentsService;
    }

    [HttpPost("posts/{postId}")]
    public async Task<IActionResult> Create(string postId, CommentForCreateDto commentForCreateDto)
    {
      Comment commentToCreate = _mapper.Map<Comment>(commentForCreateDto);
      commentToCreate.PostId = postId;
      commentToCreate.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      commentToCreate.Username = User.FindFirst(ClaimTypes.Name).Value;
      await _commentsService.Create(commentToCreate);
      return Ok();
    }

    [HttpGet("posts/{id:length(24)}")]
    public async Task<IActionResult> GetAllForPost(string postId)
    {
      IEnumerable comments = await _commentsService.GetAllForPost(postId);
      return Ok(comments);
    }

    [HttpGet("users/{id:length(24)}")]
    public async Task<IActionResult> GetAllForUser(string userId)
    {
      IEnumerable comments = await _commentsService.GetAllForUser(userId);
      return Ok(comments);
    }

    [HttpGet("{id:length(24)}")]
    public async Task<IActionResult> GetOne(string id)
    {
      var comment = await _commentsService.GetOne(id);
      if (comment == null) return NotFound();

      return Ok(comment);
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
      await _commentsService.Delete(id);
      return Ok();
    }


    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, CommentForCreateDto commentChanges)
    {
      Comment commentToUpdate = _mapper.Map<Comment>(commentChanges);
      Comment updatedComment = await _commentsService.Update(id, commentToUpdate);
      return Ok();
    }
  }
}