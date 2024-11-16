namespace LeakyPlayEntities;

public class PlaylistArtistMap
{
   public long PlaylistArtistMapId { get; set; }
   public long PlaylistId { get; set; }
   public required string ArtistName { get; set; }
}
