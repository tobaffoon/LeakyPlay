namespace LeakyPlayTgBotNet.UserService
{
   using LeakyPlayEntities;
   using LeakyPlayTgBotNet.RoomsService;

   public class UserService(IRoomsManagerService roomsService) : IUsersService
   {
      private readonly IRoomsManagerService _roomsService = roomsService;
      /// <summary>
      /// Id to session of each user.
      /// </summary>
      private readonly Dictionary<long, SessionController> _sessions = [];

      public bool RegisterUser(User user)
      {
         return _sessions.TryAdd(user.Id, new SessionController(user, _roomsService));
      }

      public bool TryGetUser(long id, out User? user)
      {
         bool result = _sessions.TryGetValue(id, out SessionController? session);
         if (session == null)
         {
            user = null;
         }
         else
         {
            user = session.user;
            user.SessionState = session.State;
         }
         return result;
      }
   }
}

