using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BingWallpapers
{
    public class ProgressEventArgs : EventArgs
    {
        public int ProgressValue { get; set; }
        public bool Complete { get; set; }
        public bool IsException { get; set; }
        public string ExceptionInfo { get; set; }
        public List<PictureInfo> Pictures { get; set; }
    }
}
