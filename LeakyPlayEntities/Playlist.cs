namespace LeakyPlayEntities;

public class Playlist
{
   public long PlaylistId { get; set; }
   public long CreatorId { get; set; }
   public required string Link { get; set; }
}
