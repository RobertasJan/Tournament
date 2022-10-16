using Tournament.Shared.Tournaments;

namespace Tournament.Client.Models
{
    public class RegisterViewModel
    {
        public string PlayerName { get; set; }
        public IEnumerable<PartneredTournamentGroupModel> GroupsToRegister { get; set; }
    }

    public class PartneredTournamentGroupModel : TournamentGroupModel
    {
        public string PartnerName { get; set; }
    }
}
