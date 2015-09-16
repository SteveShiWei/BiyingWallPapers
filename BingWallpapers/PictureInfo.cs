using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace BingWallpapers
{
    public class PictureInfo
    {
        public List<string> hotspot { get; set; }
        public string imgTitle { get; set; }
        public Uri imageUri { get; set; }
        public BitmapImage image { get; set; }
        public string countryCode { get; set; }
        public PictureInfo(string _countryCode, string _imgTitle, string _imgUri)
        {
            countryCode = _countryCode;
            imgTitle = _imgTitle;
            imageUri = new Uri(_imgUri);
        }
    }
}
