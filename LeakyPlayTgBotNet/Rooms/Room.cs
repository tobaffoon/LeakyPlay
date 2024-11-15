namespace LeakyPlayTgBotNet.Rooms;

public class Room
{
   public const int playlistsPerPage = 5;
   public readonly long id;
   private readonly List<string> _playlistNames = [];
   public int AvailableDeletePages => 1 + (_playlistNames.Count - 1) / playlistsPerPage;
   public readonly HashSet<long> members = [];
   public Room(long id)
   {
      this.id = id;
   }
}
