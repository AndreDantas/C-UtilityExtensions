using System;
using System.Collections.Generic;

namespace UtilityExtensions.Core
{
    public struct Float01
    {
        private float value;

        public float Value { get => value; set => this.value = Math.Max(Math.Min(value, 1f), 0f); }

        public Float01(float value)
        {
            this.value = Math.Max(Math.Min(value, 1f), 0f);
        }

        public static implicit operator float(Float01 f)
        {
            return f.Value;
        }

        public static implicit operator Float01(float f)
        {
            return new Float01(f);
        }

        public static Float01 operator +(Float01 f1, Float01 f2)
        {
            return new Float01(f1.Value + f2.Value);
        }

        public static Float01 operator +(Float01 f1, float f2)
        {
            return new Float01(f1.Value + f2);
        }

        public static float operator +(float f1, Float01 f2)
        {
            return f1 + f2.Value;
        }

        public static Float01 operator -(Float01 f1, Float01 f2)
        {
            return new Float01(f1.Value - f2.Value);
        }

        public static Float01 operator -(Float01 f1, float f2)
        {
            return new Float01(f1.Value - f2);
        }

        public static float operator -(float f1, Float01 f2)
        {
            return f1 - f2.Value;
        }

        /// <summary>
        /// Increases the value by 0.1.
        /// </summary>
        public static Float01 operator ++(Float01 f1)
        {
            return new Float01(f1.Value += 0.1f);
        }

        /// <summary>
        /// Decreases the value by 0.1.
        /// </summary>
        public static Float01 operator --(Float01 f1)
        {
            return new Float01(f1.Value -= 0.1f);
        }

        public static Float01 operator *(Float01 f1, Float01 f2)
        {
            return new Float01(f1.Value * f2.Value);
        }

        public static Float01 operator /(Float01 f1, Float01 f2)
        {
            return new Float01(f1.Value / f2.Value);
        }

        public static bool operator ==(Float01 f1, Float01 f2)
        {
            return EqualityComparer<Float01>.Default.Equals(f1, f2);
        }

        public static bool operator !=(Float01 f1, Float01 f2)
        {
            return !(f1 == f2);
        }

        public static bool operator >(Float01 f1, Float01 f2)
        {
            return f1.Value > f2.Value;
        }

        public static bool operator <(Float01 f1, Float01 f2)
        {
            return f1.Value < f2.Value;
        }

        public static bool operator >=(Float01 f1, Float01 f2)
        {
            return f1.Value >= f2.Value;
        }

        public static bool operator <=(Float01 f1, Float01 f2)
        {
            return f1.Value <= f2.Value;
        }

        public static bool operator true(Float01 f1)
        {
            return f1.Value <= 1 && f1.Value > 0;
        }

        public static bool operator false(Float01 f1)
        {
            return f1.Value == 0;
        }

        public override bool Equals(object obj)
        {
            Float01 @float = (Float01)obj;
            return Value == @float.Value;
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