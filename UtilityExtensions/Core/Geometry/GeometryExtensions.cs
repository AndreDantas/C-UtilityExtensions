using System;
using UtilityExtensions.Extensions;

namespace UtilityExtensions.Core.Geometry
{
    public static class GeometryExtensions
    {
        /// <summary>
        /// Returns the distance between two points.
        /// </summary>
        /// <param name="start"> </param>
        /// <param name="end"> </param>
        /// <returns> </returns>
        public static double Distance(Vector2 start, Vector2 end)
        {
            double deltaX = (end.x - start.x) * (end.x - start.x);
            double deltaY = (end.y - start.y) * (end.y - start.y);

            return Math.Sqrt(deltaX + deltaY);
        }

        public static double Distance(this Line l)
        {
            return Distance(l.start, l.end);
        }

        /// <summary>
        /// Checks if the two shapes are intersecting.
        /// </summary>
        /// <param name="a"> </param>
        /// <param name="b"> </param>
        /// <returns> </returns>
        public static bool IsShapes2DIntersecting(Shape2D a, Shape2D b)
        {
            if (a?.Points == null || b?.Points == null)
            {
                return false;
            }

            foreach (Shape2D shape in new[] { a, b })
            {
                Vector2[] points = shape.Points;
                for (int i1 = 0; i1 < points.Length; i1++)
                {
                    int i2 = (i1 + 1) % points.Length;
                    Vector2 p1 = points[i1];
                    Vector2 p2 = points[i2];

                    Vector2 normal = new Vector2(p2.y - p1.y, p1.x - p2.x);

                    double? minA = null, maxA = null;
                    foreach (Vector2 p in a.Points)
                    {
                        double projected = normal.x * p.x + normal.y * p.y;
                        if (minA == null || projected < minA)
                        {
                            minA = projected;
                        }

                        if (maxA == null || projected > maxA)
                        {
                            maxA = projected;
                        }
                    }

                    double? minB = null, maxB = null;
                    foreach (Vector2 p in b.Points)
                    {
                        double projected = normal.x * p.x + normal.y * p.y;
                        if (minB == null || projected < minB)
                        {
                            minB = projected;
                        }

                        if (maxB == null || projected > maxB)
                        {
                            maxB = projected;
                        }
                    }

                    if (maxA < minB || maxB < minA)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static Vector2 Lerp(Vector2 v1, Vector2 v2, double by)
        {
            double retx = MathExtensions.Lerp(v1.x, v2.x, by);
            double rety = MathExtensions.Lerp(v1.y, v2.y, by);

            return new Vector2(retx, rety);
        }

        public static Vector2[] GetBezierCurve(Vector2[] controlPoints, int numOfCurvePoints = 10)
        {
            if (controlPoints == null || controlPoints.Length <= 2)
            {
                return controlPoints;
            }

            ///Used to get each point of the curve
            Vector2 GetPoint(Vector2[] points, double by)
            {
                switch (points?.Length)
                {
                    case 0:
                        return default;

                    case 1:
                        return points[0];

                    case 2:
                        return Lerp(points[0], points[1], by);
                }

                Vector2[] newPoints = new Vector2[points.Length - 1];
                for (int i = 0; i < points.Length - 1; i++)
                {
                    Vector2 v1 = points[i];
                    Vector2 v2 = points[i + 1];

                    newPoints[i] = Lerp(v1, v2, by);
                }

                return GetPoint(newPoints, by);
            }

            double step = 1f / (numOfCurvePoints.ClampMin(2) - 1);
            Vector2[] curvePoints = new Vector2[numOfCurvePoints];

            for (int i = 0; i < numOfCurvePoints; i++)
            {
                double t = i * step;
                curvePoints[i] = GetPoint(controlPoints, t);
            }

            return curvePoints;
        }
    }
}