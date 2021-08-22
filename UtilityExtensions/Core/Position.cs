using System;

namespace UtilityExtensions.Core
{
    public struct Position
    {
        public int X { get; init; }
        public int Y { get; init; }

        public Position(int x, int y)
        {
            (X, Y) = (x, y);
        }

        public void Deconstruct(out int x, out int y)
        {
            (x, y) = (X, Y);
        }

        public static readonly Position Up = new(0, 1);
        public static readonly Position Down = new(0, -1);
        public static readonly Position Left = new(-1, 0);
        public static readonly Position Right = new(1, 0);

        public static bool operator ==(Position a, Position b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Position a, Position b)
        {
            return !(a == b);
        }

        public static Position operator -(Position a)
        {
            return new Position(-a.X, -a.Y);
        }

        public static Position operator -(Position a, Position b)
        {
            return new Position(a.X - b.X, a.Y - b.Y);
        }

        public override bool Equals(Object obj)
        {
            return obj is Position pos && pos.X == X && pos.Y == Y;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}