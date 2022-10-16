using Microsoft.AspNetCore.Mvc;
using Tournament.Domain.Games;
using Tournament.Domain.Services.Games;
using Tournament.Domain.Services.Tournament;
using Tournament.Domain.Tournaments;
using Tournament.Server.Models;
using Tournament.Shared.Games;
using Tournament.Shared.Tournaments;

namespace Tournament.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TournamentsController : BaseController
    {
        private readonly ILogger<TournamentsController> _logger;
        private readonly ITournamentService tournamentService;
        private readonly ITournamentGroupService tournamentGroupService;

        public TournamentsController(ILogger<TournamentsController> logger, ITournamentService tournamentService, ITournamentGroupService tournamentGroupService)
        {
            _logger = logger;
            this.tournamentService = tournamentService;
            this.tournamentGroupService = tournamentGroupService;
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

        [HttpPost()]
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
    }
}
