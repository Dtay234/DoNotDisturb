using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Do_Not_Disturb.Classes
{
    internal class Level
    {
        private int windowHeight;
        private int windowWidth;
        private List<GameObject> objects;
        
        public Level(int windowHeight, int windowWidth)
        {
            this.windowHeight = windowHeight;
            this.windowWidth = windowWidth;
            this.objects = new List<GameObject>();

            new Geometry(new Rectangle(0, 0, 10, windowHeight), BlockTypes.NormalLongBlock);
            new Geometry(new Rectangle(windowWidth, 0, 10, windowHeight), BlockTypes.NormalLongBlock);
            new Geometry(new Rectangle(0, 0, windowWidth, 10), BlockTypes.NormalLongBlock);
            new Geometry(new Rectangle(0, windowWidth, windowWidth, 10), BlockTypes.NormalLongBlock);

        }

        

        
    }
}
