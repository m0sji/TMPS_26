using FragranceComposer.Interfaces;
using FragranceComposer.Models;
namespace FragranceComposer.Services;

public class UserSession : IUserSession
{
    public UserRole CurrentRole {get; private set;} = UserRole.Customer;

    public void SignIn(UserRole role)
    {
        CurrentRole = role;
    }
}