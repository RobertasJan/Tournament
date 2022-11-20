using AutoMapper;
using Tournament.Domain.Games;
using Tournament.Domain.Players;
using Tournament.Domain.Results;
using Tournament.Domain.Tournaments;
using Tournament.Server.Models;
using Tournament.Shared.Games;
using Tournament.Shared.Players;
using Tournament.Shared.Results;
using Tournament.Shared.Tournaments;

namespace Tournament.Server
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap(typeof(MatchModel), typeof(MatchEntity)).ReverseMap();
            CreateMap(typeof(GameModel), typeof(GameEntity)).ReverseMap();
            CreateMap(typeof(MatchesGroupModel), typeof(MatchesGroupEntity)).ReverseMap();
            //  CreateMap(typeof(TournamentModel), typeof(TournamentEntity)).ReverseMap();
            CreateMap<TournamentGroupModel, TournamentGroupEntity>().ReverseMap();
            CreateMap<TournamentModel, TournamentEntity>().ReverseMap();
            CreateMap<PlayerModel, PlayerEntity>().ReverseMap();
            CreateMap<ResultEntity, ResultModel>()
                .ForMember(dto => dto.TournamentId, e => e.MapFrom(o => o.TournamentGroup.TournamentId))
                .ForMember(dto => dto.TournamentName, e => e.MapFrom(o => o.TournamentGroup.Tournament.Name));
            CreateMap<RegisteredPlayersEntity, RegisteredPlayersModel>()
                .ForMember(dto => dto.Player1Name, e => e.MapFrom(o => o.Player1.ShortenedFullName))
                .ForMember(dto => dto.Player2Name, e => e.MapFrom(o => o.Player2 != null ? o.Player2.ShortenedFullName : null));
            CreateMap<RegisteredPlayersModel, RegisteredPlayersEntity>();
        }
    }
}
