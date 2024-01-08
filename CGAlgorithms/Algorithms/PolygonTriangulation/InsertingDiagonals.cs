using System;
using System.Collections.Generic;
using System.Linq;
using CGUtilities;

namespace CGAlgorithms.Algorithms.PolygonTriangulation
{
    class InsertingDiagonals : Algorithm
    {
        public override void Run(List<CGUtilities.Point> points, List<CGUtilities.Line> lines, List<CGUtilities.Polygon> polygons, ref List<CGUtilities.Point> outPoints, ref List<CGUtilities.Line> outLines, ref List<CGUtilities.Polygon> outPolygons)
        {
            // Ensure polygon orientation is CCW
            lines = PolygoneOrientation(lines);

            // Triangulate the polygon
            outLines = DCInsertingDiagonals(lines);
            Console.WriteLine("Number of Diagonals: " + outLines.Count);
        }

        private List<Line> PolygoneOrientation(List<Line> lines)
        {
            double windingNumber = 0;

            // Calculate the winding number to determine orientation
            foreach (Line line in lines)
            {
                windingNumber += (line.End.X - line.Start.X) * (line.End.Y + line.Start.Y);
            }

            // Ensure CCW orientation
            if (windingNumber > 0)
            {
                lines.Reverse();

                // Swap start and end points for each line
                foreach (Line line in lines)
                {
                    Point replace = line.Start;
                    line.Start = line.End;
                    line.End = replace;
                }
            }

            return lines;
        }


        public List<Line> DCInsertingDiagonals(List<Line> lines)
        {
            List<Line> outDiagonals = new List<Line>();

            if (lines.Count <= 3)
                return outDiagonals;

            // Extract vertices from lines
            List<Point> vertices = GetVertices(lines);

            // Find a diagonal to split the polygon
            Line diagonal = FindDiagonal(lines, vertices);

            if (diagonal != null)
            {
                Console.WriteLine("Diagonal Start X: " + diagonal.Start.X);

                // Divide the polygon into two subpolygons
                int v_d1 = vertices.IndexOf(diagonal.Start);
                int v_d2 = vertices.IndexOf(diagonal.End);

                List<Line> polyLines1 = ExtractSubPolygonLines(vertices, v_d1, v_d2);
                List<Line> polyLines2 = ExtractSubPolygonLines(vertices, v_d2, v_d1);

                // Recursively find diagonals for subpolygons
                List<Line> d1 = DCInsertingDiagonals(polyLines1);
                List<Line> d2 = DCInsertingDiagonals(polyLines2);

                // Combine diagonals
                d1.Add(diagonal);
                Console.WriteLine("Total Diagonals for Subpolygon: " + d1.Count);
                d1.AddRange(d2);

                return d1;
            }

            return outDiagonals;
        }

        // Check if three points form a convex turn
        public bool IsConvex(Point p, Point q, Point r)
        {
            if (HelperMethods.CheckTurn(new Line(p, q), r) == Enums.TurnType.Right)
                return true;
            return false;
        }

        private List<Point> GetVertices(List<Line> lines)
        {
            List<Point> vertices = new List<Point>();

            foreach (Line line in lines)
                vertices.Add(HelperMethods.GetVector(line));

            return vertices;
        }

        private Line FindDiagonal(List<Line> lines, List<Point> vertices)
        {
            int n = vertices.Count;

            for (int i = 0; i < n; i++)
            {
                Point a = vertices[i];

                for (int j = i + 2; j < n; j++)
                {
                    Point b = vertices[j];
                    Line diagonal = new Line(a, b);

                    int v_d1 = vertices.IndexOf(diagonal.Start);
                    int v_d2 = vertices.IndexOf(diagonal.End);

                    if (IsDiagonal(lines, v_d1, v_d2, diagonal))
                    {
                        Console.WriteLine("Diagonal Found: " + diagonal.Start.X + " -> " + diagonal.End.X);
                        return diagonal;
                    }
                }
            }

            Console.WriteLine("No Diagonal Found");
            return null;
        }


        private bool IsDiagonal(List<Line> lines, int v_d1, int v_d2, Line diagonal)
        {
            Console.WriteLine("Checking Diagonal: " + diagonal.Start.X + " -> " + diagonal.End.X);

            // Check if the segment is adjacent to one of its endpoints
            if (v_d1 == (v_d2 - 1 + lines.Count) % lines.Count || v_d2 == (v_d1 - 1 + lines.Count) % lines.Count)
            {
                Console.WriteLine("Diagonal is Adjacent to Endpoint");
                return false;
            }

            // Check if the segment intersects any of the other edges of the polygon
            foreach (Line line in lines)
            {
                Enums.TurnType startPointTurn = HelperMethods.CheckTurn(diagonal, line.Start);
                Enums.TurnType endPointTurn = HelperMethods.CheckTurn(diagonal, line.End);

                if (startPointTurn == Enums.TurnType.Colinear || endPointTurn == Enums.TurnType.Colinear)
                {
                    Console.WriteLine("Diagonal is Colinear with Another Edge");
                    return false;
                }

                // Check if the diagonal intersects with the current edge
                if (DoIntersect(diagonal.Start, diagonal.End, line.Start, line.End))
                {
                    Console.WriteLine("Diagonal Intersects with Another Edge");
                    return false;
                }
            }

            Console.WriteLine("Diagonal is Valid");
            return true;
        }

        // Function to check if two line segments intersect
        private bool DoIntersect(Point p1, Point q1, Point p2, Point q2)
        {
            Enums.TurnType turn1 = HelperMethods.CheckTurn(new Line(p1, q1), p2);
            Enums.TurnType turn2 = HelperMethods.CheckTurn(new Line(p1, q1), q2);
            Enums.TurnType turn3 = HelperMethods.CheckTurn(new Line(p2, q2), p1);
            Enums.TurnType turn4 = HelperMethods.CheckTurn(new Line(p2, q2), q1);

            return ((turn1 != turn2) && (turn3 != turn4));
        }


        private bool IsInternal(List<Line> lines, int v1, int v2)
        {
            int n = lines.Count;

            // Check if the vertices between v1 and v2 are all on the same side of the line (v1, v2)
            for (int i = 0; i < n; i++)
            {
                if (i != v1 && i != v2)
                {
                    Line line = lines[i];
                    Point c = line.Start;
                    Enums.TurnType turnType = HelperMethods.CheckTurn(lines[v1], lines[v2].End.Vector(c));

                    if (turnType == Enums.TurnType.Right)
                        return false;
                }
            }

            return true;
        }
        private List<Line> ExtractSubPolygonLines(List<Point> vertices, int startIndex, int endIndex)
        {
            List<Line> polygonLines = new List<Line>();
            int n = vertices.Count;
            int i = startIndex;

            do
            {
                int nextIndex = (i + 1) % n;
                polygonLines.Add(new Line(vertices[i], vertices[nextIndex]));
                i = nextIndex;
            } while (i != endIndex);

            return polygonLines;
        }
        public override string ToString()
        {
            return "Inserting Diagonals";
        }
    }
}
