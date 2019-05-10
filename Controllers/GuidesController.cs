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
  [Route("api/[controller]", Name = "Guides")]
  [ApiController]
  [Authorize]
  public class GuidesController : ControllerBase
  {
    private readonly IMapper _mapper;
    private readonly IGuidesService _guidesService;
    private readonly ICommentsService _commentsService;

    public GuidesController(IMapper mapper, IGuidesService guidesService, ICommentsService commentsService)
    {
      _mapper = mapper;
      _guidesService = guidesService;
      _commentsService = commentsService;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<string>> Update(string id, [FromBody] GuideForUpdateAndRegisterDto guideForUpdateAndRegisterDto)
    {
      Guide guide = _mapper.Map<Guide>(guideForUpdateAndRegisterDto);
      Guide updatedGuide = await _guidesService.Update(id, guide);
      return Ok(updatedGuide);
    }

    [HttpPost]
    public async Task<IActionResult> Create(GuideForUpdateAndRegisterDto guideForRegisterDto)
    {
      Guide guideToCreate = _mapper.Map<Guide>(guideForRegisterDto);
      await _guidesService.Create(guideToCreate);
      return CreatedAtRoute("GetGuide", new { id = guideToCreate.Id }, guideToCreate);
    }

    [HttpGet("{id:length(24)}", Name = "GetGuide")]
    // [AllowAnonymous]
    public async Task<IActionResult> Get(string id)
    {
      var guide = await _guidesService.GetOne(id);
      if (guide == null) return NotFound();
      return Ok(guide);
    }

    [HttpGet(Name = "GetGuides")]
    [AllowAnonymous]
    public async Task<IActionResult> Get()
    {
      var guides = await _guidesService.GetAll();
      return Ok(guides);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
      await _guidesService.Delete(id);
      return Ok();
    }
  }
}