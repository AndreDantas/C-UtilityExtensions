namespace UtilityExtensions.Core.Geometry
{
    public struct Line
    {
        public Vector2 start;

        public Vector2 end;

        /// <summary>
        /// Returns a point that belongs to the line. When t = 0, returns the start point and t = 1
        /// returns the end point
        /// </summary>
        /// <param name="by"> </param>
        /// <returns> </returns>
        public Vector2 GetPoint(Float01 by)
        {
            return GeometryExtensions.Lerp(start, end, by);
        }

        public Line(Vector2 start, Vector2 end) : this()
        {
            this.start = start;
            this.end = end;
        }

        public static Line[] FormLinesWithPoints(Vector2[] points)
        {
            if (points == null || points.Length < 2)
            {
                return null;
            }

            Line[] lines = new Line[points.Length - 1];

            for (int i = 0; i < points.Length - 1; i++)
            {
                lines[i] = new Line(points[i], points[i + 1]);
            }

            return lines;
        }
    }
}