using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;

namespace Do_Not_Disturb.Classes.Puzzle
{
    internal class PuzzleElement
    {
        protected Vector2 location;
        public PuzzleElement(Vector2 location)
        {
            this.location = location;
        }
    }
}
