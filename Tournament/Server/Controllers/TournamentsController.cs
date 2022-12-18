using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Tournament.Client.Pages;
using Tournament.Domain;
using Tournament.Domain.Errors;
using Tournament.Domain.Games;
using Tournament.Domain.Players;
using Tournament.Domain.Players.Exceptions;
using Tournament.Domain.Services.Games;
using Tournament.Domain.Services.Players;
using Tournament.Domain.Services.Tournament;
using Tournament.Domain.Tournaments;
using Tournament.Domain.Tournaments.Exceptions;
using Tournament.Server.Models;
using Tournament.Shared;
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
        public async Task<ResponseModel<ICollection<TournamentModel>>> Get(bool? finished = null, CancellationToken cancellationToken = default)
        {
            return new ResponseModel<ICollection<TournamentModel>>(Mapper.Map<ICollection<TournamentModel>>(await tournamentService.Get(finished, cancellationToken)));
        }

        [HttpPost]
        public async Task<ResponseModel<Guid>> Create(TournamentModel model, CancellationToken cancellationToken)
        {
            try
            {
                ValidateTournament(model);
                var id = await tournamentService.Create(Mapper.Map<TournamentEntity>(model), cancellationToken);
                var tournamentGroups = Mapper.Map<ICollection<TournamentGroupEntity>>(model.Groups);
                foreach (var group in tournamentGroups)
                {
                    group.TournamentId = id;
                    await tournamentGroupService.Create(group, cancellationToken);
                }

                return new ResponseModel<Guid>(id);
            } catch (APIException ex)
            {
                return new ResponseModel<Guid>(ex.ErrorCodeModel);
            }

        }

        private void ValidateTournament(TournamentModel tournament)
        {
            if (string.IsNullOrEmpty(tournament.Name))
            {
                throw new NoNameException();
            }

            if (string.IsNullOrEmpty(tournament.Description))
            {
                throw new NoDescriptionException();
            }

            if (string.IsNullOrEmpty(tournament.Address))
            {
                throw new NoAddressException();
            }

            if (string.IsNullOrEmpty(tournament.LongDescription))
            {
                throw new NoLongDescriptionException();
            }

            if (!tournament.StartDate.HasValue)
            {
                throw new InvalidDatesException();
            }

            if (!tournament.EndDate.HasValue)
            {
                throw new InvalidDatesException();
            }

            if (tournament.EndDate.Value < tournament.StartDate.Value)
            {
                throw new InvalidDatesException();
            }

            if (tournament.StartDate.Value < DateTime.Now.AddDays(-1))
            {
                throw new DatesInThePastException();
            }

            if (tournament.Groups is null || tournament.Groups.Count < 1)
            {
                throw new NoGroupsException();
            }
        }

        [HttpPost("{id:Guid}/groups/{tournamentGroupId:Guid}/register")]
        public async Task<ResponseModel> CreateRegisteredPlayers([FromRoute] Guid id, [FromRoute] Guid tournamentGroupId, RegisteredPlayersModel model, CancellationToken cancellationToken)
        {
            try
            {
                var registeredPlayerEntity = Mapper.Map<RegisteredPlayersEntity>(model);
                registeredPlayerEntity.TournamentGroupId = tournamentGroupId;
                // registeredPlayerEntity.Id = id;
                await tournamentGroupService.AddRegistration(registeredPlayerEntity, cancellationToken);
                return new ResponseModel();
            } catch (APIException ex)
            {
                return new ResponseModel<Guid>(ex.ErrorCodeModel);
            }
        }

        [HttpGet("{id:Guid}/players")]
        public async Task<ICollection<RegisteredPlayersModel>> GetPlayers([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            return Mapper.Map<ICollection<RegisteredPlayersModel>>(await playerService.GetTournamentPlayers(id, null, cancellationToken));
        }

        [HttpGet("{id:Guid}/players/aggregated")]
        public async Task<ICollection<TournamentPlayerModel>> GetPlayersAggregated([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            return await playerService.GetAggregatedTournamentPlayers(id, null, cancellationToken);
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
                await tournamentService.FinishTournament(id, cancellationToken);
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
