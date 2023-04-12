using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;
using static System.Net.Mime.MediaTypeNames;
using Path = System.IO.Path;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TimeSpan PositionTimeMusic = new TimeSpan();
        private List<string> ListAbsolutePathFile = new List<string>();
        private MediaPlayer player = new MediaPlayer();
        private int IndexMusic = -1;
        private int SpedRatio;

        private bool StartPlayMusic;
        private bool RepeatIsEnbled;
        private bool RandomIsEnbled;
        public MainWindow()
        {
            InitializeComponent();
            player.MediaEnded += Player_MediaEnded;
            player.MediaOpened += Player_MediaOpened;
            player.Volume = Sound.Value / 100;
            ValueSoundLabel.Content = Sound.Value;
            Thread th = new Thread(_ =>
            {
                while (true)
                {
                    while (StartPlayMusic)
                    {
                        Dispatcher.Invoke((Action)(() =>
                        {
                            SliderTimeMusic.Value = (double)player.Position.TotalSeconds;
                            NowTimeMedia.Content = $"{(int)player.Position.TotalMinutes}:{player.Position.Seconds:00}";
                        }));
                        Thread.Sleep(1);
                    }
                }
            });
            th.Start();
        }

        private void Player_MediaOpened(object? sender, EventArgs e)
        {
            StartPlayMusic = true;
            SpedRatio = (int)player.SpeedRatio;
            AllTimeMedia.Content = $"{(int)player.NaturalDuration.TimeSpan.TotalMinutes}:{player.NaturalDuration.TimeSpan.Seconds:00}";
            SliderTimeMusic.Maximum = (double)player.NaturalDuration.TimeSpan.TotalSeconds;
        }

        private void Player_MediaEnded(object? sender, EventArgs e)
        {
            StartPlayMusic = false;
            if (RepeatIsEnbled)
            {
                Mp3Player.Repeat(player, ListAbsolutePathFile, IndexMusic);
            }
            else
            {
                Next_Click(null, null);
            }
        }

        private void ButtonChooseDirectories_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { IsFolderPicker = true };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                List<string> ListMusicName = new List<string>();
                ListAbsolutePathFile = Directory.GetFiles(dialog.FileName).ToList();
                foreach (var element in ListAbsolutePathFile)
                {
                    ListMusicName.Add(Path.GetFileName(element));
                }
                ListBoxMusic.ItemsSource = ListMusicName;
            }
            Back.IsEnabled = true;
            Next.IsEnabled = true;
            Repeate.IsEnabled = true;
            Random.IsEnabled = true;
            
            if (ListAbsolutePathFile.Count != 0)
            {
                Mp3Player.Play(player, ListAbsolutePathFile[0]);
            }

        }

        private void ListBoxMusic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IndexMusic = ListBoxMusic.SelectedIndex;
            player = Mp3Player.Play(player, ListAbsolutePathFile[IndexMusic]);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ReturnsValue(Mp3Player.Back(player, ListAbsolutePathFile, IndexMusic));
        }


        private void Next_Click(object sender, RoutedEventArgs e)
        {
            PositionTimeMusic = TimeSpan.Zero;

            if (RandomIsEnbled)
            {
                ReturnsValue(Mp3Player.Random(player, ListAbsolutePathFile, IndexMusic));
            }
            else
            {
                ReturnsValue(Mp3Player.Next(player, ListAbsolutePathFile, IndexMusic));
            }
        }

        private void Repeate_Click(object sender, RoutedEventArgs e)
        {
            if (RepeatIsEnbled)
            {
                Repeate.Background = Brushes.Red;
                RepeatIsEnbled = false;
            }
            else
            {
                Repeate.Background = Brushes.Green;
                RepeatIsEnbled = true;
            }
        }

        private void Random_Click(object sender, RoutedEventArgs e)
        {
            if (RandomIsEnbled)
            {
                Random.Background = Brushes.Red;
                RandomIsEnbled = false;
            }
            else
            {
                Random.Background = Brushes.Green;
                RandomIsEnbled = true;
            }
        }

        private void Sound_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                player.Volume = Sound.Value / 100;
                ValueSoundLabel.Content = (Convert.ToInt32(Sound.Value) * 10).ToString();
            }
            catch { }
        }
        private void ReturnsValue(dynamic[] list, bool ReturnTimeSpand = false)
        {
            player = list[0];
            if (ReturnTimeSpand)
            {
                PositionTimeMusic = list[1];
            }
            else
            {
                IndexMusic = list[1];
            }
        }

        private void SliderTimeMusic_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            player.Position = TimeSpan.FromSeconds(SliderTimeMusic.Value);
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (IndexMusic != -1)
            {
                player = Mp3Player.Play(player, ListAbsolutePathFile[IndexMusic], PositionTimeMusic);
                StartPlayMusic = true;

            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            if (StartPlayMusic)
            {
                ReturnsValue(Mp3Player.Stop(player), true);
                StartPlayMusic = false;

            }
        }
    }
}
