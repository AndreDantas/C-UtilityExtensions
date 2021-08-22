using System;
using System.Globalization;
using UtilityExtensions.Extensions;

namespace UtilityExtensions.Core.Geometry
{
    public struct Vector2
    {
        public double x;

        public double y;

        public Vector2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 Zero => new Vector2();
        public static Vector2 Up => new Vector2(0, 1);
        public static Vector2 Down => new Vector2(0, -1);
        public static Vector2 Left => new Vector2(-1, 0);
        public static Vector2 Right => new Vector2(1, 0);

        /// <summary>
        /// Rotates one Vector2 around another
        /// </summary>
        /// <param name="pointToRotate"> The point to rotate. </param>
        /// <param name="centerPoint"> The center point of rotation. </param>
        /// <param name="angle"> The rotation angle. </param>
        /// <returns> Rotated point </returns>
        public static Vector2 RotateVector(Vector2 pointToRotate, Vector2 centerPoint, Angle angle)
        {
            double angleInRadians = angle.Rad;

            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);

            double originX = pointToRotate.x - centerPoint.x;
            double originY = pointToRotate.y - centerPoint.y;
            double newY, newX;

            if (angle >= 0)
            {
                newX = originX * cosTheta - originY * sinTheta;
                newY = originX * sinTheta + originY * cosTheta;
            }
            else
            {
                newX = originX * cosTheta + originY * sinTheta;
                newY = -originX * sinTheta + originY * cosTheta;
            }

            return new Vector2(newX + centerPoint.x, newY + centerPoint.y);
        }

        public static implicit operator Position(Vector2 v)
        {
            return new Position((int)v.x, (int)v.y);
        }

        public static implicit operator Vector2(Position p)
        {
            return new Vector2(p.X, p.Y);
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x - v2.x, v1.y - v2.y);
        }

        public static Vector2 operator *(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x * v2.x, v1.y * v2.y);
        }

        public static Vector2 operator /(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x / v2.x, v1.y / v2.y);
        }

        public static Vector2 operator *(Vector2 v1, double n)
        {
            return new Vector2(v1.x * n, v1.y * n);
        }

        public static Vector2 operator /(Vector2 v1, double n)
        {
            return new Vector2(v1.x / n, v1.y / n);
        }

        public static Vector2 operator *(Vector2 v1, decimal n)
        {
            return new Vector2(v1.x * (double)n, v1.y * (double)n);
        }

        public static Vector2 operator /(Vector2 v1, decimal n)
        {
            return new Vector2(v1.x / (double)n, v1.y / (double)n);
        }

        public static Vector2 operator *(Vector2 v1, long n)
        {
            return new Vector2(v1.x * n, v1.y * n);
        }

        public static Vector2 operator /(Vector2 v1, long n)
        {
            return new Vector2(v1.x / n, v1.y / n);
        }

        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            return !(v1 == v2);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "({0}, {1})", x, y);
        }

        public override int GetHashCode()
        {
            int hashCode = -1359342342;
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            return obj is Vector2 vector &&
                   x.CloseTo(vector.x) &&
                   y.CloseTo(vector.y);
        }
    }
}