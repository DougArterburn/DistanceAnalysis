using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASignInSpace
{

    public class Distances
    {
        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public class DistanceClass
        {
            public int Distance
            {
                get;
                set;
            }

            public int Count  //Pairs of points
            {
                get;
                set;
            }

            public List<Point> PointList
            {
                get;
                set;
            } = new List<Point>();

        }


        public void Run(BitArray gridIn)
        {
            int x = 0, y = 0;
            List<int> distances = new List<int>();
            List<Point> allPoints = new List<Point>();
            List<Point> p1List = new List<Point>();
            List<Point> p2List = new List<Point>();

            var Grid = BitArrayHelper.CreateBoolArray(gridIn, Width, Height);

            for (y = 0; y < Height; y++)
            {
                for (x = 0; x < Width; x++)
                {
                    if (Grid[x, y])
                    {
                        allPoints.Add(new Point(x, y));
                    }
                }
            }

            var allPointsArray = allPoints.ToArray();
            SortedDictionary<int, DistanceClass> AllDistanceCounts = new SortedDictionary<int, DistanceClass>();

            for (int i = 0; i < allPointsArray.Length; i++)
            {
                var p1 = allPointsArray[i];
                for (int j = i + 1; j < allPointsArray.Length; j++)
                {
                    var p2 = allPointsArray[j];
                    int xDistance = Math.Abs(p1.X - p2.X);
                    int yDistance = Math.Abs(p1.Y - p2.Y);
                    int d = xDistance * xDistance + yDistance * yDistance;
                    if (!AllDistanceCounts.ContainsKey(d)) AllDistanceCounts.Add(d, new DistanceClass());
                    AllDistanceCounts[d].Distance = d;
                    AllDistanceCounts[d].Count++;
                    AllDistanceCounts[d].PointList.Add(p1);
                    AllDistanceCounts[d].PointList.Add(p2);
                }
            }


            Draw draw = new Draw();
            draw.Width = Width;
            draw.Height = Height;
            draw.FilenamePrefix = "Distance";
            foreach (var p in AllDistanceCounts)
            {

                /*
                 * 
                 * Manually change the condition. There are a total of 12230 unique distances in the starmap, 
                 * so it is a impractical to look at all the graphs at once.
                 * 
                 */
                //int[] matchList = new int[] { 3, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
                if (p.Value.Count == (int) Math.Sqrt(p.Value.Distance))
                {
                    draw.DrawLinesPoints = new Point[p.Value.PointList.Count];
                    //draw.PlotAddressList = new Point[p.Value.PointList.Count];
                    draw.PlotAddressList = new Point[p.Value.PointList.Count];
                    for (int i = 0; i < p.Value.PointList.Count; i++)
                    {
                        //draw.DrawLinesPoints[i] = p.Value.PointList[i];                      
                        draw.PlotAddressList[i] = p.Value.PointList[i];
                    }
                    draw.FilenamePrefix = $"distance{p.Value.Distance:D5}-{p.Value.PointList.Count:D3}";
                    draw.DrawBitArray(new BitArray(Width * Height));
                    //draw.DrawBitArray(BitArrayHelper.GetStarmap());
                }
            }




        }
    }
}
