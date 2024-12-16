namespace LeakyPlayEntities
{
   /// <summary>
   /// Room that is basiacally a place for users to assosiate single shared playlist with.
   /// </summary>
   public class Room
   {
      public long RoomId { get; init; }
      public required string RoomName { get; init; }
      public required string CombinedPlaylistLink { get; init; }
      public required string CombinedPlaylistName { get; init; }

   }
}