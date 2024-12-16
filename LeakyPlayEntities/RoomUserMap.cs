namespace LeakyPlayEntities
{
   /// <summary>
   /// Map a user to the room they are in.
   /// </summary>
   public class RoomUserMap
   {
      public long RoomUserMapId { get; init; }
      public long RoomId { get; init; }
      public long UserId { get; init; }
   }
}