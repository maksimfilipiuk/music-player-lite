using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace music_player_lite.Model
{
    class PlayerData
    {
        ObservableCollection<string> playlists;
        ObservableCollection<string> songs;

        int currentPlaylistIndex = -1;

        public ObservableCollection<string> Playlists
        {
            get
            {
                if(playlists == null)
                {
                    playlists = LoadPlaylists();
                }

                return playlists;
            }
        }

        public ObservableCollection<string> Songs { get => songs; }

        public int CurrentPlaylistIndex
        {
            get
            {
                return currentPlaylistIndex;
            }
            set
            {
                currentPlaylistIndex = value;
                songs = LoadSongs(currentPlaylistIndex);
            }
        }

        private ObservableCollection<string> LoadSongs(int playlistIndex)
        {
            ObservableCollection<string> _songs = new ObservableCollection<string>();
            string path = "music\\" + playlists[playlistIndex];
            string[] files = Directory.GetFiles(path, "*.mp3");

            for (int i = 0; i < files.Length; i++)
            {
                files[i] = files[i].Replace(path + "\\", String.Empty);
                files[i] = files[i].Replace(".mp3", String.Empty);
            }

            foreach (var item in files)
            {
                _songs.Add(item);
            }

            return _songs;
        }

        private ObservableCollection<string> LoadPlaylists()
        {
            ObservableCollection<string> _playlists = new ObservableCollection<string>();
            DirectoryInfo directory = new DirectoryInfo("music");

            foreach (var item in directory.GetDirectories())
            {
                _playlists.Add(item.Name);
            }

            return _playlists;
        }
    }
}
