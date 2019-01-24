using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using music_player_lite.Model;
using music_player_lite.Infrastructure;
using System.Windows.Input;
using System.Windows.Media;

namespace music_player_lite.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        private static PlayerData playerData;
        public PlayerData PlayerData
        {
            get
            {
                if(playerData == null)
                {
                    playerData = new PlayerData();
                }

                return playerData;
            }
        }
        public ObservableCollection<string> Playlist
        {
            get
            {
                return PlayerData.Playlists;
            }
        }
        public ObservableCollection<string> Songs
        {
            get
            {
                return PlayerData.Songs;
            }
        }

        MediaPlayer mediaPlayer = new MediaPlayer();
        bool isMediaPlayerPaused = false;

        private int[] currentSong = { -1, -1 }; // 0 - playlist index, 1 - song index;
        private int currentSongListIndex = -1;

        public int CurrentPlaylistIndex
        {
            get
            {
                return PlayerData.CurrentPlaylistIndex;
            }
            set
            {
                PlayerData.CurrentPlaylistIndex = value;
                OnPropertyChanged("Songs");
            }
        }

        public int[] CurrentSong { get => currentSong; set => currentSong = value; }
        public int CurrentSongListIndex { get => currentSongListIndex; set => currentSongListIndex = value; }

        public string SongTitle { get => Songs[CurrentSong[1]]; }

        public float Volume { set => mediaPlayer.Volume = value; }
        public float Balance { set => mediaPlayer.Balance = value; }
        public float Speed { set => mediaPlayer.SpeedRatio = value; }

        RelayCommand updateSongListCommand;
        public ICommand UpdateSongList
        {
            get
            {
                if(updateSongListCommand == null)
                {
                    updateSongListCommand = new RelayCommand(ExecuteUpdateSongListCommand, CanExecuteUpdateSongListCommand);
                }

                return updateSongListCommand;
            }
        }

        RelayCommand playSongCommand;
        public ICommand PlaySongCommand
        {
            get
            {
                if(playSongCommand == null)
                {
                    playSongCommand = new RelayCommand(ExecutePlaySongCommand, CanExecutePlaySongCommand);
                }

                return playSongCommand;
            }
        }

        RelayCommand pauseSongCommand;
        public ICommand PauseSongCommand
        {
            get
            {
                if (pauseSongCommand == null)
                {
                    pauseSongCommand = new RelayCommand(ExecutePauseSongCommand, CanExecutePauseSongCommand);
                }

                return pauseSongCommand;
            }
        }

        RelayCommand stopSongCommand;
        public ICommand StopSongCommand
        {
            get
            {
                if (stopSongCommand == null)
                {
                    stopSongCommand = new RelayCommand(ExecuteStopSongCommand, CanExecuteStopSongCommand);
                }

                return stopSongCommand;
            }
        }

        private bool CanExecuteUpdateSongListCommand(object obj)
        {
            if (CurrentPlaylistIndex == -1)
                return false;

            return true;
        }

        private bool CanExecutePlaySongCommand(object obj)
        {
            if (CurrentSongListIndex == -1)
                return false;
            return true;
        }

        private bool CanExecutePauseSongCommand(object obj)
        {
            if (CurrentSong[1] == -1 || isMediaPlayerPaused)
                return false;
            return true;
        }

        private bool CanExecuteStopSongCommand(object obj)
        {
            if (CurrentSong[1] == -1)
                return false;
            return true;
        }


        private void ExecuteUpdateSongListCommand(object obj)
        {
            CurrentPlaylistIndex = CurrentPlaylistIndex;
        }

        private void ExecutePlaySongCommand(object obj)
        {
            if (CurrentSong[0] == CurrentPlaylistIndex && CurrentSong[1] == CurrentSongListIndex)
            {
                mediaPlayer.Play();
                isMediaPlayerPaused = false;
                return;
            }

            PlaySong(CurrentPlaylistIndex, CurrentSongListIndex);
        }

        private void ExecutePauseSongCommand(object obj)
        {
            mediaPlayer.Pause();
            isMediaPlayerPaused = true;
        }

        private void ExecuteStopSongCommand(object obj)
        {
            mediaPlayer.Stop();
            CurrentSong[0] = -1;
            CurrentSong[1] = -1;
        }


        private void PlaySong(int currentPlaylistIndex, int currentSongListIndex)
        {
            isMediaPlayerPaused = false;

            string path = "music\\" + Playlist[CurrentPlaylistIndex] + "\\" + Songs[CurrentSongListIndex] + ".mp3";
            mediaPlayer.Open(new Uri(path, UriKind.Relative));
            mediaPlayer.Play();

            // в массив CurrentSong записываем playlist index и song index
            CurrentSong[0] = CurrentPlaylistIndex;
            CurrentSong[1] = CurrentSongListIndex;

            OnPropertyChanged("SongTitle");
            OnPropertyChanged("RewindMaximum");
        }
    }
}
