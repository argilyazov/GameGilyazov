using System;
using System.Drawing;
using System.Numerics;

namespace Game.Model
{
    public class Car
    {
        public readonly string Image;

        public Vector Location;

        public double Direction;

        public Vector Velocity;
        
        public readonly double Controllability;

        private double currentVelocity;

        private readonly double mass;
        private readonly double maxVelocity;
        private readonly double maxTurnRate;
        private readonly double dragCoef;
        private readonly double boostCoef;

        public Car() : this(500, 500, 0, 1.0, 2.0, 0.05, 10, 5, 0.5)
        {
        }

        public Car(int x, int y, double rotate, double mass, double maxVelocity, double maxTurnRate, double drag,
            double boost, double controll)
        {
            Location = new Vector(x, y);
            Velocity = new Vector(0, 0);
            Direction = 0;
            this.mass = mass;
            this.maxVelocity = maxVelocity;
            this.maxTurnRate = maxTurnRate;
            dragCoef = drag;
            boostCoef = boost;
            Controllability = controll;
            Image = @"D:\C#\Hello world!\Game\Текстуры\машина.png";
        }

        public static CarForce GetThrustForce(double forceValue)
        {
            return r => new Vector(Math.Cos(r.Direction) * forceValue, Math.Sin(r.Direction) * forceValue);
        }

        public void MoveCar(Turn turn, CarForce force, Size spaceSize, double dt)
        {
            var turnRate = turn == Turn.Left ? -maxTurnRate : turn == Turn.Right ? maxTurnRate : 0;
            var dir = Direction + turnRate;
            var velocity = Velocity + force(this);
            if (velocity.Length > currentVelocity) velocity = velocity.Normalize() * currentVelocity;
            var location = Location + velocity;
            if (location.X < 0) velocity = new Vector(Math.Max(0, velocity.X), velocity.Y);
            if (location.X > spaceSize.Width) velocity = new Vector(Math.Min(0, velocity.X), velocity.Y);
            if (location.Y < 0) velocity = new Vector(velocity.X, Math.Max(0, velocity.Y));
            if (location.Y > spaceSize.Height) velocity = new Vector(velocity.X, Math.Min(0, velocity.Y));

            Location = location.BoundTo(spaceSize);
            Velocity = velocity;
            Direction = dir;
        }

        public void Slowdown()
        {
            if (currentVelocity > 0) currentVelocity -= maxVelocity / dragCoef;
            else currentVelocity = 0;
        }

        public void SpeedUp()
        {
            if (currentVelocity < maxVelocity) currentVelocity += maxVelocity / boostCoef;
            else currentVelocity = maxVelocity;
        }
    }
}