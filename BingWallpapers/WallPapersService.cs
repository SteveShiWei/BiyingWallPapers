using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Html;

namespace BingWallpapers
{
    public class WallpapersService
    {
        private int selectedDay;
        private List<string> countries = new List<string>(new string[] {
            "zh-CN", "fr-FR",  "de-DE","en-US",  "ja-JP","en-GB"});
        private int http_times;
        private int http_times_all;
        private bool downloading = false;
        private Dictionary<int, List<PictureInfo>> allHaveDownloadPictures;
        public EventHandler<ProgressEventArgs> GetOneDayWallpapersProgressEvent;
        private void OnGetOneDayWallpapersProgressEvent(ProgressEventArgs progressEventArgs)
        {
            if (GetOneDayWallpapersProgressEvent != null)
            {
                GetOneDayWallpapersProgressEvent.Invoke(this, progressEventArgs);
            }
        }
        private static WallpapersService _Current;
        public static WallpapersService Current
        {
            get
            {
                if (_Current == null)
                    _Current = new WallpapersService();
                return _Current;
            }
        }

        private WallpapersService()
        {
            allHaveDownloadPictures = new Dictionary<int, List<PictureInfo>>();
        }

        public void GetOneDayWallpapers(int day)
        {
            if (downloading) return;
            downloading = true;
            selectedDay = day;
            if (!allHaveDownloadPictures.Keys.Contains(selectedDay))
            {
                allHaveDownloadPictures.Add(selectedDay, new List<PictureInfo>());
            }
            string format = "http://appserver.m.bing.net/BackgroundImageService/TodayImageService.svc/GetTodayImage?dateOffset=-{0}&urlEncodeHeaders=true&osName=windowsPhone&osVersion=8.10&orientation=480x800&deviceName=WP8Device&mkt={1}";
            http_times_all = http_times = countries.Count<string>();
            foreach (string country in countries)
            {
                if (allHaveDownloadPictures[selectedDay].Select(item => item.countryCode == country).Count() == 0)
                {
                    string bingUrlFmt = string.Format(format, selectedDay, country);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(bingUrlFmt);
                    request.Method = "GET";
                    request.BeginGetResponse(result => this.responseHandler(result, request, country, bingUrlFmt), null);
                }
                else
                {
                    SetProgress();
                }
            }
        }

        private void responseHandler(IAsyncResult asyncResult, HttpWebRequest request, string myloc, string _imgUri)
        {
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.EndGetResponse(asyncResult);
            }
            catch (Exception e)
            {
                downloading = false;
                OnGetOneDayWallpapersProgressEvent(
                    new ProgressEventArgs
                    {
                        IsException = true,
                        Complete = false,
                        ExceptionInfo = e.Message,
                        ProgressValue = 0,
                        Pictures = null
                    });
                return;
            }
            if (request.HaveResponse)
            {
                List<string> _hotspot = new List<string>();
                string _imgTitle = "";
                foreach (string str in response.Headers.AllKeys)
                {
                    string str2 = str;
                    string str3 = response.Headers[str];
                    if (str2.Contains("Image-Info-Credit"))
                    {
                        _imgTitle = WebUtility.UrlDecode(str3);
                    }
                    else if (str2.Contains("Image-Info-Hotspot-"))
                    {
                        string[] strArray = WebUtility.UrlDecode(str3).Replace(" ", "").Split(new char[] { ';' });
                        _hotspot.AddRange(strArray);
                    }
                }
                PictureInfo info = new PictureInfo(myloc, _imgTitle, _imgUri);
                info.hotspot = _hotspot;
                allHaveDownloadPictures[selectedDay].Add(info);
            }
            Debug.WriteLine("allHaveDownloadPictures[selectedDay].Count:" + allHaveDownloadPictures[selectedDay].Count);
            SetProgress();
        }

        private void SetProgress()
        {
            http_times--;
            bool finish = http_times == 0;
            if (finish)
                downloading = false;
            OnGetOneDayWallpapersProgressEvent(
           new ProgressEventArgs
           {
               IsException = false,
               Complete = finish,
               ExceptionInfo = "",
               ProgressValue = (int)(((float)(http_times_all - http_times) / (float)http_times_all) * 100),
               Pictures = allHaveDownloadPictures[selectedDay]
           });
        }
    }
}
