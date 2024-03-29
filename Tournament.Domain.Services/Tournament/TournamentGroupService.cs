﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Domain.Players;
using Tournament.Domain.Players.Exceptions;
using Tournament.Domain.Tournaments;
using Tournament.Infrastructure.Data;

namespace Tournament.Domain.Services.Tournament
{
    public interface ITournamentGroupService
    {
        Task Create(TournamentGroupEntity tournament, CancellationToken cancellationToken);
        Task<TournamentGroupEntity> GetById(Guid id, CancellationToken cancellationToken);
        Task AddRegistration(RegisteredPlayersEntity registeredPlayers, CancellationToken cancellationToken);
    }


    public class TournamentGroupService : ITournamentGroupService
    {
        private readonly AppDbContext _db;

        public TournamentGroupService(AppDbContext db)
        {
            this._db = db;
        }

        public async Task Create(TournamentGroupEntity tournamentGroup, CancellationToken cancellationToken)
        {
            await _db.TournamentGroups.AddAsync(tournamentGroup, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task AddRegistration(RegisteredPlayersEntity registeredPlayers, CancellationToken cancellationToken)
        {
            //check registered players
            if (_db.RegisteredPlayers.Any(x =>
            ((x.Player1Id == registeredPlayers.Player1Id || x.Player2Id == registeredPlayers.Player1Id)
            || (registeredPlayers.Player2Id != null && (x.Player1Id == registeredPlayers.Player2Id || x.Player2Id == registeredPlayers.Player2Id)))
            && x.TournamentGroupId == registeredPlayers.TournamentGroupId
            ))
            {
                throw new PlayerAlreadyRegisteredException();
            }
            await _db.RegisteredPlayers.AddAsync(registeredPlayers, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<TournamentGroupEntity> GetById(Guid id, CancellationToken cancellationToken)
        {
            var item = await _db.TournamentGroups.FirstOrDefaultAsync(x => x.Id == id, cancellationToken) ?? throw new Exception($"Tournament group {id} not found.");
            return item;
        }
    }
}

