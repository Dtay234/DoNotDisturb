using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Do_Not_Disturb.Classes.Puzzle;

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

            new Geometry(new Rectangle(0, 0, 10, windowHeight), BlockTypes.ARedBlock);
            new Geometry(new Rectangle(windowWidth, 0, 10, windowHeight), BlockTypes.ARedBlock);
            new Geometry(new Rectangle(0, 0, windowWidth, 10), BlockTypes.ARedBlock);
            new Geometry(new Rectangle(0, windowWidth, windowWidth, 10), BlockTypes.ARedBlock);
            
            objects.Add(new Block(new Vector2(500, 500), new Rectangle(0, 0, 100, 100), BlockTypes.ARedBlock));
            objects.Add(new Bubble(new Vector2(1000, 1950), new Rectangle(0, 0, 20, 20)));

        }

        public Level(String filename)
        {
            String[] allLines = File.ReadAllLines(filename);

            int width = 0;
            int height = 0;
            bool inBlock = true;

            foreach(String line in allLines)
            {
                string[] letters = line.Split(',');
                foreach(String chr in letters)
                {
                    
                }

            }

        }

        

        
    }
}
