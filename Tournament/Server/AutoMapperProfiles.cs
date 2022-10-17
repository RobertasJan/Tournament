using AutoMapper;
using Tournament.Domain.Games;
using Tournament.Domain.Players;
using Tournament.Domain.Tournaments;
using Tournament.Server.Models;
using Tournament.Shared.Games;
using Tournament.Shared.Players;
using Tournament.Shared.Tournaments;

namespace Tournament.Server
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap(typeof(MatchModel), typeof(MatchEntity)).ReverseMap();
            CreateMap(typeof(GameModel), typeof(GameEntity)).ReverseMap();
            //  CreateMap(typeof(TournamentModel), typeof(TournamentEntity)).ReverseMap();
            CreateMap<TournamentGroupModel, TournamentGroupEntity>().ReverseMap();
            CreateMap<TournamentModel, TournamentEntity>().ReverseMap();
            CreateMap<PlayerModel, PlayerEntity>().ReverseMap();
        }
    }
}
