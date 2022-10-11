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
    [Route("[controller]")]
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

        [HttpPost()]
        public async Task<Guid> Create(MatchModel model, CancellationToken cancellationToken)
        {
            return await matchService.Create(Mapper.Map<MatchEntity>(model), cancellationToken);
        }

        [HttpPost("{id:Guid}/games")]
        public async Task<Guid> CreateGame([FromRoute] Guid id, GameModel model, CancellationToken cancellationToken)
        {
            return await gameService.Create(Mapper.Map<GameEntity>(model), cancellationToken);
        }

        [HttpPost("{id:Guid}/games/{gameId:Guid}")]
        public async Task UpdateGame([FromRoute] Guid id, [FromRoute] Guid gameId, GameModel model, CancellationToken cancellationToken)
        {
            await gameService.Update(Mapper.Map<GameEntity>(model), cancellationToken);
        }
    }
}
