using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BitMap.Annotations;

namespace BitMap
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public WriteableBitmap WB { get; set; }
        public List<Ball> Balls { get; set; } = new();
        public int BallCount
        {
            get => Balls.Count;
            set => _ = value;
        }

        public int FramesPerSecond { get; set; }
        private DateTime _lastFrameUpdate;
        private readonly Random _rand;
        private int _framesSinceUpdate;
        private readonly int _width;
        private readonly int _dpi;
        private readonly int _height;

        public MainWindowViewModel()
        {
            _rand = new();
            _width = 1000;
            _height = 1000;
            _dpi = 96;
            WB = new(_width, _height, _dpi, _dpi, PixelFormats.Bgra32, null);
        }

        private void AddBall(int Number = 1)
        {
            for (int i = 0; i < Number; i++)
            {
                var x = _rand.Next(0, _width);
                var y = _rand.Next(0, _height);
                var rotation = (float)Math.Atan2((_height / 2) - y, (_width / 2) - x);
                Balls.Add(new() { Location = new(x, y), Rotation = rotation, ColorData = new byte[] { (byte)_rand.Next(0, 255), (byte)_rand.Next(0, 255), (byte)_rand.Next(0, 255) } });
            }
        }

        public void Tick()
        {
            if (DateTime.Now - _lastFrameUpdate > TimeSpan.FromSeconds(1))
            {
                FramesPerSecond = _framesSinceUpdate;
                OnPropertyChanged(nameof(FramesPerSecond));
                _framesSinceUpdate = 0;
                _lastFrameUpdate = DateTime.Now;
            }
            else
            {
                _framesSinceUpdate++;
            }

            if (_rand.NextDouble() > 0.25)
            {
                AddBall(50);
                OnPropertyChanged(nameof(BallCount));

            }

            for (var index = 0; index < Balls.Count; index++)
            {
                var ball = Balls[index];
                var vector = new Vector2((float)Math.Cos(ball.Rotation), (float)Math.Sin(ball.Rotation));
                ball.Location += 10 * vector;
                if (ball.Location.X < 0 || ball.Location.X > _width || ball.Location.Y < 0 || ball.Location.Y > _height)
                {
                    Balls.RemoveAt(index);
                    OnPropertyChanged(nameof(BallCount));

                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Ball
    {
        public Vector2 Location { get; set; }
        public float Rotation { get; set; }
        public byte[] ColorData;

    }

}
