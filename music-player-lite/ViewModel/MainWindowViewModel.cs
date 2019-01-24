using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using music_player_lite.Model;

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

        

        

        
        
    }
}
