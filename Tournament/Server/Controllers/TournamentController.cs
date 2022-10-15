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
    public class TournamentController : BaseController
    {
        private readonly ILogger<TournamentController> _logger;
        private readonly ITournamentService tournamentService;

        public TournamentController(ILogger<TournamentController> logger, ITournamentService tournamentService)
        {
            _logger = logger;
            this.tournamentService = tournamentService;
        }

        [HttpGet("{id:Guid}")]
        public async Task<TournamentModel> Get([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            return Mapper.Map<TournamentModel>(await tournamentService.GetById(id, cancellationToken));
        }

        [HttpPost()]
        public async Task<Guid> Create(TournamentModel model, CancellationToken cancellationToken)
        {
            return await tournamentService.Create(Mapper.Map<TournamentEntity>(model), cancellationToken);
        }
    }
}
