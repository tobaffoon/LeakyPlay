using LeakyPlayEntities;
using LeakyPlayTgBotNet.RoomsService;
using Stateless;

using State = LeakyPlayEntities.Session.State;

namespace LeakyPlayTgBotNet.UserService;

/// <summary>
/// User's session with bot. By itself it doesn't reflect room state (added and generated playlists, users). Also room can change.
/// 
/// Commands that do not change the 'state' (/artists for example) are not represented as triggers in state machine.
/// </summary>
internal class SessionController
{
   public readonly User user;
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
   /// string param - playlist link chosen for deletion
   /// </summary>
   private readonly StateMachine<State, Trigger>.TriggerWithParameters<string> _chooseDeletePlaylistTrigger;
   // TODO: replace string with uri
   /// <summary>
   /// Uri - link to playlist
   /// </summary>
   private readonly StateMachine<State, Trigger>.TriggerWithParameters<string> _chooseAddPlaylistTrigger;
   private readonly IRoomsManagerService _rooms;
   public IRoomService? CurrentRoom { get; private set; } = null;
   public int? DeletePlaylistPage { get; private set; } = null;

   public SessionController(User user, IRoomsManagerService roomsService)
   {
      this.user = user;
      _rooms = roomsService;
      _enterRoomTrigger = _stateMachine.SetTriggerParameters<int>(Trigger.EnterMainRoom);
      _chooseChangeRoomTrigger = _stateMachine.SetTriggerParameters<int>(Trigger.ChooseChangeRoom);
      _chooseDeletePlaylistTrigger = _stateMachine.SetTriggerParameters<string>(Trigger.ChooseDeletePlaylist);
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
      bool roomExists = _rooms.TryGetRoom(newRoomId, out IRoomService? newRoom);
      if (!roomExists)
      {
         throw new ArgumentException($"Room with id: {newRoomId}, does not exist");
      }

      CurrentRoom = newRoom!;
      CurrentRoom.AddMember(user.Id, user.Username);
   }

   // state entryActions don't check conditions as it's done by state machine
   private void OnLeaveRoom()
   {
      CurrentRoom!.TryRemoveMember(user.Id);
      CurrentRoom = null;
   }

   private void OnChooseChangeRoom(int newRoomId)
   {
      OnLeaveRoom();
      OnEnterRoom(newRoomId);
   }

   private void OnChooseDeletePlaylist(string playlistLink)
   {
      CurrentRoom!.TryDeletePlaylist(playlistLink);
   }

   private void OnChooseAddPlaylist(string playlistLink)
   {
   }

   private void OnEnterAddPlaylist()
   {

   }

   private void OnEnterDeletePlaylist()
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

