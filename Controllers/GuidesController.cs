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
  [Route("api/[controller]", Name = "Guides")]
  [ApiController]
  [Authorize]
  public class GuidesController : ControllerBase
  {
    private readonly IMapper _mapper;
    private readonly IGuidesService _guidesService;
    private readonly ICommentsService _commentsService;
    private readonly IConfiguration _config;
    private Cloudinary _cloudinary;

    public GuidesController(IMapper mapper, IGuidesService guidesService, ICommentsService commentsService, IConfiguration config)
    {
      _mapper = mapper;
      _guidesService = guidesService;
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
    public async Task<ActionResult<string>> Update(string id, [FromBody] GuideForUpdateAndRegisterDto guideForUpdateAndRegisterDto)
    {
      string currUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      string creatorId = await _guidesService.GetCreatorId(id);

      if (currUserId != creatorId) return StatusCode(401);

      foreach (PhotoForCreationDto photo in guideForUpdateAndRegisterDto.Photos)
      {
        if (photo.File != null)
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

      }
      foreach (SectionForCreateDto section in guideForUpdateAndRegisterDto.Sections)
      {
        foreach (PhotoForCreationDto photo in section.Photos)
        {
          if (photo.File != null)
          {
            var file = photo.File;
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
              var uploadParams = new ImageUploadParams()
              {
                File = new FileDescription(file)
                // Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
              };
              uploadResult = _cloudinary.Upload(uploadParams);
            }
            photo.Url = uploadResult.Uri.ToString();
            photo.PublicId = uploadResult.PublicId;
          }
        }
      }
      Guide guide = _mapper.Map<Guide>(guideForUpdateAndRegisterDto);
      Guide updatedGuide = await _guidesService.Update(id, guide);
      return Ok(updatedGuide);
    }

    [HttpPost]
    public async Task<IActionResult> Create(GuideForRegisterDto guideForRegisterDto)
    {
      guideForRegisterDto.CreatorId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      Guide guideToCreate = _mapper.Map<Guide>(guideForRegisterDto);
      guideToCreate.Sections = new List<Section>();
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

    [HttpGet("profile/{id:length(24)}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllForProfile(string id)
    {
      var guides = await _guidesService.GetAllForProfile(id);
      return Ok(guides);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
      string currUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      string creatorId = await _guidesService.GetCreatorId(id);

      if (currUserId != creatorId) return StatusCode(401);

      await _guidesService.Delete(id);
      return Ok();
    }
  }
}