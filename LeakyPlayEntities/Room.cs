namespace LeakyPlayEntities;

public class Room
{
   public long RoomId { get; set; }
   public required string RoomName { get; set; }
   public required string CommonPlaylistLink { get; set; }
   public required string CommonPlaylistName { get; set; }

}
