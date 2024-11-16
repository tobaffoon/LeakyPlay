namespace LeakyPlayTgBotNet.RoomsService;

public interface IRoomsManagerService
{
   /// <summary>
   /// Tries to create a room.
   /// </summary>
   /// <returns>True if new room was created. False if it was present.</returns>
   bool TryCreateRoom(long id, out RoomService room);
   /// <summary>
   /// Tries to delete the room.
   /// </summary>
   /// <returns>True if the room was deleted. False if it was not present.</returns>
   bool TryDeleteRoom(long id);
   /// <summary>
   /// Tries to get a room by id.
   /// </summary>
   /// <returns>True if the room exists. False otherwise.</returns>
   bool TryGetRoom(long id, out RoomService? room);
}
