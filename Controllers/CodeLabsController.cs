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
    public async Task<ActionResult<CodeLab>> Update(string id, [FromBody] CodeLabForCreateDto codeLabForCreateDto)
    {
      CodeLab codeLabChanges = _mapper.Map<CodeLab>(codeLabForCreateDto);
      CodeLab updatedCodeLab = await _codeLabsService.Update(id, codeLabChanges);
      return Ok(updatedCodeLab);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CodeLabForCreateDto codeLabForCreateDto)
    {
      var codeLabToCreate = _mapper.Map<CodeLab>(codeLabForCreateDto);
      await _codeLabsService.Create(codeLabToCreate);
      return Ok(codeLabToCreate);
    }

    [HttpGet("{id:length(24)}")]
    public async Task<IActionResult> Get(string id)
    {
      var codeLab = await _codeLabsService.GetOne(id);
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