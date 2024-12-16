using LeakyPlayEntities;

namespace LeakyPlayTgBotNet.UserService;

/// <summary>
/// Service to get information about users in LeakyPlay tgbot.
/// </summary>
public interface IUsersService
{
   public bool TryGetUser(long id, out User? user);
}
