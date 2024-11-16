using LeakyPlayEntities;

namespace LeakyPlayTgBotNet.RoomsService;

public class RoomService : IRoomService
{
   public const int playlistsPerPage = 5;
   public readonly Room info;
   private readonly HashSet<User> _members = [];
   private readonly HashSet<Playlist> _playlists = [];
   public int AvailableDeletePages => 1 + (_playlists.Count - 1) / playlistsPerPage;

   public RoomService(Room room)
   {
      info = room;
   }

   public bool AddMember(long id, string? username)
   {
      if (_members.Any(user => user.Id == id))
      {
         return false;
      }

      _members.Add(new User
      {
         Id = id,
         Username = username
      });
      return true;
   }

   public bool TryRemoveMember(long id)
   {
      return _members.RemoveWhere(user => user.Id == id) > 0;
   }

   public bool TryAddPlaylist(long creatorId, string link, string name)
   {
      if (_playlists.Any(playlist => playlist.Link == link))
      {
         return false;
      }

      _playlists.Add(new Playlist
      {
         CreatorId = creatorId,
         Link = link,
         Name = name
      });
      return true;
   }

   public bool TryDeletePlaylist(string link)
   {
      return _playlists.RemoveWhere(playlist => playlist.Link == link) > 0;
   }


   public bool TryCombinePlaylists()
   {
      if (_playlists.Count == 0)
      {
         return false;
      }

      return true;
   }

   public string[] GetArtistsNames()
   {
      return ["There", "will", "be", "aritsts", "names", "list", "from", "Yandex.API"];
   }

   public string[] GetPlaylistsNames()
   {
      return _playlists.Select(playlist => playlist.Name).ToArray();
   }

   public string[] GetMembersUsernames()
   {
      return _members.Select(user =>
      {
         if (user.Username is not null)
         {
            return $"@{user.Username}";
         }
         return $"@{user.Id}";
      }).ToArray();
   }

   public string GetCombinedPlaylistLink()
   {
      return info.CombinedPlaylistLink;
   }
}
