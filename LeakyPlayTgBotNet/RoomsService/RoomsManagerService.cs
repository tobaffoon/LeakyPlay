namespace LeakyPlayTgBotNet.RoomsService
{
   using LeakyPlayEntities;

   public class RoomsManagerService : IRoomsManagerService
   {
      private readonly Dictionary<long, IRoomService> _rooms = [];
      public bool TryCreateRoom(long id, string name, string combinedPlaylistName, out IRoomService room)
      {
         bool present = _rooms.TryGetValue(id, out IRoomService? presentRoom);
         if (present)
         {
            room = presentRoom!;
            return false;
         }

         _rooms[id] = new RoomService(new Room
         {
            RoomId = id,
            RoomName = name,
            CombinedPlaylistLink = "[get link here]",
            CombinedPlaylistName = combinedPlaylistName
         });
         room = _rooms[id];
         return true;
      }

      public bool TryDeleteRoom(long id)
      {
         return _rooms.Remove(id);
      }

      public bool TryGetRoom(long id, out IRoomService? room)
      {
         return _rooms.TryGetValue(id, out room);
      }
   }
}