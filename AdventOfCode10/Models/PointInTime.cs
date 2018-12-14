using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode10.Models
{
    public class PointInTime
    {
        public int Second { get; set; }
        public int HighestTouching { get; set; }
        
        public int MinX { get; set; }

        public int MinY { get; set; }

        public int MaxX { get; set; }

        public int MaxY { get; set; }

        public PointInTime(int inSecond, int inHighestTouching, int minX, int miny, int maxx, int maxy)
        {
            Second = inSecond;
            HighestTouching = inHighestTouching;
            MinX = minX;
            MinY = miny;
            MaxX = maxx;
            MaxY = maxy;
        }

    }
}
