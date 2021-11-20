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
        public MainWindow()
        {
            InitializeComponent();
            _vm = DataContext as MainWindowViewModel;
            if (_vm is null) throw new NullReferenceException("VM");

            DispatcherTimer renderTimer = new(DispatcherPriority.Render);
            renderTimer.Interval = TimeSpan.FromMilliseconds(10);
            renderTimer.Tick += RenderTimer_Tick;
            renderTimer.Start();
        }

        private void RenderTimer_Tick(object? sender, EventArgs e)
        {
            if (!_point.HasValue)
            {
                _point = new(_vm._width / 4d, _vm._height / 4d);
            }

            _vm.Tick((int)_point.Value.X * 2, (int)_point.Value.Y * 2);

            Wb.Lock();
            Wb.Clear(Colors.CornflowerBlue);

            foreach (var ball in _vm.Balls)
            {
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
