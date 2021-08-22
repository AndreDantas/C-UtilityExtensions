using System;
using System.Collections.Generic;

namespace UtilityExtensions.Core
{
    public struct Decimal01
    {
        private decimal value;

        public decimal Value { get => value; set => this.value = Math.Max(Math.Min(value, 1m), 0m); }

        public Decimal01(decimal value)
        {
            this.value = Math.Max(Math.Min(value, 1m), 0m);
        }

        public static implicit operator decimal(Decimal01 f)
        {
            return f.Value;
        }

        public static implicit operator Decimal01(decimal f)
        {
            return new Decimal01(f);
        }

        public static Decimal01 operator +(Decimal01 d1, Decimal01 d2)
        {
            return new Decimal01(d1.Value + d2.Value);
        }

        public static Decimal01 operator +(Decimal01 d1, decimal d2)
        {
            return new Decimal01(d1.Value + d2);
        }

        public static decimal operator +(decimal d1, Decimal01 d2)
        {
            return d1 + d2.Value;
        }

        public static Decimal01 operator -(Decimal01 d1, Decimal01 d2)
        {
            return new Decimal01(d1.Value - d2.Value);
        }

        public static Decimal01 operator -(Decimal01 d1, decimal d2)
        {
            return new Decimal01(d1.Value - d2);
        }

        public static decimal operator -(decimal d1, Decimal01 d2)
        {
            return d1 - d2.Value;
        }

        /// <summary>
        /// Increases the value by 0.1.
        /// </summary>
        public static Decimal01 operator ++(Decimal01 d1)
        {
            return new Decimal01(d1.Value += 0.1m);
        }

        /// <summary>
        /// Decreases the value by 0.1.
        /// </summary>
        public static Decimal01 operator --(Decimal01 d1)
        {
            return new Decimal01(d1.Value -= 0.1m);
        }

        public static Decimal01 operator *(Decimal01 d1, Decimal01 d2)
        {
            return new Decimal01(d1.Value * d2.Value);
        }

        public static Decimal01 operator /(Decimal01 d1, Decimal01 d2)
        {
            return new Decimal01(d1.Value / d2.Value);
        }

        public static bool operator ==(Decimal01 d1, Decimal01 d2)
        {
            return EqualityComparer<Decimal01>.Default.Equals(d1, d2);
        }

        public static bool operator !=(Decimal01 d1, Decimal01 d2)
        {
            return !(d1 == d2);
        }

        public static bool operator >(Decimal01 d1, Decimal01 d2)
        {
            return d1.Value > d2.Value;
        }

        public static bool operator <(Decimal01 d1, Decimal01 d2)
        {
            return d1.Value < d2.Value;
        }

        public static bool operator >=(Decimal01 d1, Decimal01 d2)
        {
            return d1.Value >= d2.Value;
        }

        public static bool operator <=(Decimal01 d1, Decimal01 d2)
        {
            return d1.Value <= d2.Value;
        }

        public static bool operator true(Decimal01 d1)
        {
            return d1.Value <= 1 && d1.Value > 0;
        }

        public static bool operator false(Decimal01 d1)
        {
            return d1.Value == 0;
        }

        public override bool Equals(object obj)
        {
            Decimal01 @decimal = (Decimal01)obj;
            return Value == @decimal.Value;
        }

        public override int GetHashCode()
        {
            return -1937169414 + Value.GetHashCode();
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }
}