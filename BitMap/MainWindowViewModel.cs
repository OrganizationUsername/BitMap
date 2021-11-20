using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BitMap
{
    public class MainWindowViewModel
    {
        public WriteableBitmap WB { get; set; } = new(300, 300, 96, 96, PixelFormats.Bgra32, null);
        public List<Ball> Balls { get; set; } = new();
        private Random _rand;
        public MainWindowViewModel()
        {
            _rand = new();
        }

        public void Tick()
        {
            if (_rand.NextDouble() > 0.75)
            {
                var x = _rand.Next(0, 300);
                var y = _rand.Next(0, 300);
                var rotation = (float)Math.Atan2(150 - y, 150 - x);
                Balls.Add(new() { Location = new(x, y), Rotation = rotation });
            }

            for (var index = 0; index < Balls.Count; index++)
            {
                var ball = Balls[index];
                var vector = new Vector2((float)Math.Cos(ball.Rotation), (float)Math.Sin(ball.Rotation));
                ball.Location += 10 * vector;
                if (ball.Location.X < 0 || ball.Location.X > 300 || ball.Location.Y < 0 || ball.Location.Y > 300)
                {
                    Balls.RemoveAt(index);
                }
            }
        }

    }

    public class Ball
    {
        public Vector2 Location { get; set; }
        public float Rotation { get; set; }
    }

}
