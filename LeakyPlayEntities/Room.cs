namespace LeakyPlayEntities;

public class Room
{
   public long RoomId { get; init; }
   public required string RoomName { get; init; }
   public required string CommonPlaylistLink { get; init; }
   public required string CommonPlaylistName { get; init; }

}
