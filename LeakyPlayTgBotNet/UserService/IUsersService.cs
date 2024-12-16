

namespace LeakyPlayTgBotNet.UserService
{
   using LeakyPlayEntities;
   /// <summary>
   /// Service to get information about users in LeakyPlay tgbot.
   /// </summary>
   public interface IUsersService
   {
      public bool TryGetUser(long id, out User? user);
      /// <summary/>
      /// <returns>
      /// True if user was added. False if user is already registered.
      /// </returns>
      public bool RegisterUser(User user);
   }
}