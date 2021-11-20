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
            var color = Color.FromArgb(255, (byte)_rand.Next(0, 255), (byte)_rand.Next(0, 255), (byte)_rand.Next(0, 255)); //Not good for gifs.
            WB.Clear(Colors.CornflowerBlue);

            foreach (var ball in _vm.Balls)
            {
                WB.FillEllipseCentered((int)ball.Location.X, (int)ball.Location.Y, 5, 5, Colors.Blue);
            }
            WB.Unlock();
        }
    }
}
