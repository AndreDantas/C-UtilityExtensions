using System;

namespace UtilityExtensions.Core.Services
{
    public abstract class BackgroundService : IService
    {
        public TimeSpan interval;
        public bool enabled;

        protected BackgroundService(TimeSpan interval)
        {
            if (interval.TotalSeconds < 0)
            {
                throw new ArgumentException("The interval can't be negative", nameof(interval));
            }

            this.interval = interval;
        }

        public abstract void Execute();
    }
}