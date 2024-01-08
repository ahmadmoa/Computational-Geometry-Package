using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class QuickHull : Algorithm
    {
        private Dictionary<string, Point> pointsMap = new Dictionary<string, Point>();
        private List<Point> outPoints = new List<Point>();
        private List<Line> outLines = new List<Line>();
        private List<Polygon> outPolygons = new List<Polygon>();

        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count <= 3)
            {
                outPoints = points;

                return;
            }

            points = points.OrderBy(p => p.Y).ToList();

            var left = 0;
            var right = points.Count - 1;

            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].X < points[left].X)
                {
                    left = i;
                }

                if (points[i].X >= points[right].X)
                {
                    right = i;
                }
            }

            var up = new List<Point>();
            var down = new List<Point>();

            for (int i = 0; i < points.Count; i++)
            {
                if (i == left || i == right)
                {
                    continue;
                }

                var result = CrossProduct(points[left], points[right], points[i]);
                if (result == Enums.TurnType.Left)
                {
                    down.Add(points[i]);
                }
                else if (result == Enums.TurnType.Right)
                {
                    up.Add(points[i]);
                }
            }

            findPoints(points[left], points[right], up);
            findPoints(points[left], points[right], down);

            foreach (var t in pointsMap)
            {
                outPoints.Add(t.Value);
            }
            outLines = this.outLines;
            outPolygons = this.outPolygons;
        }

        private void findPoints(Point a, Point b, List<Point> points)
        {
            if (points.Count == 0)
            {
                if (!pointsMap.ContainsKey($"{a.X}-{a.Y}"))
                {
                    pointsMap.Add($"{a.X}-{a.Y}", a);
                }

                if (!pointsMap.ContainsKey($"{b.X}-{b.Y}"))
                {
                    pointsMap.Add($"{b.X}-{b.Y}", b);
                }
                // outPoints.Add(a);
                // outPoints.Add(b);

                outLines.Add(new Line(a, b));

                return;
            }

            double max = -1;
            int index = 0;

            for (int i = 0; i < points.Count; i++)
            {
                double d = CalculateDistance(points[i], a, b);
                if (d > max)
                {
                    index = i;
                    max = d;
                }
            }

            var left = new List<Point>();
            var right = new List<Point>();

            for (int i = 0; i < points.Count; i++)
            {
                if (i == index)
                {
                    continue;
                }

                Enums.PointInPolygon temp = HelperMethods.PointInTriangle(points[i], a, b, points[index]);

                if (temp != Enums.PointInPolygon.Outside)
                {
                    continue;
                }

                if (points[i].X < points[index].X)
                {
                    left.Add(points[i]);
                }
                else
                {
                    right.Add(points[i]);
                }
            }

            findPoints(a, points[index], left);
            findPoints(points[index], b, right);


            // findPoints()
        }

        private Enums.TurnType CrossProduct(Point p1, Point p2, Point p)
        {
            var result = (p.X - p1.X) * (p2.Y - p1.Y) - (p.Y - p1.Y) * (p2.X - p1.X);

            if (result < 0) return Enums.TurnType.Right;
            else if (result > 0) return Enums.TurnType.Left;
            else return Enums.TurnType.Colinear;
        }

        public static double CalculateDistance(Point point, Point p1, Point p2)
        {
            double x1 = p1.X;
            double y1 = p1.Y;
            double x2 = p2.X;
            double y2 = p2.Y;
            double x0 = point.X;
            double y0 = point.Y;

            return Math.Abs(
                (y2 - y1) * x0 - (x2 - x1) * y0 + x2 * y1 - y2 * x1
            ) / Math.Sqrt(
                    Math.Pow(y2 - y1, 2) + Math.Pow(x2 - x1, 2)
            );
        }

        public override string ToString()
        {
            return "Convex Hull - Quick Hull";
        }
    }
}