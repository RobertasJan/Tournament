using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Tournament.Domain;
using Tournament.Domain.Games;
using Tournament.Domain.Players.Exceptions;
using Tournament.Domain.Services.Games;
using Tournament.Server.Hubs;
using Tournament.Server.Models;
using Tournament.Shared;
using Tournament.Shared.Games;

namespace Tournament.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchesController : BaseController
    {
        private readonly ILogger<MatchesController> _logger;
        private readonly IMatchService matchService;
        private readonly IGameService gameService;

        public MatchesController(ILogger<MatchesController> logger, IMatchService matchService, IGameService gameService)
        {
            _logger = logger;
            this.matchService = matchService;
            this.gameService = gameService;
        }

        [HttpGet("{id:Guid}")]
        public async Task<MatchModel> Get([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            return Mapper.Map<MatchModel>(await matchService.GetById(id, cancellationToken));
        }

        [HttpGet]
        public async Task<ICollection<MatchModel>> GetList([FromQuery] Guid tournamentGroupId, CancellationToken cancellationToken)
        {
            return Mapper.Map<ICollection<MatchModel>>(await matchService.Get(tournamentGroupId: tournamentGroupId, cancellationToken: cancellationToken));
        }

        [HttpGet("groups")]
        public async Task<ICollection<MatchesGroupModel>> GetGroupList([FromQuery] Guid tournamentGroupId, CancellationToken cancellationToken)
        {
            return Mapper.Map<ICollection<MatchesGroupModel>>(await matchService.GetGroups(tournamentGroupId, cancellationToken));
        }


        [HttpGet("{id:Guid}/games")]
        public async Task<ICollection<GameModel>> GetGames([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            return Mapper.Map<ICollection<GameModel>>(await gameService.Get(id, cancellationToken));
        }

        [HttpPost()]
        public async Task<ResponseModel<Guid>> Create(MatchModel model, CancellationToken cancellationToken)
        {
            try
            {
                var matchEntity = Mapper.Map<MatchEntity>(model);
                ValidateMatchModel(model);

                matchEntity.Player1Name = model.Player1Name;
                matchEntity.Player2Name = model.Player2Name;
                matchEntity.Player3Name = model.Player3Name;
                matchEntity.Player4Name = model.Player4Name;
                return new ResponseModel<Guid>(await matchService.Create(matchEntity, cancellationToken));
            } catch (APIException ex)
            {
                return new ResponseModel<Guid>(ex.ErrorCodeModel);
            }
        }

        private void ValidateMatchModel(MatchModel model)
        {
            if (model.Team1 is not null || model.Team2 is not null)
            {
                return;
            }

            if (string.IsNullOrEmpty(model.Player1Name) ||
                string.IsNullOrEmpty(model.Player3Name))
            {
                throw new NoFirstnameException();
            }

            if (model.Type != Domain.Games.MatchType.MensSingles && model.Type != Domain.Games.MatchType.WomensSingles)
            {
                if (string.IsNullOrEmpty(model.Player2Name) ||
                    string.IsNullOrEmpty(model.Player4Name))
                {
                    throw new NoFirstnameException();
                }
            }
        }

        [HttpPost("{id:Guid}/games")]
        public async Task<Guid> CreateGame([FromRoute] Guid id, GameModel model, CancellationToken cancellationToken)
        {
            model.MatchId = id;
            return await gameService.Create(Mapper.Map<GameEntity>(model), cancellationToken);
        }

        [HttpPut("{id:Guid}/games/{gameId:Guid}")]
        public async Task UpdateGame([FromRoute] Guid id, [FromRoute] Guid gameId, GameModel model, CancellationToken cancellationToken)
        {
            model.Id = gameId;
            model.MatchId = id;
            await gameService.Update(Mapper.Map<GameEntity>(model), cancellationToken);
        }

        [HttpDelete("{id:Guid}/games/{gameId:Guid}")]
        public async Task DeleteGame([FromRoute] Guid id, [FromRoute] Guid gameId, CancellationToken cancellationToken)
        {
            await gameService.Delete(gameId, cancellationToken);
        }

        [HttpPut("{id:Guid}")]
        public async Task Update([FromRoute] Guid id, UpdateMatchModel model, CancellationToken cancellationToken)
        {
            var match = await matchService.GetById(id, cancellationToken);
            match.ModifiedAt = DateTime.UtcNow;
            if (model.Result != MatchResult.Undetermined && match.MatchEnd is null)
            {
                match.MatchEnd = DateTime.UtcNow;
            }
            match.Record = model.Record;
            match.Result = model.Result;
            await matchService.Update(match, cancellationToken);
        }


        [HttpPut("{id:Guid}/setnextmatch")]
        public async Task SetNextMatch([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var match = await matchService.GetById(id, cancellationToken);
            await matchService.SetNextMatch(match);
            await matchService.SetByes(match.MatchesGroup.TournamentGroupId, cancellationToken);
        }
    }
}
