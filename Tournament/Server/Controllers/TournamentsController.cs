using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tournament.Client.Pages;
using Tournament.Domain.Games;
using Tournament.Domain.Players;
using Tournament.Domain.Services.Games;
using Tournament.Domain.Services.Players;
using Tournament.Domain.Services.Tournament;
using Tournament.Domain.Tournaments;
using Tournament.Server.Models;
using Tournament.Shared.Games;
using Tournament.Shared.Players;
using Tournament.Shared.Tournaments;

namespace Tournament.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TournamentsController : BaseController
    {
        private readonly ILogger<TournamentsController> _logger;
        private readonly ITournamentService tournamentService;
        private readonly ITournamentGroupService tournamentGroupService;
        private readonly IMatchService matchService;
        private readonly IPlayerService playerService;

        public TournamentsController(ILogger<TournamentsController> logger, ITournamentService tournamentService, ITournamentGroupService tournamentGroupService, IPlayerService playerService, IMatchService matchService)
        {
            _logger = logger;
            this.tournamentService = tournamentService;
            this.tournamentGroupService = tournamentGroupService;
            this.playerService = playerService;
            this.matchService = matchService;
        }

        [HttpGet("{id:Guid}")]
        public async Task<TournamentModel> Get([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            return Mapper.Map<TournamentModel>(await tournamentService.GetById(id, cancellationToken));
        }

        [HttpGet]
        public async Task<ICollection<TournamentModel>> Get(CancellationToken cancellationToken)
        {
            return Mapper.Map<ICollection<TournamentModel>>(await tournamentService.Get(cancellationToken));
        }

        [HttpPost]
        public async Task<Guid> Create(TournamentModel model, CancellationToken cancellationToken)
        {
            var id = await tournamentService.Create(Mapper.Map<TournamentEntity>(model), cancellationToken);
            var tournamentGroups = Mapper.Map<ICollection<TournamentGroupEntity>>(model.Groups);
            foreach (var group in tournamentGroups)
            {
                group.TournamentId = id;
                await tournamentGroupService.Create(group, cancellationToken);
            }
            return id;
        }

        [HttpPost("{id:Guid}/groups/{tournamentGroupId:Guid}/register")]
        public async Task CreateRegisteredPlayers([FromRoute] Guid id, [FromRoute] Guid tournamentGroupId, RegisteredPlayersModel model, CancellationToken cancellationToken)
        {
            var registeredPlayerEntity = Mapper.Map<RegisteredPlayersEntity>(model);
            registeredPlayerEntity.TournamentGroupId = tournamentGroupId;
            // registeredPlayerEntity.Id = id;
            await tournamentGroupService.AddRegistration(registeredPlayerEntity, cancellationToken);
        }

        [HttpGet("{id:Guid}/players")]
        public async Task<ICollection<RegisteredPlayersModel>> GetPlayers([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            return Mapper.Map<ICollection<RegisteredPlayersModel>>(await playerService.GetTournamentPlayers(id, null, cancellationToken));
        }

        [HttpGet("{id:Guid}/groups/{tournamentGroupId:Guid}/players")]
        public async Task<ICollection<RegisteredPlayersModel>> GetPlayers([FromRoute] Guid id, [FromRoute] Guid tournamentGroupId, CancellationToken cancellationToken)
        {
            return Mapper.Map<ICollection<RegisteredPlayersModel>>(await playerService.GetTournamentPlayers(id, tournamentGroupId, cancellationToken));
        }

        [HttpPost("{id:Guid}")]
        public async Task AddMatch([FromRoute] Guid id, MatchModel match, CancellationToken cancellationToken)
        {
            await tournamentService.AddMatch(Mapper.Map<MatchEntity>(match), cancellationToken);
        }

        [HttpPut("{id:Guid}/state")]
        public async Task SetState([FromRoute] Guid id, SetStateModel model, CancellationToken cancellationToken)
        {
            var tournament = await tournamentService.GetById(id, cancellationToken);
            if (model.State == TournamentState.Draws)
            {
                await tournamentService.StartDraws(id, cancellationToken);
            }
            else if (model.State == TournamentState.Ongoing)
            {
                await tournamentService.StartTournament(id, cancellationToken);
            }
            else if (model.State == TournamentState.Finished)
            {
                await tournamentService.StartTournament(id, cancellationToken);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        [HttpGet("{id:Guid}/matches")]
        public async Task<ICollection<MatchModel>> GetMatches([FromRoute] Guid id, bool? active, CancellationToken cancellationToken)
        {
            return Mapper.Map<ICollection<MatchModel>>(await matchService.Get(tournamentId: id, active: active, cancellationToken: cancellationToken));
        }
    }
}
