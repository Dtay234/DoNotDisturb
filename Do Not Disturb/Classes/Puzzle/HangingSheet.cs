using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;

namespace Do_Not_Disturb.Classes.Puzzle
{
    internal class HangingSheet : PuzzleElement
    {
        
        private List<LinkedList<Vector2>> points;

        public HangingSheet(Vector2 location, int length, int width) : base (location)
        {
            points = new();

            for (int i = 0; i < width; i++)
            {
                points.Add(new LinkedList<Vector2>());
                points[i].AddLast(new Vector2(location.X + (10 * i), location.Y));

                for (int j = 1; j < length; j++)
                {
                    points[i].AddLast(new Vector2(0, j * 20));
                }
            }
        }
    }
}
