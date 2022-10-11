using AutoMapper;
using Tournament.Domain.Games;
using Tournament.Server.Models;
using Tournament.Shared.Games;

namespace Tournament.Server
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap(typeof(MatchModel), typeof(MatchEntity)).ReverseMap();
            CreateMap(typeof(GameModel), typeof(GameEntity)).ReverseMap();
        }
    }
}
