using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tournament.Client.Services;
using Tournament.Domain.Games;
using Tournament.Domain.Services.Games;
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
        public async Task<Guid> Create(MatchModel model, CancellationToken cancellationToken)
        {
            return await matchService.Create(Mapper.Map<MatchEntity>(model), cancellationToken);
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

        [HttpPut("{id:Guid}")]
        public async Task Update([FromRoute] Guid id, MatchModel model, CancellationToken cancellationToken)
        {
            model.Id = id;
            await matchService.Update(Mapper.Map<MatchEntity>(model), cancellationToken);
        }
    }
}
