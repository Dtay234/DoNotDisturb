using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;


namespace Do_Not_Disturb.Classes
{
    
    internal class Animation<T> where T : Enum
    {
        private const int fps = 60;
        private int frame;
        private Dictionary<T, Tuple<int, int, int, int>> data;
        private int[] hitboxOffset;
        private int[] spriteSize;

        public Animation(string filePath)
        {
            T[] array = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
            string[] stringArray = Enum.GetNames(typeof(T));
            string[] file = File.ReadAllLines("../../../Content/AnimationData/" + filePath);
            data = new();
            

            for (int i = 0; i < file.Length; i++)
            {
                if (file[i] == null || file[i] == string.Empty) 
                {
                    continue;
                }

                if (file[i][0] == '@')
                {
                    string[] temp = file[i].Substring(1).Split(',');
                    spriteSize = new int[2];
                    spriteSize[0] = int.Parse(temp[0].Split('x')[0]);
                    spriteSize[0] = int.Parse(temp[0].Split('x')[1]);
                    hitboxOffset = new int[2];
                    hitboxOffset[0] = int.Parse(temp[1].Split(':')[0]);
                    hitboxOffset[0] = int.Parse(temp[1].Split(':')[1]);

                }

                if (file[i].Substring(0,2) == "//")
                {
                    string name = file[i].Substring(2);
                    string[] split = file[i + 1].Split(',');

                    if (split[0] == "#")
                    {
                        split[0] = spriteSize[0].ToString();
                    }
                    if (split[1] == "#")
                    {
                        split[1] = spriteSize[1].ToString();
                    }

                    int[] numbers = new int[4];

                    for (int j = 0; j < 4; j++)
                    {
                        numbers[j] = int.Parse(split[j]);
                    }

                    data.Add((T)Enum.Parse(typeof(T), name), new Tuple<int, int, int, int>(numbers[0], numbers[1], numbers[2], numbers[3]));
                }
            }

            
        }
    }
}
