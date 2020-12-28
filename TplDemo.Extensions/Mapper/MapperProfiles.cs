using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TplDemo.Model.DataModel;
using TplDemo.Model.ViewModel;

namespace TplDemo.Extensions.Mapper
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<ViewCreateUser, sysUserInfoEntity>().ReverseMap();
        }
    }
}