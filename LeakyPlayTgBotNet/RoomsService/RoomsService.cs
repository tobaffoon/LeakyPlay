namespace LeakyPlayTgBotNet.RoomsService;

public class RoomsService : IRoomsService
{
   private readonly Dictionary<long, Room> _rooms = [];
   public bool CreateRoom(long id, out Room? room)
   {
      bool present = _rooms.TryGetValue(id, out room);
      if (present)
      {
         return false;
      }

      room = new Room(id);
      _rooms[id] = room;
      return true;
   }

   public bool DeleteRoom(long id)
   {
      return _rooms.Remove(id);
   }

   public bool TryGetRoom(long id, out Room? room)
   {
      return _rooms.TryGetValue(id, out room);
   }
}
