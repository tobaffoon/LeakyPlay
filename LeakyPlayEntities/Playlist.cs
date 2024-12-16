namespace LeakyPlayEntities
{
   /// <summary>
   /// Playlist added to a room.
   /// </summary>
   public class Playlist
   {
      public long CreatorId { get; init; }
      public required string Name { get; init; }
      public required string Link { get; init; }
   }
}