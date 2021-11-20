using System;
using System.Numerics;
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
        public WriteableBitmap WB => _vm.WB;
        private Random _rand;
        private MainWindowViewModel _vm;
        public MainWindow()
        {

            InitializeComponent();
            _vm = DataContext as MainWindowViewModel;
            if (_vm is null) throw new NullReferenceException("VM");

            _rand = new();
            img.Source = _vm.WB;
            DispatcherTimer renderTimer = new(DispatcherPriority.Render);
            renderTimer.Interval = TimeSpan.FromMilliseconds(10);
            renderTimer.Tick += RenderTimer_Tick;
            renderTimer.Start();
        }

        private void RenderTimer_Tick(object? sender, EventArgs e)
        {
            WB.Lock();
            _vm.Tick();
            byte[] colorData = { (byte)_rand.Next(0, 255), (byte)_rand.Next(0, 255), (byte)_rand.Next(0, 255), 255 };
            byte[] black = { 0, 0, 0, 255 };
            var stride = WB.PixelWidth * WB.Format.BitsPerPixel / 8;
            for (int x = 0; x < WB.PixelWidth; x++)
            {
                for (int y = 0; y < WB.PixelWidth; y++)
                {
                    Int32Rect rect = new(x, y, 1, 1);
                    bool Any = false;
                    foreach (var ball in _vm.Balls)
                    {
                        if (Vector2.DistanceSquared(new(x, y), ball.Location) < 10)
                        {
                            Any = true;
                            WB.WritePixels(rect, black, stride, 0);
                            break;
                        }
                    }
                    if (!Any)
                    {
                        WB.WritePixels(rect, colorData, stride, 0);
                    }
                    foreach (var ball in _vm.Balls)
                    {
                        //WB.FillEllipseCentered((int)ball.Location.X, (int)ball.Location.Y, 5, 5, Colors.Blue); //Extremely slow.
                    }
                }
            }
            WB.Unlock();
        }
    }
}
