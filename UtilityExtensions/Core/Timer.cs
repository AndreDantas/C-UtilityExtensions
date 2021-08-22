using System;
using UtilityExtensions.Extensions;

namespace UtilityExtensions.Core
{
    public class Timer
    {
        private double _time;
        private double _counter;

        public double Counter { get => _counter; private set => _counter = MathExtensions.Clamp(value, 0, Time); }

        public bool IsFinished { get; private set; }

        public double Time
        {
            get => _time;

            set => _time = Math.Max(value, 0);
        }

        private Timer()
        {
        }

        public Timer(double time) : this()
        {
            Time = time;
            Counter = 0;
            IsFinished = false;
        }

        public bool Update(double step)
        {
            if (IsFinished)
            {
                Counter = Time;
                return true;
            }
            Counter += step;
            if (Counter >= Time)
            {
                IsFinished = true;
                Counter = Time;
            }
            else
            {
                IsFinished = false;
            }

            return IsFinished;
        }

        public bool Reverse(double step)
        {
            if (IsFinished)
            {
                IsFinished = false;
            }

            Counter -= step;

            return Counter.CloseTo(0f);
        }

        public void Reset()
        {
            Counter = 0f;
            IsFinished = false;
        }

        public void Finish()
        {
            Counter = Time;
            IsFinished = true;
        }
    }
}