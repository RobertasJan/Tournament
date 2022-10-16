﻿using System.Reflection;
using Tournament.Client.Services;
using Tournament.Domain.Games;
using Tournament.Domain.Tournaments;
using Tournament.Shared.Games;
using Tournament.Shared.Tournaments;
using MatchType = Tournament.Domain.Games.MatchType;

namespace Tournament.Client.Models
{
    public class TournamentViewModel : BaseViewModel<TournamentModel>
    {

        private TournamentService service { get; set; }

        public TournamentViewModel(TournamentService service, TournamentModel tournamentModel = null)
        {
            if (tournamentModel != null)
            {
                this.Data = tournamentModel;
            }
            this.service = service;

        }

        public async Task<Guid> Save()
        {
            return await service.CreateTournament(this.Data);
        }
    }
}