using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FreshmanCSForum.API.Data;
using FreshmanCSForum.API.Data.Interfaces;
using FreshmanCSForum.API.Dtos;
using FreshmanCSForum.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace FreshmanCSForum.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CodeLabsController : ControllerBase
  {
    private readonly IMapper _mapper;
    private readonly ICodeLabsService _codeLabsService;

    public CodeLabsController(IMapper mapper, ICodeLabsService codeLabsService)
    {
      _mapper = mapper;
      _codeLabsService = codeLabsService;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CodeLab>> Update(string id, [FromBody] CodeLabForUpdateDto codeLabForUpdateDto)
    {
      CodeLab codeLabChanges = _mapper.Map<CodeLab>(codeLabForUpdateDto);
      CodeLab updatedCodeLab = await _codeLabsService.Update(id, codeLabChanges);
      return Ok(updatedCodeLab);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CodeLabForCreateDto codeLabForCreateDto)
    {
      string currUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      CodeLab codeLabToCreate = _mapper.Map<CodeLab>(codeLabForCreateDto);
      codeLabToCreate.LeadId = currUserId;
      codeLabToCreate.MemberIds = new List<string> { currUserId };
      codeLabToCreate.Applications = new List<Application> { };
      codeLabToCreate.Chat = new Chat();
      await _codeLabsService.Create(codeLabToCreate);
      return Ok(codeLabToCreate);
    }

    [HttpPut("{id:length(24)}/apply")]
    public async Task<IActionResult> Apply(string id, Application application)
    {
      application.Status = "applied";
      CodeLabForReturnDto codeLabWithNewApplicant = await _codeLabsService.AddApplicant(id, application);
      return Ok(codeLabWithNewApplicant);
    }

    [HttpPut("{id:length(24)}/acceptApplication/{applicantId:length(24)}")]
    public async Task<IActionResult> AcceptApplication(string id, string applicantId)
    {
      string currUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      CodeLabForReturnDto codeLabWithNewApplicant;
      try
      {
        codeLabWithNewApplicant = await _codeLabsService.AcceptApplication(id, applicantId, currUserId);
      }
      catch (System.UnauthorizedAccessException e)
      {
        return StatusCode(401);
      }
      return Ok(codeLabWithNewApplicant);
    }


    [HttpPut("{id:length(24)}/rejectApplication/{applicantId:length(24)}")]
    public async Task<IActionResult> RejectApplication(string id, string applicantId)
    {
      string currUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      CodeLabForReturnDto codeLabWithNewApplicant;
      try
      {
        codeLabWithNewApplicant = await _codeLabsService.RejectApplication(id, applicantId, currUserId);
      }
      catch (System.UnauthorizedAccessException e)
      {
        return StatusCode(401);
      }
      return Ok(codeLabWithNewApplicant);
    }

    [HttpGet("{id:length(24)}")]
    public async Task<IActionResult> Get(string id)
    {
      CodeLabForReturnDto codeLab = await _codeLabsService.GetOne(id);
      if (codeLab == null) return NotFound();
      return Ok(codeLab);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
      var codeLabs = await _codeLabsService.GetAll();
      return Ok(codeLabs);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
      await _codeLabsService.Delete(id);
      return Ok();
    }
  }
}