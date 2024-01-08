using System;
using System.Collections.Generic;
using System.Linq;
using CGUtilities.DataStructures;

namespace CGAlgorithms.Algorithms.SegmentIntersection
{
    class SweepLine : Algorithm
    {
        public override void Run(List<CGUtilities.Point> points, List<CGUtilities.Line> lines, List<CGUtilities.Polygon> polygons, ref List<CGUtilities.Point> outPoints, ref List<CGUtilities.Line> outLines, ref List<CGUtilities.Polygon> outPolygons)
        {
            var eventQueue = new OrderedSet<Event>(
                (a, b) => a.point.X == b.point.X ? a.point.Y.CompareTo(b.point.Y) : a.point.X.CompareTo(b.point.X));

            var sweepLineSet = new OrderedSet<CGUtilities.Line>(
                (a, b) => -a.Start.Y.CompareTo(b.Start.Y));

            for (int i = 0; i < lines.Count; i++)
            {
                eventQueue.Add(new Event(lines[i].Start, "StartPoint", i));
                eventQueue.Add(new Event(lines[i].End, "EndPoint", i));
            }
            for (int i = 0; i < eventQueue.Count(); i++)
            {
                Console.WriteLine(eventQueue[i].point.X);
            }
            List<int> l = new List<int>();
            PrintEventQueue(eventQueue);

            var intersectionCount = 0;

            while (eventQueue.Count != 0)
            {
                var currentEvent = eventQueue.First();

                Console.WriteLine(currentEvent.type);
                Console.WriteLine(currentEvent.point.X);

                if (currentEvent.type.Equals("StartPoint"))
                {
                    HandleStartPointEvent(currentEvent, lines, sweepLineSet, ref intersectionCount, ref eventQueue, ref outPoints);
                }
                else if (currentEvent.type.Equals("EndPoint"))
                {
                    HandleEndPointEvent(currentEvent, lines, sweepLineSet, ref intersectionCount, ref eventQueue, ref outPoints);
                }
                else if (currentEvent.type.Equals("IntersectionPoint"))
                {
                    HandleIntersectionPointEvent(currentEvent, lines, sweepLineSet, ref intersectionCount, ref eventQueue, ref outPoints);
                }

                eventQueue.RemoveFirst();
            }

            Console.WriteLine(outPoints.Count());
        }

        private static void PrintEventQueue(OrderedSet<Event> eventQueue)
        {
            foreach (var currentEvent in eventQueue)
            {
                Console.WriteLine(currentEvent.point.X);
            }
        }

        private void HandleStartPointEvent(Event currentEvent, List<CGUtilities.Line> lines, OrderedSet<CGUtilities.Line> sweepLineSet, ref int intersectionCount, ref OrderedSet<Event> eventQueue, ref List<CGUtilities.Point> outPoints)
        {
            var segment = lines[currentEvent.segment1];
            sweepLineSet.Add(segment);

            var upperAndLower = sweepLineSet.DirectUpperAndLower(segment);
            var upperSegment = upperAndLower.Key;
            var lowerSegment = upperAndLower.Value;

            CheckAndHandleIntersection(upperSegment, segment, ref intersectionCount, lines.IndexOf(upperSegment), lines.IndexOf(segment), ref eventQueue, ref outPoints);
            CheckAndHandleIntersection(segment, lowerSegment, ref intersectionCount, lines.IndexOf(segment), lines.IndexOf(lowerSegment), ref eventQueue, ref outPoints);
        }

        private static void HandleEndPointEvent(Event currentEvent, List<CGUtilities.Line> lines, OrderedSet<CGUtilities.Line> sweepLineSet, ref int intersectionCount, ref OrderedSet<Event> eventQueue, ref List<CGUtilities.Point> outPoints)
        {
            var segment = lines[currentEvent.segment1];
            var upperAndLower = sweepLineSet.DirectUpperAndLower(segment);
            var upperSegment = upperAndLower.Key;
            var lowerSegment = upperAndLower.Value;
            sweepLineSet.Remove(segment);

            CheckAndHandleIntersection(upperSegment, lowerSegment, ref intersectionCount, lines.IndexOf(upperSegment), lines.IndexOf(lowerSegment), ref eventQueue, ref outPoints);
        }


        private void HandleIntersectionPointEvent(Event currentEvent, List<CGUtilities.Line> lines, OrderedSet<CGUtilities.Line> sweepLineSet, ref int intersectionCount, ref OrderedSet<Event> eventQueue, ref List<CGUtilities.Point> outPoints)
        {
            outPoints.Add(currentEvent.point);
            var segment1 = lines[currentEvent.segment1];
            var segment2 = lines[currentEvent.segment2];

            var seg1UpperAndLower = sweepLineSet.DirectUpperAndLower(segment1);
            var upperSegment = seg1UpperAndLower.Key;
            var seg2UpperAndLower = sweepLineSet.DirectUpperAndLower(segment2);
            var lowerSegment = seg2UpperAndLower.Value;

            CheckAndHandleIntersection(upperSegment, segment2, ref intersectionCount, lines.IndexOf(upperSegment), lines.IndexOf(segment2), ref eventQueue, ref outPoints);
            CheckAndHandleIntersection(lowerSegment, segment1, ref intersectionCount, lines.IndexOf(lowerSegment), lines.IndexOf(segment1), ref eventQueue, ref outPoints);

            SwapAndAddToSweepLineSet(segment1, segment2, sweepLineSet);
        }

        private static void CheckAndHandleIntersection(CGUtilities.Line segment1, CGUtilities.Line segment2, ref int intersectionCount, int index1, int index2, ref OrderedSet<Event> eventQueue, ref List<CGUtilities.Point> outPoints)
        {
            if (segment1 != null && segment2 != null && CGUtilities.HelperMethods.Intersection(segment1, segment2))
            {
                intersectionCount++;
                var intersectionPoint = CGUtilities.HelperMethods.IntersectionPoint(segment1, segment2);
                HandleIntersectionPoint(intersectionPoint, index1, index2, ref eventQueue, ref outPoints);
            }
        }

        private void SwapAndAddToSweepLineSet(CGUtilities.Line segment1, CGUtilities.Line segment2, OrderedSet<CGUtilities.Line> sweepLineSet)
        {
            sweepLineSet.Remove(segment1);
            sweepLineSet.Remove(segment2);

            var swap = segment1;
            segment1 = segment2;
            segment2 = swap;

            sweepLineSet.Add(segment1);
            sweepLineSet.Add(segment2);
        }

        private static void HandleIntersectionPoint(CGUtilities.Point intersectionPoint, int index1, int index2, ref OrderedSet<Event> eventQueue, ref List<CGUtilities.Point> outPoints)
        {
            if (!outPoints.Contains(intersectionPoint))
            {
                eventQueue.Add(new Event(intersectionPoint, "IntersectionPoint", index1, index2));
                outPoints.Add(intersectionPoint);
            }
        }

        public class Event
        {
            public CGUtilities.Point point { get; set; }
            public int segment1 { get; set; }
            public int segment2 { get; set; }
            public string type { get; set; }

            public Event(CGUtilities.Point point_p, string type_t, int segment_s)
            {
                point = point_p;
                type = type_t;
                segment1 = segment_s;
            }

            public Event(CGUtilities.Point point_p, string type_t, int segment_s1, int segment_s2)
            {
                point = point_p;
                type = type_t;
                segment1 = segment_s1;
                segment2 = segment_s2;
            }
        }

        public override string ToString()
        {
            return "Sweep Line";
        }
    }
}
