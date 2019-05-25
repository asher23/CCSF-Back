using AutoMapper;
using AutoMapper.Configuration;
using FreshmanCSForum.API.Dtos;
using FreshmanCSForum.API.Models;
using System.Collections.Generic;
using System.Linq;

namespace FreshmanCSForum.API.Helpers
{
  public class AutoMapperProfiles : Profile
  {
    public AutoMapperProfiles()
    {
      CreateMap<UserForRegisterDto, User>();
      CreateMap<GuideForUpdateAndRegisterDto, Guide>()
      .ForMember(dest => dest.Sections, opt =>
        {
          opt.MapFrom(src => src.Sections);
        });
      CreateMap<GuideForRegisterDto, Guide>();
      CreateMap<SectionForCreateDto, Section>();
      CreateMap<CodeLabForCreateDto, CodeLab>();
      CreateMap<CommentForCreateDto, Comment>();
      CreateMap<UserForUpdateDto, User>();
      CreateMap<User, UserForReturnDto>();
      CreateMap<Comment, CommentForReturnDto>();

    }
  }
}