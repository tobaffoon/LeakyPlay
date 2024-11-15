namespace LeakyPlayTgBotNet.Rooms;

public interface IRoomsService
{
   bool CreateRoom(long id, out Room? room);
   bool DeleteRoom(long id);
   bool TryGetRoom(long id, out Room? room);
}
