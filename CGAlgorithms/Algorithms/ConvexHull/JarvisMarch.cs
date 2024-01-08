using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class JarvisMarch : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            int numberOfPoints = points.Count();

            if (numberOfPoints <= 3)
            {
                outPoints.AddRange(points);
                return;
            }

            Point startingPoint = FindLeftmostPoint(points);
            do
            {
                outPoints.Add(startingPoint);
                Point nextPoint = GetNextPoint(startingPoint, points);

                startingPoint = nextPoint;
            } while (startingPoint != points[FindLeftmostPointIndex(points)]);

        }
        private Point GetNextPoint(Point currentPoint, List<Point> points)
        {
            Random random = new Random();
            Point nextPoint = points[random.Next(points.Count)];

            for (int i = 0; i < points.Count; i++)
            {
                Line line = new Line(currentPoint, nextPoint);
                Enums.TurnType turnType = HelperMethods.CheckTurn(line, points[i]);

                if (turnType == Enums.TurnType.Left)
                    nextPoint = points[i];

                if (turnType == Enums.TurnType.Colinear && CalculateDistance(currentPoint, points[i]) > CalculateDistance(currentPoint, nextPoint))
                    nextPoint = points[i];
            }

            return nextPoint;
        }

        private int FindLeftmostPointIndex(List<Point> points)
        {
            int leftmostIndex = 0;

            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].X < points[leftmostIndex].X)
                    leftmostIndex = i;
            }

            return leftmostIndex;
        }

        private Point FindLeftmostPoint(List<Point> points)
        {
            return points[FindLeftmostPointIndex(points)];
        }

        private double CalculateDistance(Point a, Point b)
        {
            return Math.Sqrt((b.Y - a.Y) * (b.Y - a.Y) + (b.X - a.X) * (b.X - a.X));
        }

        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}