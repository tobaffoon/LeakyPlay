using LeakyPlayTgBotNet.RoomsService;
using Stateless;

namespace LeakyPlayTgBotNet.UserService;

/// <summary>
/// User's session with bot. By itself it doesn't reflect room state (added and generated playlists, users). Also room can change.
/// 
/// Commands that do not change the 'state' (/artists for example) are not represented as triggers in state machine.
/// </summary>
internal class Session
{
   public readonly long id;
   private readonly StateMachine<State, Trigger> _stateMachine = new StateMachine<State, Trigger>(State.Start);
   /// <summary>
   /// int param - roomId
   /// </summary>
   private readonly StateMachine<State, Trigger>.TriggerWithParameters<int> _enterRoomTrigger;
   /// <summary>
   /// int param - roomId
   /// </summary>
   private readonly StateMachine<State, Trigger>.TriggerWithParameters<int> _chooseChangeRoomTrigger;
   /// <summary>
   /// int param - playlistId chosen for deletion
   /// </summary>
   private readonly StateMachine<State, Trigger>.TriggerWithParameters<int> _chooseDeletePlaylistTrigger;
   // TODO: replace string with uri
   /// <summary>
   /// Uri - link to playlist
   /// </summary>
   private readonly StateMachine<State, Trigger>.TriggerWithParameters<string> _chooseAddPlaylistTrigger;
   private readonly IRoomsService _rooms;
   public Room? Room { get; private set; } = null;
   public int? DeletePlaylistPage { get; private set; } = null;

   public Session(long id, IRoomsService roomsService)
   {
      this.id = id;
      _rooms = roomsService;

      _enterRoomTrigger = _stateMachine.SetTriggerParameters<int>(Trigger.EnterMainRoom);
      _chooseChangeRoomTrigger = _stateMachine.SetTriggerParameters<int>(Trigger.ChooseChangeRoom);
      _chooseDeletePlaylistTrigger = _stateMachine.SetTriggerParameters<int>(Trigger.ChooseDeletePlaylist);
      _chooseAddPlaylistTrigger = _stateMachine.SetTriggerParameters<string>(Trigger.EnterAddPlaylist);

      // start
      _stateMachine.Configure(State.Start)
          .Permit(Trigger.StartBot, State.RoomEnter);

      // room entrance page
      _stateMachine.Configure(State.RoomEnter)
        .Permit(Trigger.EnterMainRoom, State.MainRoom)
        .OnEntryFrom(Trigger.LeaveRoom, OnLeaveRoom);

      // main room page
      _stateMachine.Configure(State.MainRoom)
        // enter a room
        .OnEntryFrom(_enterRoomTrigger, OnEnterRoom)
        // delete a playlist
        .Permit(Trigger.ChooseDeletePlaylist, State.DeletePlaylist)
        .OnEntryFrom(_chooseDeletePlaylistTrigger, OnChooseDeletePlaylist)
        // add a playlist
        .Permit(Trigger.ChooseAddPlaylist, State.AddPlaylistConfirm)
        .OnEntryFrom(_chooseAddPlaylistTrigger, OnChooseAddPlaylist)
        // change room
        .Permit(Trigger.EnterChangeRoom, State.ChangeRoomConfirm)
        .OnEntryFrom(_chooseChangeRoomTrigger, OnChooseChangeRoom)
        // leave room
        .Permit(Trigger.LeaveRoom, State.RoomEnter);

      _stateMachine.Configure(State.DeletePlaylist)
        .OnEntry(OnEnterDeletePlaylist)
        .Permit(Trigger.ChooseDeletePlaylist, State.MainRoom)
        .Permit(Trigger.Cancel, State.MainRoom);

      _stateMachine.Configure(State.AddPlaylistConfirm)
        .OnEntry(OnEnterAddPlaylist)
        .Permit(Trigger.EnterAddPlaylist, State.MainRoom)
        .Permit(Trigger.Cancel, State.MainRoom);

      _stateMachine.Configure(State.ChangeRoomConfirm)
        .Permit(Trigger.ChooseChangeRoom, State.MainRoom)
        .Permit(Trigger.Cancel, State.MainRoom);
   }



   private void OnEnterRoom(int newRoomId)
   {
      bool roomExists = _rooms.TryGetRoom(newRoomId, out Room? newRoom);
      if (!roomExists)
      {
         throw new ArgumentException($"Room with id: {newRoomId}, does not exist");
      }

      Room = newRoom!;
      Room.members.Add(id);
   }

   private void OnChooseChangeRoom(int NewRoomId)
   {

   }

   private void OnChooseDeletePlaylist(int PlaylistId)
   {

   }

   private void OnChooseAddPlaylist(string PlaylistLink)
   {

   }

   private void OnEnterAddPlaylist()
   {

   }

   private void OnEnterDeletePlaylist()
   {

   }

   private void OnLeaveRoom()
   {

   }

   private void OnGenerateCommon()
   {

   }

   private void OnCommonArtists()
   {

   }

   private void OnAllPlaylist()
   {

   }

   private void OnAddPlaylist()
   {

   }

   private void OnChangeRoom()
   {

   }

   private enum State
   {
      Start,
      RoomEnter,
      MainRoom,
      DeletePlaylist,
      AddPlaylistConfirm,
      ChangeRoomConfirm
   }
   private enum Trigger
   {
      StartBot,
      EnterMainRoom,
      EnterDeletePlaylist,
      ChooseDeletePlaylist,
      EnterAddPlaylist,
      ChooseAddPlaylist,
      EnterChangeRoom,
      ChooseChangeRoom,
      LeaveRoom,
      Cancel
   }
}

