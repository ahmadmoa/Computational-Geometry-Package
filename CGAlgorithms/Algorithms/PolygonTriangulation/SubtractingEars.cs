using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.PolygonTriangulation
{
    class SubtractingEars : Algorithm
    {


        private bool IsConvex(CGUtilities.Point a, CGUtilities.Point b, CGUtilities.Point c, List<CGUtilities.Point> polygon)
        {
            // Calculate vectors AB and BC
            CGUtilities.Point AB = new CGUtilities.Point(b.X - a.X, b.Y - a.Y);
            CGUtilities.Point BC = new CGUtilities.Point(c.X - b.X, c.Y - b.Y);

            // Calculate the cross product to determine the angle
            double crossProduct = AB.X * BC.Y - AB.Y * BC.X;

            // If the cross product is positive, the angle is convex; if it's negative, it's reflex
            return crossProduct >= 0; // Consider colinear as convex or change condition based on your requirements
        }

        private bool IsEar(CGUtilities.Point v1, CGUtilities.Point v, CGUtilities.Point v2, List<CGUtilities.Point> polygon)
        {
            for (int i = 0; i < polygon.Count; i++)
            {
                if (polygon[i] != v1 && polygon[i] != v && polygon[i] != v2)
                {
                    double area = CalculateTriangleArea(v1, v, v2);

                    double area1 = CalculateTriangleArea(v1, v, polygon[i]);
                    double area2 = CalculateTriangleArea(v, v2, polygon[i]);
                    double area3 = CalculateTriangleArea(v2, v1, polygon[i]);

                    // If the sum of areas of the three triangles equals the area of the main triangle, the point is inside
                    if (Math.Abs(area - (area1 + area2 + area3)) < 1e-9) // Adjust epsilon for floating-point comparison
                    {
                        return false; // Triangle contains another point, segment is not entirely in the interior
                    }
                }
            }

            return true; // Segment is entirely in the interior, hence an ear
        }

        private double CalculateTriangleArea(CGUtilities.Point a, CGUtilities.Point b, CGUtilities.Point c)
        {
            return Math.Abs((a.X * (b.Y - c.Y) + b.X * (c.Y - a.Y) + c.X * (a.Y - b.Y)) / 2.0);
        }

        public override void Run(List<CGUtilities.Point> points, List<CGUtilities.Line> lines, List<CGUtilities.Polygon> polygons, ref List<CGUtilities.Point> outPoints, ref List<CGUtilities.Line> outLines, ref List<CGUtilities.Polygon> outPolygons)
        {
            Console.WriteLine(points.Count);
            // Loop through each polygon
            for (int i = 0; i < polygons.Count; i++)
            {
                // Add the polygon's lines' start points to the list of points
                for (int j = 0; j < polygons[i].lines.Count; j++)
                {
                    points.Add(polygons[i].lines[j].Start);
                }
                Console.WriteLine(points.Count);
            }

            if (points.Count <= 3)
            {
                //outPoints = points;
                return;
            }

            List<CGUtilities.Point> vertices = new List<CGUtilities.Point>(points); // Copy the points to represent the vertices

            while (vertices.Count > 3) // Triangulate until only 3 vertices (triangle) remain
            {
                bool foundEar = false;

                for (int i = 0; i < vertices.Count; i++)
                {
                    int prev = (i - 1 + vertices.Count) % vertices.Count;
                    int next = (i + 1) % vertices.Count;

                    if (!IsConvex(vertices[prev], vertices[i], vertices[next], vertices) &&
                        IsEar(vertices[prev], vertices[i], vertices[next], vertices))
                    {
                        foundEar = true;

                        // Add the lines forming the ear to outLines
                        outLines.Add(new CGUtilities.Line(vertices[prev], vertices[next]));
                        Console.WriteLine(outLines);

                        vertices.RemoveAt(i);
                        break;

                    }
                }


                if (!foundEar)
                {
                    break;
                }
            }
        }

        public override string ToString()
        {
            return "Subtracting Ears";
        }
    }
}