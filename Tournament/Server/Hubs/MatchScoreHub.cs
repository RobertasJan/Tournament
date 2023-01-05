using Microsoft.AspNetCore.SignalR;
using Tournament.Domain.Games;
using Tournament.Server.Models;
using Tournament.Shared.Games;

namespace Tournament.Server.Hubs
{
    public class MatchScoreHub : Hub
    {
        public Task AddToGroup(string matchId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, matchId);
        }
        public async Task UpdateMatchScore(List<GameModel> games, string matchId)
        {
            await Clients.Group(matchId).SendAsync("UpdateMatchScore", games);
        }
    }
}
