using CGUtilities;
using System;
using System.Collections.Generic;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremePoints : Algorithm
    {
        public override void Run(
            List<Point> points,
            List<Line> lines,
            List<Polygon> polygons,
            ref List<Point> outPoints,
            ref List<Line> outLines,
            ref List<Polygon> outPolygons)
        {
            outPoints = ComputeExtremePoints(points);
        }

        public List<Point> ComputeExtremePoints(List<Point> points)
        {
            List<Point> unique = new List<Point>();

            for (int i = 0; i < points.Count; i++)
            {
                bool found = false;
                for (int j = 0; j < unique.Count; j++)
                {
                    if (points[i].Equals(unique[j]))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    unique.Add(points[i]);
                }
            }

            points = unique;
            List<Point> extremePoints = new List<Point>();
            int n = points.Count;

            for (int i = 0; i < n; i++)
            {
                bool isExtreme = true;
                for (int j = 0; j < n; j++)
                {
                    for (int k = 0; k < n; k++)
                    {
                        for (int l = 0; l < n; l++)
                        {
                            if (i != j && i != k && i != l && j != k && j != l && k != l)
                            {
                                if (IsInsideTriangle(points[i], points[j], points[k], points[l]))
                                {
                                    isExtreme = false;
                                    break;
                                }
                            }
                        }
                        if (!isExtreme)
                            break;
                    }
                    if (!isExtreme)
                        break;
                }


                if (isExtreme)
                    extremePoints.Add(points[i]);
            }


            //extremePoints = FilterPointsOnSegments(extremePoints);

            return extremePoints;
        }

        private bool IsInsideTriangle(Point p, Point p1, Point p2, Point p3)
        {
            return !HelperMethods.PointInTriangle(p, p1, p2, p3).Equals(Enums.PointInPolygon.Outside);
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }
    }
}
