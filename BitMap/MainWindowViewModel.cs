using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BitMap
{
    public class MainWindowViewModel
    {
        public WriteableBitmap WB { get; set; } = new(300, 300, 96, 96, PixelFormats.Bgra32, null);

        public MainWindowViewModel()
        {


        }
    }
}
