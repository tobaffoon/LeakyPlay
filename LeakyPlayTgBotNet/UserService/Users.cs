namespace LeakyPlayTgBotNet.UserService
{
   public class Users
   {
      /// <summary>
      /// Id to session of each user.
      /// </summary>
      private readonly Dictionary<long, SessionController> _sessions = [];
   }
}