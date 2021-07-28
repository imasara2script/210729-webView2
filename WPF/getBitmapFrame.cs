using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 
using System.Windows.Media.Imaging;

namespace netHTA
{
    class getBitmapFrame
    {
        public BitmapFrame fromString(string アドレス)
        {
            Uri bmpURI = new Uri(アドレス, UriKind.RelativeOrAbsolute);
            return BitmapFrame.Create(bmpURI);
        }
    }
}
