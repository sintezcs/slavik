using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Devices;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Slavik.Data;

namespace Slavik.Common
{
    public class SlavikPlayer
    {
        private ObservableCollection<SampleDataItem> items = new ObservableCollection<SampleDataItem>();

        private bool isPlaying = false;

        private MediaElement player;

        private Queue<IStorageFile> playlist = new Queue<IStorageFile>(); 


        public SlavikPlayer()
        {            
            player.MediaEnded += player_MediaEnded;
        }

        public ObservableCollection<SampleDataItem> Sounds { get { return items; } set { items = value; } } 

        void player_MediaEnded(object sender, RoutedEventArgs args)
        {
            playlist.Dequeue();
            if (playlist.Count > 0)
            {
                PlayLastItem();
            }
        }

        async void PlayLastItem()
        {
            var item = playlist.Peek();
            var stream = await item.OpenAsync(FileAccessMode.Read);
            player.SetSource(stream, item.ContentType);
            player.Play();
        }

        public void Play(string itemId)
        {
            var item = items.FirstOrDefault(i => i.UniqueId == itemId);
            
            if (item == null)
            {
                return;
            }
            
            playlist.Enqueue(item.Audio);
            PlayLastItem();
        } 
    }
}
