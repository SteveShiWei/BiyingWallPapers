using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BingWallpapers
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DayPicturesPage : Page
    {
        public DayPicturesPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null && e.Parameter is int)
            {
                WallpapersService.Current.GetOneDayWallpapersProgressEvent += OnOneDayWallpapersProgressEvent;
                WallpapersService.Current.GetOneDayWallpapers((int)e.Parameter);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            WallpapersService.Current.GetOneDayWallpapersProgressEvent -= OnOneDayWallpapersProgressEvent;
            base.OnNavigatedFrom(e);
        }

        private async void OnOneDayWallpapersProgressEvent(object sender, ProgressEventArgs e)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (e.IsException)
                {
                    info.Text = "获取图片异常：" + e.ExceptionInfo;
                }
                else
                {
                    progress.Value = e.ProgressValue;
                    if (e.Complete)
                    {
                        tips.Visibility = Visibility.Collapsed;
                        pictureList.ItemsSource = e.Pictures;
                        Debug.WriteLine("e.Pictures.Count:" + e.Pictures.Count);
                    }
                }
            });

        }

        private async void view_Click(object sender, RoutedEventArgs e)
        {
            PictureInfo pictureInfo = (sender as AppBarButton).DataContext as PictureInfo;
            await Launcher.LaunchUriAsync(pictureInfo.imageUri);
        }

        private async void save_Click(object sender, RoutedEventArgs e)
        {
            PictureInfo pictureInfo = (sender as AppBarButton).DataContext as PictureInfo;
            List<Byte> allBytes = new List<byte>();
            using (var response = await HttpWebRequest.Create(pictureInfo.imageUri).GetResponseAsync())
            {
                using (Stream responseStream = response.GetResponseStream())
                {

                    byte[] buffer = new byte[4000];
                    int bytesRead = 0;
                    while ((bytesRead = await responseStream.ReadAsync(buffer, 0, 4000)) > 0)
                    {
                        allBytes.AddRange(buffer.Take(bytesRead));
                    }
                }
            }
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(
                        "bingPicture" + DateTime.Now.Ticks + ".jpg", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteBytesAsync(file, allBytes.ToArray());
        }
    }
}

