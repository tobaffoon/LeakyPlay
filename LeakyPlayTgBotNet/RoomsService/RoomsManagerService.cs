namespace LeakyPlayTgBotNet.RoomsService;

public class RoomsManagerService : IRoomsManagerService
{
   private readonly Dictionary<long, RoomService> _rooms = [];
   public bool TryCreateRoom(long id, out RoomService room)
   {
      bool present = _rooms.TryGetValue(id, out RoomService? presentRoom);
      if (present)
      {
         room = presentRoom!;
         return false;
      }

      room = new RoomService(id);
      _rooms[id] = room;
      return true;
   }

   public bool TryDeleteRoom(long id)
   {
      return _rooms.Remove(id);
   }

   public bool TryGetRoom(long id, out RoomService? room)
   {
      return _rooms.TryGetValue(id, out room);
   }
}
