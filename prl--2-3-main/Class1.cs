using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1
{
    internal class Mp3Player
    {
        public static MediaPlayer Play(MediaPlayer player, string ListAbsolutePathFile, TimeSpan? PositionTimeMedia = null)
        {
            player.Open(new Uri(ListAbsolutePathFile, UriKind.Absolute));
            player.Position = (TimeSpan)(PositionTimeMedia == null ? TimeSpan.Zero : PositionTimeMedia);
            player.Play();
            return player;
        }
        public static dynamic[] Stop(MediaPlayer player)
        {
            player.Pause();
            return new dynamic[2] {player, player.Position };
        }
        public static dynamic[] Back(MediaPlayer player, List<string> ListAbsolutePathFile, int IndexMusic)
        {
            IndexMusic--;
            IndexMusic = IndexMusic < 0 ? ListAbsolutePathFile.Count - 1 : IndexMusic;
            player = Mp3Player.Play(Mp3Player.Stop(player)[0], ListAbsolutePathFile[IndexMusic]);
            return new dynamic[2] { player, IndexMusic };
        }
        public static dynamic[] Next(MediaPlayer player, List<string> ListAbsolutePathFile, int IndexMusic)
        {
            IndexMusic++;
            IndexMusic = IndexMusic == ListAbsolutePathFile.Count ? 0 : IndexMusic;
            player = Mp3Player.Play(Mp3Player.Stop(player)[0], ListAbsolutePathFile[IndexMusic]);
            return new dynamic[2] {player ,IndexMusic};
        }
        public static dynamic[] Random(MediaPlayer player, List<string> ListAbsolutePathFile, int IndexMusic)
        {
            Random rand = new Random();
            IndexMusic = rand.Next(0, ListAbsolutePathFile.Count);
            player = Mp3Player.Play(Mp3Player.Stop(player)[0], ListAbsolutePathFile[IndexMusic]);
            return new dynamic[2] { player, IndexMusic };
        }
        public static dynamic[] Repeat(MediaPlayer player, List<string> ListAbsolutePathFile, int IndexMusic)
        {
            player = Mp3Player.Play(Mp3Player.Stop(player)[0], ListAbsolutePathFile[IndexMusic]);
            return new dynamic[2] { player, IndexMusic };
        }
    }
}
