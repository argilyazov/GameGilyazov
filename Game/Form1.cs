using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;
using Game.Model;
using Vector = Game.Model.Vector;

namespace Game
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            DoubleBuffered = true;
            var map = new Map();
            var roadImage = new Bitmap(map.RoadImage);
            var angleImage1 = new Bitmap(map.AngleImage1);
            var angleImage2 = new Bitmap(map.AngleImage2);
            var angleImage3 = new Bitmap(map.AngleImage3);
            var angleImage4 = new Bitmap(map.AngleImage4);
            var spaceSize = new Size(1900, 1000);
            var car = new Car();
            var backgroundImage = new Bitmap(@"D:\C#\Hello world!\Game\Текстуры\проект.png");
            var carImage = new Bitmap(car.Image);

            KeyDown += (sender, args) =>
            {
                switch (args.KeyValue)
                {
                    case (char) Keys.A:
                        if (car.Velocity.Length > 0)
                        {
                            car.Velocity *= car.Controllability;
                            car.MoveCar(Turn.Left, Car.GetThrustForce(0.2), spaceSize, 0);
                        }

                        break;
                    case (char) Keys.D:
                        if (car.Velocity.Length > 0)
                        {
                            car.Velocity *= car.Controllability;
                            car.MoveCar(Turn.Right, Car.GetThrustForce(0.2), spaceSize, 0);
                        }

                        break;
                    case (char) Keys.S:
                        car.Slowdown();
                        car.MoveCar(Turn.None, Car.GetThrustForce(0), spaceSize, 0);
                        break;
                    case (char) Keys.W:
                        car.SpeedUp();
                        car.MoveCar(Turn.None, Car.GetThrustForce(0), spaceSize, 0);
                        break;
                }
            };

            var time = 0;
            var timer = new Timer();
            timer.Interval = 10;
            timer.Tick += (sender, args) =>
            {
                time++;
                car.MoveCar(Turn.None, Car.GetThrustForce(0.2), spaceSize, 0);
                Invalidate();
            };
            timer.Start();

            Invalidate();
            Paint += (sender, args) =>
            {
                var graphics = args.Graphics;
                
                //args.Graphics.DrawImage(backgroundImage, 0, 0);
                var point = new Point(0, 0);
                foreach (var site in map.CharMap)
                {
                    if (site == 'R')
                    {
                        graphics.DrawImage(roadImage,point);
                        point = new Point(point.X+roadImage.Width,point.Y);
                    }
                    if (site == '1')
                    {
                        graphics.DrawImage(angleImage1,point);
                        point = new Point(0,point.Y+angleImage1.Height);
                    }
                    if (site == '2')
                    {
                        graphics.DrawImage(angleImage2,point);
                        point = new Point(point.X+angleImage2.Width,point.Y);
                    }
                    if (site == '3')
                    {
                        graphics.DrawImage(angleImage3,point);
                        point = new Point(0,point.Y+angleImage3.Height);
                    }
                    if (site == '4')
                    {
                        graphics.DrawImage(angleImage4,point);
                        point = new Point(point.X+angleImage4.Width,point.Y);
                    }
                }
                graphics.TranslateTransform((float) car.Location.X, (float) car.Location.Y);
                graphics.RotateTransform((float) (car.Direction * 57.2958));
                graphics.DrawImage(carImage, -carImage.Width / 2, -carImage.Height / 2);
            };
            InitializeComponent();
        }
    }
}