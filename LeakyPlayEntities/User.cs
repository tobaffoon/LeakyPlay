namespace LeakyPlayEntities
{
   /// <summary>
   /// Abstract user that joins rooms and manages shared playlists.
   /// </summary>
   public class User
   {
      /// <summary>
      /// Relation between user and tgbot
      /// </summary>
      public enum State
      {
         Start,
         RoomEnter,
         MainRoom,
         DeletePlaylist,
         AddPlaylistConfirm,
         ChangeRoomConfirm
      }

      public long Id { get; init; }
      public string? Username { get; init; }
      public State SessionState { get; set; }
   }
}