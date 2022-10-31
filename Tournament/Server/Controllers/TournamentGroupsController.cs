using Microsoft.AspNetCore.Mvc;
using Tournament.Domain.Games;
using Tournament.Domain.Players;
using Tournament.Domain.Services.Players;
using Tournament.Domain.Services.Tournament;
using Tournament.Domain.Tournaments;
using Tournament.Server.Models;
using Tournament.Shared.Players;
using Tournament.Shared.Tournaments;

namespace Tournament.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TournamentGroupsController : BaseController
    {
        private readonly ILogger<TournamentGroupsController> _logger;
        private readonly ITournamentGroupService tournamentGroupService;

        public TournamentGroupsController(ILogger<TournamentGroupsController> logger, ITournamentGroupService tournamentGroupService)
        {
            _logger = logger;
            this.tournamentGroupService = tournamentGroupService;
        }

        [HttpGet("{id:Guid}")]
        public async Task<TournamentGroupModel> Get([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            return Mapper.Map<TournamentGroupModel>(await tournamentGroupService.GetById(id, cancellationToken));
        }
    }
}
