namespace LeakyPlayEntities
{
   /// <summary>
   /// Map a playlist to a room it has been added to.
   /// </summary>
   public class RoomPlaylistMap
   {
      public long RoomPlaylistMapId { get; set; }
      public long RoomId { get; set; }
      public long PlaylistId { get; set; }
   }
}