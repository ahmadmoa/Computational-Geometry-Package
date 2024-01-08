using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class Incremental : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count <= 3)
            {
                outPoints = points;
                return;
            }

            outPoints.Add(points[0]);
            outPoints.Add(points[1]);
            outPoints.Add(points[2]);

            points.RemoveAt(2);
            points.RemoveAt(1);
            points.RemoveAt(0);

            for (int i = 0; i < points.Count; i++)
            {
                var point = points[i];
                if (IsPointInPolygon(point, outPoints))
                {
                    points.RemoveAt(i--);
                    continue;
                }

                CheckForSegments(point, ref points, ref outPoints);
            }
        }

        private void CheckForSegments(Point point, ref List<Point> points, ref List<Point> outPoints)
        {
            List<Point> support = new List<Point>();

            for (int i = 0; i < outPoints.Count; i++)
            {
                var p1 = outPoints[i == 0 ? outPoints.Count - 1 : (i - 1) % outPoints.Count];
                var p = outPoints[i];
                var p2 = outPoints[(i + 1) % outPoints.Count];

                var left = HelperMethods.CheckTurn(new Line(p1, p), point);
                var right = HelperMethods.CheckTurn(new Line(p, p2), point);

                if (left == right)
                {
                    continue;
                }

                if (left == Enums.TurnType.Colinear)
                {
                    p = p1;
                }

                support.Add(p);
            }

            if (support.Count < 2)
            {
                return;
            }

            for (int i = 0; i < outPoints.Count; i++)
            {
                if (
                    HelperMethods.PointInTriangle(outPoints[i], point, support[0], support[1]) == Enums.PointInPolygon.Inside
                )
                {
                    outPoints.RemoveAt(i);
                    i = i == 0 ? 0 : i - 1;
                }
            }

            var dir = HelperMethods.CheckTurn(new Line(support[0], point), support[1]);

            if (dir == Enums.TurnType.Left)
            {
                var index = outPoints.IndexOf(support[1]);
                if (index == -1)
                {
                    index = 0;
                }
                outPoints.Insert(index, point);
            }
            else if (dir == Enums.TurnType.Right)
            {
                var index = outPoints.IndexOf(support[0]);
                if (index == -1)
                {
                    index = 0;
                }
                outPoints.Insert(index, point);
            }
        }

        public bool IsPointInPolygon(Point point, List<Point> polygon)
        {
            int count = 0;
            int n = polygon.Count;

            for (int i = 0, j = n - 1; i < n; j = i++)
            {
                Point pi = polygon[i];
                Point pj = polygon[j];

                // (pi.Y == point.Y && pi.X == point.X) || (pj.Y == point.Y && pj.X == point.X)
                if (HelperMethods.PointOnSegment(point, pi, pj))
                {
                    return true;
                }

                if (((pi.Y <= point.Y && point.Y < pj.Y) || (pj.Y <= point.Y && point.Y < pi.Y)) &&
                    (point.X < (pj.X - pi.X) * (point.Y - pi.Y) / (pj.Y - pi.Y) + pi.X))
                {
                    count++;
                }
            }

            return count % 2 == 1;
        }

        public override string ToString()
        {
            return "Convex Hull - Incremental";
        }
    }
}