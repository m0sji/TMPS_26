using FragranceComposer.Models;
namespace FragranceComposer.Interfaces;

public interface IUserSession
{
    UserRole CurrentRole {get;}
}