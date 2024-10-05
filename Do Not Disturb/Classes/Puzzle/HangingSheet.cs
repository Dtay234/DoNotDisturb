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

        public HangingSheet(Vector2 location) : base (location)
        {
            points = new();

        }
    }
}
