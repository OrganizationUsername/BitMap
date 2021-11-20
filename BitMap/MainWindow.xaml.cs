using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace BitMap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public WriteableBitmap WB => _vm.Wb;
        private Random _rand;
        private MainWindowViewModel _vm;
        public MainWindow()
        {
            InitializeComponent();
            _vm = DataContext as MainWindowViewModel;
            if (_vm is null) throw new NullReferenceException("VM");

            _rand = new();
            img.Source = _vm.Wb;
            DispatcherTimer renderTimer = new(DispatcherPriority.Render);
            renderTimer.Interval = TimeSpan.FromMilliseconds(10);
            renderTimer.Tick += RenderTimer_Tick;
            renderTimer.Start();
        }

        private void RenderTimer_Tick(object? sender, EventArgs e)
        {
            _vm.Tick();
            WB.Lock();

            var stride = WB.PixelWidth * WB.Format.BitsPerPixel / 8;
            WB.Clear(Colors.CornflowerBlue);

            foreach (var ball in _vm.Balls)
            {
                WB.FillEllipseCentered((int)ball.Location.X, (int)ball.Location.Y, 5, 5, Color.FromArgb(255, ball.ColorData[0], ball.ColorData[1], ball.ColorData[2]));
            }
            WB.Unlock();
        }
    }
}
