using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;

namespace BitMap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public WriteableBitmap Wb => _vm.Wb;
        private readonly MainWindowViewModel _vm;
        private Point? _point;
        private readonly int _scaleFactor;
        public MainWindow()
        {
            InitializeComponent();
            _vm = DataContext as MainWindowViewModel;
            if (_vm is null) throw new NullReferenceException("VM");
            _scaleFactor = 1; //If the Wb is twice the size of the Image control, ScaleFactor should be 2.

            DispatcherTimer renderTimer = new(DispatcherPriority.Render);
            renderTimer.Interval = TimeSpan.FromMilliseconds(1);
            renderTimer.Tick += RenderTimer_Tick;
            renderTimer.Start();
        }

        private void RenderTimer_Tick(object? sender, EventArgs e)
        {
            if (!_point.HasValue)
            {
                //Use center of bitmap as target if not moused over
                _point = new(_vm.Width / 2d / _scaleFactor, _vm.Height / 2d / _scaleFactor);
            }

            _vm.Tick((int)_point.Value.X * _scaleFactor, (int)_point.Value.Y * _scaleFactor);

            Wb.Lock(); //https://docs.microsoft.com/en-us/dotnet/api/system.windows.media.imaging.writeablebitmap.lock?view=windowsdesktop-6.0
            Wb.Clear(Colors.CornflowerBlue);

            foreach (var ball in _vm.Balls)
            {
                //https://github.com/reneschulte/WriteableBitmapEx
                Wb.FillEllipseCentered((int)ball.Location.X, (int)ball.Location.Y, 5, 5, Color.FromArgb(255, ball.ColorData[0], ball.ColorData[1], ball.ColorData[2]));
            }
            Wb.Unlock();
        }

        private void Image_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _point = e.GetPosition(sender as Image);
        }

        private void Image_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _point = null;
        }
    }
}
