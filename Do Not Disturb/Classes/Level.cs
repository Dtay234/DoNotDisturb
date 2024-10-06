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
using System.Reflection.Metadata.Ecma335;

namespace Do_Not_Disturb.Classes
{
    internal class Level
    {
        private int windowHeight;
        private int windowWidth;
        private List<GameObject> objects;
        private string filePath;
        private int levelNumber;

        public static BlockTypes[] enumArr = Enum.GetValues(typeof(BlockTypes)).Cast<BlockTypes>().ToArray();

        public Level(int windowHeight, int windowWidth)
        {
            this.windowHeight = windowHeight;
            this.windowWidth = windowWidth;
            int[] coords = {2, 0};
             
            for (int i = 0; i< 100; i++)
            {
               // new Geometry(new Point(100i, ))
            }
            
            new Block(new Vector2(500, 500), BlockTypes.ARedBlock);
            new Bubble(new Vector2(1000, 1950), new Rectangle(0, 0, 20, 20));

        }

        public Level(String filename)
        {
            filePath = filename;
  
            

        }

        public string GetFilePath()
        {
            return filePath;
        }

        public void loadWorld()
        {
            String[] allLines = File.ReadAllLines(this.filePath);

            int width = 0;
            int height = 0;
            bool inBlock = true;


            for (int row = 0; row < allLines.Length; row++)
            {
                if (row == 0)
                {
                    levelNumber = Int32.Parse(allLines[row][0].ToString());
                    continue;
                }
                string[] letters = allLines[row].Split(',');


                for (int col = 0; col < letters.Length; col++)
                {
                    int num = Int32.Parse(letters[col]);
                    if (num == -1)
                    {
                        continue;
                    }
                    if (num == -2)
                    {
                        if (Game1.objects.Exists(x => x is Player))
                        {
                            throw new Exception("Too many players!");
                        }

                        new Player(new Vector2(66 * col, 66 * row));
                        continue;
                    }



                    new Geometry(new Point(66 * col, 66 * row), enumArr[num]);




                }

            }
        }
        public RED loadActualObjects(string filename)
        {
            string[] allLines = File.ReadAllLines(filename);
            string[] lines;
            Vector2[] RED = new Vector2[3];
            for(int i = 1; i < allLines.Length; i++) { 
                lines = allLines[i].Split(",");
                int[] dimensions;
                if (lines[0].Equals("Player"))
                {
                    continue;
                }

                BlockTypes enumVal = (BlockTypes)Enum.Parse(typeof(BlockTypes), lines[0]);

                /*
                if (lines[0].Contains("Hort"))
                {
                    dimensions = new int[] { 2, 1 };
                }
                else if ()
                {

                }
                else
                {
                    dimensions = new int[] { 1, 1 };
                }
                */

                if(enumVal == BlockTypes.RBlueBlock)
                {
                    RED[0] = new Vector2(float.Parse(lines[1]) * 66, float.Parse(lines[2]) * 66);

                } else if(enumVal == BlockTypes.EGreenBlock)
                {
                    RED[1] = new Vector2(float.Parse(lines[1]) * 66, float.Parse(lines[2]) * 66);
                } else if(enumVal == BlockTypes.DYellowBlock)
                {
                    RED[2] = new Vector2(float.Parse(lines[1]) * 66, float.Parse(lines[2]) * 66);
                } else
                {
                  new Block(new Vector2(float.Parse(lines[1]) * 66, float.Parse(lines[2]) * 66), enumVal);

                }

            }

            return new RED(RED[0], RED[1], RED[2]);
           
        }


    }
}
