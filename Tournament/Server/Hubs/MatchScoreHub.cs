﻿using Microsoft.AspNetCore.SignalR;
using Tournament.Domain.Games;
using Tournament.Server.Models;
using Tournament.Shared.Games;

namespace Tournament.Server.Hubs
{
    public class MatchScoreHub : Hub
    {
        public async Task UpdateMatchScore(GameModel match)
        {
            await Clients.All.SendAsync("UpdateMatchScore", match);
        }
    }
}
