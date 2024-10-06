﻿using System;
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

        public Level(int windowHeight, int windowWidth)
        {
            this.windowHeight = windowHeight;
            this.windowWidth = windowWidth;
            int[] coords = {2, 0};
             
            new Geometry(new Rectangle(0, 0, 10, windowHeight), BlockTypes.ARedBlock, coords );
            new Geometry(new Rectangle(windowWidth, 0, 10, windowHeight), BlockTypes.ARedBlock ,coords);
            new Geometry(new Rectangle(0, 0, windowWidth, 10), BlockTypes.ARedBlock, coords);
            new Geometry(new Rectangle(0, windowWidth, windowWidth, 10), BlockTypes.ARedBlock, coords);
            
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
                    
                }
                string[] letters = allLines[row].Split(',');
                
                for(int col = 0; col < letters.Length; col++)
                {
                    switch(letters[col])
                    {
                        case "-1":
                            break;

                        case "2":
                            new Geometry(new Rectangle(col * 66, row * 66, 66, 66), BlockTypes.TopVertLongBlock, (Tuple<int, int>)(0, 2));
                            break;

                        case "4":
                            break;

                        case "10":
                            new Geometry(new Rectangle(col * 66, row * 66, 66, 66), BlockTypes.LeftHorLongBlock);
                            break;

                        case "11":
                            new Geometry(new Rectangle(col * 66, row * 66, 66, 66), BlockTypes.BotVertLongBlock);
                            break;

                        case "12":
                            new Geometry(new Rectangle(col * 66, row * 66, 66, 66), BlockTypes.BotVertLongBlock);
                            break;
                    }
                }

            }
            

        }

        public string GetFilePath()
        {
            return filePath;
        }


    }
}
