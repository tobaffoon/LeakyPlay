namespace LeakyPlayEntities;

public class Session
{
   public long UserId { get; init; }
   public State SessionState { get; set; }

   public enum State
   {
      Start,
      RoomEnter,
      MainRoom,
      DeletePlaylist,
      AddPlaylistConfirm,
      ChangeRoomConfirm
   }
}