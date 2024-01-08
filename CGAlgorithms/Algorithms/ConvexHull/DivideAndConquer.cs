using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class DivideAndConquer : Algorithm
    {
        private List<Point> Merge(List<Point> leftList, List<Point> rightList)
        {
            int rightMostIndexLeft = FindRightMostIndex(leftList);
            int leftMostIndexRight = FindLeftMostIndex(rightList);

            int uppera, upperb;
            FindUpperTangent(leftList, rightList, rightMostIndexLeft, leftMostIndexRight, out uppera, out upperb);

            int lowera, lowerb;
            FindLowerTangent(leftList, rightList, rightMostIndexLeft, leftMostIndexRight, out lowera, out lowerb);

            List<Point> result = CombineTangents(leftList, rightList, uppera, upperb, lowera, lowerb);

            return result;
        }

        private int FindRightMostIndex(List<Point> points)
        {
            int rightMostIndex = 0;

            for (int i = 1; i < points.Count; i++)
            {
                if (IsRightMost(points[i], points[rightMostIndex]))
                    rightMostIndex = i;
            }

            return rightMostIndex;
        }

        private bool IsRightMost(Point point, Point reference)
        {
            return point.X > reference.X || (point.X == reference.X && point.Y > reference.Y);
        }

        private int FindLeftMostIndex(List<Point> points)
        {
            int leftMostIndex = 0;

            for (int i = 1; i < points.Count; i++)
            {
                if (IsLeftMost(points[i], points[leftMostIndex]))
                    leftMostIndex = i;
            }

            return leftMostIndex;
        }

        private bool IsLeftMost(Point point, Point reference)
        {
            return point.X < reference.X || (point.X == reference.X && point.Y < reference.Y);
        }

        private void FindUpperTangent(List<Point> leftList, List<Point> rightList, int rightMostIndexLeft, int leftMostIndexRight, out int uppera, out int upperb)
        {
            int upperaCandidate = rightMostIndexLeft;
            int upperbCandidate = leftMostIndexRight;

            bool isFound = false;
            while (!isFound)
            {
                isFound = true;

                upperaCandidate = IsColinearRightTurn(new Line(rightList[upperbCandidate], leftList[upperaCandidate]),
                    leftList[(upperaCandidate + 1) % leftList.Count]) ? (upperaCandidate + 1) % leftList.Count : upperaCandidate;

                int i = upperbCandidate > 0 ? upperbCandidate - 1 : rightList.Count - 1;
                upperbCandidate = IsColinearLeftTurn(new Line(leftList[upperaCandidate], rightList[upperbCandidate]), rightList[i]) ?
                    (upperbCandidate > 0 ? upperbCandidate - 1 : rightList.Count - 1) : upperbCandidate;

                while (IsRightTurn(new Line(rightList[upperbCandidate], leftList[upperaCandidate]),
                    leftList[(upperaCandidate + 1) % leftList.Count]))
                {
                    upperaCandidate = (upperaCandidate + 1) % leftList.Count;
                    isFound = false;
                }

                while (IsLeftTurn(new Line(leftList[upperaCandidate], rightList[upperbCandidate]), rightList[i]))
                {
                    upperbCandidate = upperbCandidate > 0 ? upperbCandidate - 1 : rightList.Count - 1;
                    i = upperbCandidate > 0 ? upperbCandidate - 1 : rightList.Count - 1;
                    isFound = false;
                }
            }

            uppera = upperaCandidate;
            upperb = upperbCandidate;
        }

        private void FindLowerTangent(List<Point> leftList, List<Point> rightList, int rightMostIndexLeft, int leftMostIndexRight, out int lowera, out int lowerb)
        {
            int loweraCandidate = rightMostIndexLeft;
            int lowerbCandidate = leftMostIndexRight;

            bool isFound = false;
            while (!isFound)
            {
                isFound = true;

                int i = loweraCandidate > 0 ? loweraCandidate - 1 : leftList.Count - 1;
                loweraCandidate = IsColinearLeftTurn(new Line(rightList[lowerbCandidate], leftList[loweraCandidate]), leftList[i]) ?
                    (loweraCandidate > 0 ? loweraCandidate - 1 : leftList.Count - 1) : loweraCandidate;

                lowerbCandidate = IsColinearRightTurn(new Line(leftList[loweraCandidate], rightList[lowerbCandidate]),
                    rightList[(lowerbCandidate + 1) % rightList.Count]) ? (lowerbCandidate + 1) % rightList.Count : lowerbCandidate;

                while (IsLeftTurn(new Line(rightList[lowerbCandidate], leftList[loweraCandidate]), leftList[i]))
                {
                    loweraCandidate = loweraCandidate > 0 ? loweraCandidate - 1 : leftList.Count - 1;
                    i = loweraCandidate > 0 ? loweraCandidate - 1 : leftList.Count - 1;
                    isFound = false;
                }

                while (IsRightTurn(new Line(leftList[loweraCandidate], rightList[lowerbCandidate]), rightList[(lowerbCandidate + 1) % rightList.Count]))
                {
                    lowerbCandidate = (lowerbCandidate + 1) % rightList.Count;
                    isFound = false;
                }
            }

            lowera = loweraCandidate;
            lowerb = lowerbCandidate;
        }

        private bool IsColinearRightTurn(Line line, Point point)
        {
            return HelperMethods.CheckTurn(line, point) == Enums.TurnType.Colinear;
        }

        private bool IsColinearLeftTurn(Line line, Point point)
        {
            return HelperMethods.CheckTurn(line, point) == Enums.TurnType.Colinear;
        }

        private bool IsRightTurn(Line line, Point point)
        {
            return HelperMethods.CheckTurn(line, point) == Enums.TurnType.Right;
        }

        private bool IsLeftTurn(Line line, Point point)
        {
            return HelperMethods.CheckTurn(line, point) == Enums.TurnType.Left;
        }

        private List<Point> CombineTangents(List<Point> leftList, List<Point> rightList, int uppera, int upperb, int lowera, int lowerb)
        {
            List<Point> result = new List<Point>();

            int j = uppera;
            while (j != lowera)
            {
                result.Add(leftList[j]);
                j = (j + 1) % leftList.Count;
            }
            result.Add(leftList[lowera]);

            j = lowerb;
            while (j != upperb)
            {
                result.Add(rightList[j]);
                j = (j + 1) % rightList.Count;
            }
            result.Add(rightList[upperb]);

            return result;
        }

        private List<Point> DivideAndConquerRecursive(List<Point> inputPoints)
        {
            if (inputPoints.Count == 1) return inputPoints;

            int end = inputPoints.Count % 2 == 0 ? inputPoints.Count / 2 : inputPoints.Count / 2 + 1;
            return Merge(
                DivideAndConquerRecursive(inputPoints.GetRange(0, inputPoints.Count / 2)),
                DivideAndConquerRecursive(inputPoints.GetRange(inputPoints.Count / 2, end))
            );
        }

        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count == 1)
            {
                outPoints = points;
                return;
            }

            List<Point> sortedPoints = points.OrderBy(x => x.X).ThenBy(x => x.Y).ToList();
            List<Point> convexHull = DivideAndConquerRecursive(sortedPoints);

            foreach (var point in convexHull)
            {
                if (!outPoints.Contains(point))
                {
                    outPoints.Add(point);
                }
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Divide & Conquer";
        }
    }
}
