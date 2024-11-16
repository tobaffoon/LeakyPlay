namespace LeakyPlayEntities;

public class PlaylistArtistMap
{
   public long PlaylistArtistMapId { get; init; }
   public long PlaylistId { get; init; }
   public required string ArtistName { get; init; }
}
