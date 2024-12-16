namespace LeakyPlayTgBotNet.UserService
{
   using LeakyPlayEntities;
   using LeakyPlayTgBotNet.RoomsService;
   using Stateless;

   using State = LeakyPlayEntities.User.State;

   /// <summary>
   /// User's session with bot. By itself it doesn't reflect room state (added and generated playlists, users). Also room can change.
   /// 
   /// Commands that do not change the 'state' (/artists for example) are not represented as triggers in state machine.
   /// </summary>
   public class SessionController
   {
      public readonly User user;
      private readonly StateMachine<State, Trigger> _stateMachine = new StateMachine<State, Trigger>(State.Start);
      /// <summary>
      /// int param - newRoomId, string - newRoomName
      /// </summary>
      private readonly StateMachine<State, Trigger>.TriggerWithParameters<int, string> _createRoomTrigger;
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
      public State State => _stateMachine.State;

      public SessionController(User user, IRoomsManagerService roomsService)
      {
         this.user = user;
         _rooms = roomsService;
         _enterRoomTrigger = _stateMachine.SetTriggerParameters<int>(Trigger.EnterMainRoom);
         _createRoomTrigger = _stateMachine.SetTriggerParameters<int, string>(Trigger.CreateNewRoom);
         _chooseChangeRoomTrigger = _stateMachine.SetTriggerParameters<int>(Trigger.ChooseChangeRoom);
         _chooseDeletePlaylistTrigger = _stateMachine.SetTriggerParameters<string>(Trigger.ChooseDeletePlaylist);
         _chooseAddPlaylistTrigger = _stateMachine.SetTriggerParameters<string>(Trigger.EnterAddPlaylist);

         // register
         _stateMachine.Configure(State.Start)
             .Permit(Trigger.Start, State.RoomEnter);

         // room entrance page
         _stateMachine.Configure(State.RoomEnter)
           .PermitReentry(Trigger.CreateNewRoom)
           .Permit(Trigger.EnterMainRoom, State.MainRoom)
           .OnEntryFrom(_createRoomTrigger, OnCreateRoom)
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
           .Permit(Trigger.LeaveRoom, State.RoomEnter)
           // combine playlists
           .PermitReentry(Trigger.CombinePlaylists)
           .OnEntryFrom(Trigger.CombinePlaylists, OnGenerateCommon)
           // get common artists
           .PermitReentry(Trigger.GetCommonArtists)
           .OnEntryFrom(Trigger.GetCommonArtists, OnCommonArtists)
           // combine playlists
           .PermitReentry(Trigger.GetAllPlaylists)
           .OnEntryFrom(Trigger.GetAllPlaylists, OnAllPlaylist);

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

      public void Fire(Trigger trigger)
      {
         _stateMachine.Fire(trigger);
      }

      public void FireCreateRoom(int newRoomId, string newRoomName)
      {
         _stateMachine.Fire(_createRoomTrigger, newRoomId, newRoomName);
      }

      public void FireEnterRoom(int roomId)
      {
         _stateMachine.Fire(_enterRoomTrigger, roomId);
      }

      public void FireChangeRoom(int roomId)
      {
         _stateMachine.Fire(_chooseChangeRoomTrigger, roomId);
      }

      public void FireAddPlaylist(string playlistLink)
      {
         _stateMachine.Fire(_chooseAddPlaylistTrigger, playlistLink);
      }

      public void FireDeletePlaylist(string playlistLink)
      {
         _stateMachine.Fire(_chooseDeletePlaylistTrigger, playlistLink);
      }

      private void OnEnterRoom(int roomId)
      {
         bool roomExists = _rooms.TryGetRoom(roomId, out IRoomService? newRoom);
         if (!roomExists)
         {
            throw new ArgumentException($"Room with id: {roomId}, does not exist");
         }

         CurrentRoom = newRoom!;
         CurrentRoom.AddMember(user.Id, user.Username);
      }

      private void OnCreateRoom(int newRoomId, string newRoomName)
      {
         _rooms.TryCreateRoom(newRoomId, newRoomName, "[CombinedPlaylistName]", out _);
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
         CurrentRoom!.TryAddPlaylist(user.Id, playlistLink, "[PlaylistName]");
      }

      private void OnEnterAddPlaylist()
      {
         //present
      }

      private void OnEnterDeletePlaylist()
      {
         //present
      }

      private void OnGenerateCommon()
      {
         //present placeholder
      }

      private void OnCommonArtists()
      {
         CurrentRoom!.GetArtistsNames();
      }

      private void OnAllPlaylist()
      {
         CurrentRoom!.GetPlaylistsNames();
      }

      public enum Trigger
      {
         Start,
         EnterMainRoom,
         CreateNewRoom,
         EnterDeletePlaylist,
         ChooseDeletePlaylist,
         EnterAddPlaylist,
         CombinePlaylists,
         GetCommonArtists,
         GetAllPlaylists,
         ChooseAddPlaylist,
         EnterChangeRoom,
         ChooseChangeRoom,
         LeaveRoom,
         Cancel
      }
   }
}