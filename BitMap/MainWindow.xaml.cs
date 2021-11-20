using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace BitMap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public WriteableBitmap WB { get; set; }
        private Random _rand;
        private MainWindowViewModel _vm;
        public MainWindow()
        {

            InitializeComponent();
            _vm = DataContext as MainWindowViewModel;


            _rand = new();
            WB = new(300, 300, 96, 96, PixelFormats.Bgra32, null);
            //img.Source = WB;
            DispatcherTimer renderTimer = new(DispatcherPriority.Render);
            renderTimer.Interval = TimeSpan.FromMilliseconds(1);
            renderTimer.Tick += RenderTimer_Tick;
            renderTimer.Start();
        }

        private void RenderTimer_Tick(object? sender, EventArgs e)
        {
            WB.Lock();

            byte[] colorData = { (byte)_rand.Next(0, 255), (byte)_rand.Next(0, 255), (byte)_rand.Next(0, 255), 255 };

            for (int x = 0; x < WB.PixelWidth; x++)
            {

                for (int y = 0; y < WB.PixelWidth; y++)
                {
                    Int32Rect rect = new(x, y, 1, 1);
                    var stride = WB.PixelWidth * WB.Format.BitsPerPixel / 8;
                    WB.WritePixels(rect, colorData, stride, 0);
                }
            }
            WB.Unlock();
        }
    }
}
