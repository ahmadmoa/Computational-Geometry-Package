using CGUtilities;
using System.Collections.Generic;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremeSegments : Algorithm
    {
        public override void Run(
            List<Point> points,
            List<Line> lines,
            List<Polygon> polygons,
            ref List<Point> outPoints,
            ref List<Line> outLines,
            ref List<Polygon> outPolygons)
        {
            int numberOfPoints = points.Count;

            for (int y = 0; y < numberOfPoints; y++)
            {
                for (int x = 0; x < numberOfPoints; x++)
                {
                    bool flag1 = true;
                    bool flag2 = true;

                    for (int i = 0; i < numberOfPoints; i++)
                    {
                        if (i == y || i == x)
                            continue;

                        Point a = points[y].Vector(points[x]);
                        Point b = points[x].Vector(points[i]);
                        Enums.TurnType turn = HelperMethods.CheckTurn(a, b);

                        if (turn != Enums.TurnType.Left && !HelperMethods.PointOnSegment(points[i], points[y], points[x]))
                            flag1 = false;

                        if (turn != Enums.TurnType.Right && !HelperMethods.PointOnSegment(points[i], points[y], points[x]))
                            flag2 = false;
                    }

                    if (flag1 || flag2)
                    {
                        AddToOutputPoints(outPoints, points[y]);
                        AddToOutputPoints(outPoints, points[x]);
                    }
                }
            }
        }

        private void AddToOutputPoints(List<Point> outPoints, Point point)
        {
            if (!outPoints.Contains(point))
                outPoints.Add(point);
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Segments";
        }
    }
}
