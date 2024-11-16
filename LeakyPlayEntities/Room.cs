namespace LeakyPlayEntities;

public class Room
{
   public long RoomId { get; init; }
   public required string RoomName { get; init; }
   public required string CombinedPlaylistLink { get; init; }
   public required string CombinedPlaylistName { get; init; }

}
