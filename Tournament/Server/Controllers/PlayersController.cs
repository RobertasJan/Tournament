using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tournament.Domain.Games;
using Tournament.Domain.Players;
using Tournament.Domain.Services.Games;
using Tournament.Domain.Services.Players;
using Tournament.Domain.User;
using Tournament.Server.Models;
using Tournament.Shared.Games;
using Tournament.Shared.Players;

namespace Tournament.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayersController : BaseController
    {
        private readonly ILogger<PlayersController> _logger;
        private readonly IPlayerService playerService;
        private readonly SignInManager<ApplicationUserEntity> userManager;
        public PlayersController(ILogger<PlayersController> logger, IPlayerService playerService, SignInManager<ApplicationUserEntity> userManager)
        {
            _logger = logger;
            this.playerService = playerService;
            this.userManager = userManager;
        }

        [HttpGet("id:string")]
        public async Task<PlayerModel> Get([FromRoute] string id, CancellationToken cancellationToken)
        {
            return Mapper.Map<PlayerModel>(await playerService.GetById(Guid.Parse(id), cancellationToken));
        }

        [HttpGet]
        public async Task<ICollection<PlayerModel>> Get([FromQuery] Guid? tournamentId, [FromQuery] string? searchText, [FromQuery] Gender? gender, CancellationToken cancellationToken)
        {
            return Mapper.Map<ICollection<PlayerModel>>(await playerService.Get(tournamentId: tournamentId, searchText: searchText, gender: gender));
        }
    }
}
