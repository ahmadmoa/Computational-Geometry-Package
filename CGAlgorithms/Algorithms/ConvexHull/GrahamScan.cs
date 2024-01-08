using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class GrahamScan : Algorithm
    {
        // worked for polygon special case
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            Console.WriteLine("2145");
            Console.WriteLine("# List of Point #");
            foreach (Point value in points)
            {
                Console.Write("(" + value.X + "," + value.Y + ") ");
            }
            Console.WriteLine();
            Console.WriteLine();
            var bol1 = points.Count <= 3;
            var bol2 = AreCollinear(points);
            if (bol2 == true) Console.WriteLine("bol2 is true");
            else Console.WriteLine("bol2 is false");
            Console.WriteLine("points.Count <= 3 " + bol1);
            Console.WriteLine("AreCollinear(points) ", bol2);
            if (points.Count <= 3 || AreCollinear(points))
            {
                Console.WriteLine("ok1");
                outPoints = points.Distinct().ToList();
                return;
            }

            Point p0 = points.OrderBy(p => p.Y).ThenBy(p => p.X).First();
            List<Point> order = points.Where(p => p != p0)
                                      .OrderBy(p => getAngle(p0, p))
                                      .ThenBy(p => CalculateDistance(p0, p))
                                      .ToList();
            Console.WriteLine("# Sorted points based on angle with point p0 ({0},{1})#", p0.X, p0.Y);
            foreach (Point value in order)
            {
                Console.WriteLine($"({value.X},{value.Y}) : {getAngle(p0, value)}");
            }

            outPoints.Add(p0);
            outPoints.Add(order[0]);
            outPoints.Add(order[1]);
            order.RemoveAt(0);
            order.RemoveAt(0);

            Console.WriteLine("# Current Convex Hull #");
            DisplayPoints(outPoints);

            foreach (Point value in order)
            {
                keepLeft(outPoints, value);
            }

            Console.WriteLine();
            Console.WriteLine("# Convex Hull #");
            DisplayPoints(outPoints);
        }
        const int TURN_LEFT = 1;
     

        private double CalculateDistance(Point p1, Point p2)
        {
            double dx = p2.X - p1.X;
            double dy = p2.Y - p1.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
        private bool AreCollinear(List<Point> points)
        {
            if (points.Count < 3)
                return true;

            double slope = (points[1].Y - points[0].Y) / (points[1].X - points[0].X);
            for (int i = 2; i < points.Count; i++)
            {
                double currentSlope = (points[i].Y - points[0].Y) / (points[i].X - points[0].X);
                if (Math.Abs(currentSlope - slope) > double.Epsilon)
                {
                    return false;
                }
            }

            return true;
        }

        private void DisplayPoints(List<Point> points)
        {
            foreach (Point value in points)
            {
                Console.Write($"({value.X},{value.Y}) ");
            }
            Console.WriteLine();
        }
        public int turn(Point p, Point q, Point r)
        {
            return ((q.X - p.X) * (r.Y - p.Y) - (r.X - p.X) * (q.Y - p.Y)).CompareTo(0);
        }

        public void keepLeft(List<Point> hull, Point r)
        {
            while (hull.Count > 1 && turn(hull[hull.Count - 2], hull[hull.Count - 1], r) != TURN_LEFT)
            {
                Console.WriteLine("Removing Point ({0}, {1}) because turning right ", hull[hull.Count - 1].X, hull[hull.Count - 1].Y);
                hull.RemoveAt(hull.Count - 1);
            }
            if (hull.Count == 0 || hull[hull.Count - 1] != r)
            {
                Console.WriteLine("Adding Point ({0}, {1})", r.X, r.Y);
                hull.Add(r);
            }
            Console.WriteLine("# Current Convex Hull #");
            foreach (Point value in hull)
            {
                Console.Write("(" + value.X + "," + value.Y + ") ");
            }
            Console.WriteLine();
            Console.WriteLine();

        }

        public double getAngle(Point p1, Point p2)
        {
            double xDiff = p2.X - p1.X;
            double yDiff = p2.Y - p1.Y;
            return Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI;
        }

        public override string ToString()
        {
            return "Convex Hull - Graham Scan";
        }
    }
}
