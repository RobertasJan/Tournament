﻿using Tournament.Domain.Games;
using Tournament.Domain.User;

namespace Tournament.Domain.Players
{
    public class PlayerEntity : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName { get
            {
                return FirstName + " " + LastName;
            } 
        }

        public Gender Gender { get; set; }
        public ICollection<PlayerMatchEntity> PlayerMatches { get; set; }
        public ICollection<RegisteredPlayersEntity> Registrations { get; set; }
        public string? UserId { get; set; }
        public ApplicationUserEntity? User { get; set; }
    }

    public enum Gender
    {
        Male = 1,
        Female = 2
    }

}
