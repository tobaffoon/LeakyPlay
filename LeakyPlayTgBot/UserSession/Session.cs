using Stateless;

namespace LeakyPlayTgBot.UserSession
{
   /// <summary>
   /// User's session with bot. It doesn't reflect room state (added and generated playlists, users). Also room can change.
   /// </summary>
   internal class Session
   {
      private const int _playlistsPerPage = 5;

      private readonly StateMachine<State, Trigger> _stateMachine;
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
      /// <summary>
      /// Uri - link to playlist
      /// </summary>
      private readonly StateMachine<State, Trigger>.TriggerWithParameters<Uri> _chooseAddPlaylistTrigger;
      private readonly int _userId;

      private int? _roomId;
      private int? _deletePlaylistPage;
      private List<string> _playlistNames;
      private int _availableDeletePages => 1 + (_playlistNames.Count - 1) / _playlistsPerPage;

      public Session(int userId)
      {
         _userId = userId;
         _deletePlaylistPage = null;
         _stateMachine = new StateMachine<State, Trigger>(State.Start);
         _playlistNames = [];

         _enterRoomTrigger = _stateMachine.SetTriggerParameters<int>(Trigger.EnterMainRoom);
         _chooseChangeRoomTrigger = _stateMachine.SetTriggerParameters<int>(Trigger.ChooseChangeRoom);
         _chooseDeletePlaylistTrigger = _stateMachine.SetTriggerParameters<int>(Trigger.ChooseDeletePlaylist);
         _chooseAddPlaylistTrigger = _stateMachine.SetTriggerParameters<Uri>(Trigger.EnterAddPlaylist);

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
           // generate common playlist
           .PermitReentry(Trigger.GenerateCommon)
           .OnEntryFrom(Trigger.GenerateCommon, OnGenerateCommon)
           // show all artists in common playlist
           .PermitReentry(Trigger.ShowCommonArtists)
           .OnEntryFrom(Trigger.ShowCommonArtists, OnCommonArtists)
           // show all playlists
           .PermitReentry(Trigger.ShowAllPlaylists)
           .OnEntryFrom(Trigger.ShowAllPlaylists, OnAllPlaylist)
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



      private void OnEnterRoom(int NewRoomId)
      {

      }

      private void OnChooseChangeRoom(int NewRoomId)
      {

      }

      private void OnChooseDeletePlaylist(int PlaylistId)
      {

      }

      private void OnChooseAddPlaylist(Uri PlaylistLink)
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
         GenerateCommon,
         ShowCommonArtists,
         ShowAllPlaylists,
         EnterChangeRoom,
         ChooseChangeRoom,
         LeaveRoom,
         Cancel
      }
   }
}
