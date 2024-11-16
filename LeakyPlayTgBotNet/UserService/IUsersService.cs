using LeakyPlayEntities;

namespace LeakyPlayTgBotNet.UserService;

public interface IUsersService
{
   public bool TryGetUser(long id, out User? user);
}
