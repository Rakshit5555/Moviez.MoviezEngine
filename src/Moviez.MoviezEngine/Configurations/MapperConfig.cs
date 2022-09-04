using System;
using AutoMapper;
using Moviez.MoviezEngine.Entities;
using Moviez.MoviezEngine.Models.RequestModels;
using Moviez.MoviezEngine.Models.ResponseModels;

namespace Moviez.MoviezEngine.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<ActorCreateRequest, Actor>().ReverseMap();
            CreateMap<MovieDetailReponseForUser, MovieDetailReponse>();
            CreateMap<BaseResponseModelForUser, BaseResponseModel>().ReverseMap();
        }
    }
}

