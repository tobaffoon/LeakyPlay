namespace LeakyPlayEntities
{
   /// <summary>
   /// Map a playlist to artists whose songs are in it.
   /// </summary>
   public class PlaylistArtistMap
   {
      public long PlaylistArtistMapId { get; init; }
      public long PlaylistId { get; init; }
      public required string ArtistName { get; init; }
   }
}