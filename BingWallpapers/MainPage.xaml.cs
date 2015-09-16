using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class MainPage : Page
    {
        private string TodayPictureUri = "http://appserver.m.bing.net/BackgroundImageService/TodayImageService.svc/GetTodayImage?dateOffset=-0&urlEncodeHeaders=true&osName=windowsphone&osVersion=8.10&orientation=480x800&deviceName=WP8Device&mkt=zh-CN";
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            background.UriSource = new Uri(TodayPictureUri);
        }

        private void today_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DayPicturesPage), 0);
        }

        private void yesterday_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DayPicturesPage), 1);
        }

        private void twodayago_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DayPicturesPage), 2);
        }

        private void threedayago_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DayPicturesPage), 3);
        }

        private void other_Click(object sender, RoutedEventArgs e)
        {
            showMorePicture.Begin();
        }

        private void minus_bar_Click(object sender, RoutedEventArgs e)
        {
            int day = Int32.Parse(dayNumber.Text);
            if (day > 0)
            {
                day--;
                dayNumber.Text = day.ToString();
            }
        }

        private void plus_bar_Click(object sender, RoutedEventArgs e)
        {
            int day = Int32.Parse(dayNumber.Text);
            day++;
            dayNumber.Text = day.ToString();

        }

        private void go_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DayPicturesPage), Int32.Parse(dayNumber.Text));
        }
    }
}
