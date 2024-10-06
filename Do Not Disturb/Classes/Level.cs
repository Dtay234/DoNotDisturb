using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Do_Not_Disturb.Classes.Puzzle;
using System.Data;
using System.Reflection;

namespace Do_Not_Disturb.Classes
{
    internal class Level
    {
        private int windowHeight;
        private int windowWidth;
        private List<GameObject> objects;
        private string filePath;
        private int levelNumber;

        public Level(int windowHeight, int windowWidth)
        {
            this.windowHeight = windowHeight;
            this.windowWidth = windowWidth;
            int[] coords = {2, 0};
             
            for (int i = 0; i< 100; i++)
            {
               // new Geometry(new Point(100i, ))
            }
            
            new Block(new Vector2(500, 500), new Rectangle(0, 0, 100, 100), BlockTypes.ARedBlock);
            new Bubble(new Vector2(1000, 1950), new Rectangle(0, 0, 20, 20));

        }

        public Level(String filename)
        {
            filePath = filename;
            String[] allLines = File.ReadAllLines(filename);

            int width = 0;
            int height = 0;
            bool inBlock = true;

            
            for(int row = 0; row < allLines.Length; row++)
            {
                if(row == 0)
                {
                    levelNumber = Int32.Parse(allLines[row][0].ToString());
                    continue;
                }
                string[] letters = allLines[row].Split(',');

                
                for(int col = 0; col < letters.Length; col++)
                {
                    int num = Int32.Parse(letters[col]);
                    if(num == -1)
                    {
                        continue;
                    }
                    

                    BlockTypes enumValue = Enum.GetValues(typeof(BlockTypes)).Cast<BlockTypes>().ToArray()[num];
                    new Geometry(new Point(66*col, 66*row), enumValue);


                    

                }

            }
            

        }

        public string GetFilePath()
        {
            return filePath;
        }


    }
}
