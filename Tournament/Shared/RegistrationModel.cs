using System.ComponentModel.DataAnnotations;
using Tournament.Domain.Players;

namespace Tournament.Shared;
public class RegistrationModel : LoginModel
{
    public string ConfirmPassword { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Gender Gender { get; set; } = Gender.Male;
    public DateTime? BirthDate { get; set; }
}
