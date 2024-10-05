using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Do_Not_Disturb.Classes.Puzzle
{
    internal class BoxMovingPuzzle
    {
        Rectangle destination;
        Block box;
        bool complete;

        public bool Complete
        {
            get { return complete; }
        }

        public BoxMovingPuzzle(Rectangle destination, BlockTypes type)
        {
            this.destination = destination;

        }

        
        public static int SharedArea(Rectangle rect1, Rectangle rect2)
        {


            int length = Math.Min(rect1.X + rect1.Width + rect2.Width - rect2.X, rect2.Width);
            int width = Math.Min(rect1.Y + rect1.Height + rect2.Height - rect2.Y, rect2.Height);

            return length * width;
        }
    }
}
