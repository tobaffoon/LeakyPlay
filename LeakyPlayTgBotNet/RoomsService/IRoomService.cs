namespace LeakyPlayTgBotNet.RoomsService
{
   /// <summary>
   /// Service to manage a single room
   /// </summary>
   public interface IRoomService
   {
      /// <summary>
      /// Adds member to a room.
      /// </summary>
      /// <returns>True if member was added. False if member was already present.</returns>
      bool AddMember(long id, string? username);
      /// <summary>
      /// Removes member by id.
      /// </summary>
      /// <returns>True if member was removed. False if member was not present.</returns>
      bool TryRemoveMember(long id);
      // TODO replace string with uri
      /// <summary>
      /// Add playlist to combine list.
      /// </summary>
      /// <returns>True if playlist was added. False if playlist was already present.</returns>
      bool TryAddPlaylist(long creatorId, string link, string name);
      /// <summary>
      /// Try deleting playlist from combine list by link.
      /// </summary>
      /// <returns>True if playlist was deleted from combine list. False if it doesn't exist.</returns>
      bool TryDeletePlaylist(string link);
      /// <summary>
      /// Try combining playlists in combine list.
      /// </summary>
      /// <returns>True if playlist was created. False otherwise, for example if combine list is empty.</returns>
      bool TryCombinePlaylists();
      string GetCombinedPlaylistLink();
      string[] GetArtistsNames();
      string[] GetPlaylistsNames();
      string[] GetMembersUsernames();
   }
}