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
        private enum FaceDirection
        {
            Left,
            Right
        }

        private const int fps = 60;
        private int animationFrame;
        private int frameCounter;
        private Dictionary<T, Tuple<int, int, int, int>> data;
        private int[] hitboxOffset;
        private int[] spriteSize;
        private Texture2D spritesheet;
        private double timer;
        private const double timePerFrame = 1 / (double)fps;
        T currentAnimation;
        FaceDirection faceDirection;
        Queue<T> queue;


        public Animation(string filePath, Texture2D texture)
        {
            T[] array = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
            string[] stringArray = Enum.GetNames(typeof(T));
            string[] file = File.ReadAllLines("../../../Content/AnimationData/" + filePath);
            data = new();
            spritesheet = texture;
            queue = new();
            

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
                    spriteSize[1] = int.Parse(temp[0].Split('x')[1]);
                    hitboxOffset = new int[2];
                    hitboxOffset[0] = int.Parse(temp[1].Split(':')[0]);
                    hitboxOffset[1] = int.Parse(temp[1].Split(':')[1]);

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

        public void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.TotalSeconds;
            if (timer > timePerFrame)
            {
                frameCounter++;
                timer -= timePerFrame;
            }

            if (frameCounter == data[currentAnimation].Item4)
            {
                frameCounter = 0;
                animationFrame++;
            }
            if (animationFrame == data[currentAnimation].Item3)
            {
                animationFrame = 0;
            }

            if (animationFrame == 0 && frameCounter == 0 && queue.Count > 0)
            {
                currentAnimation = queue.Dequeue();
            }
        }

        public void ChangeAnimation(T newAnimation, int i, bool reset = false)
        {
            if (reset)
            {
                if (!currentAnimation.Equals(newAnimation))
                {
                    frameCounter = 0;
                    animationFrame = 0;
                    currentAnimation = newAnimation;
                    timer = 0;
                }
                
            }
            else
            {
                if(queue.Count == 0)
                queue.Enqueue(newAnimation);
            }
            

            

            

         

                faceDirection = (FaceDirection)i;
            


            
            
        }

        public void Draw(SpriteBatch sb, Rectangle destination, int layer = 0)
        {
            if (spritesheet == null)
            {
                return;
            }

            int x = animationFrame * data[currentAnimation].Item1;
            int y = 0;

            foreach (KeyValuePair<T, Tuple<int,int,int,int>> item in data)
            {
                if (!item.Key.Equals(currentAnimation))
                {
                    y += item.Value.Item2;
                    
                }
                else
                {
                    break;
                }
            }

            Rectangle source = new Rectangle(x, y, spriteSize[0], spriteSize[1]);
            
            
            
            sb.Draw(spritesheet, 
                new Rectangle(
                    Camera.RelativePosition(
                        destination.Location.ToVector2()).ToPoint() - new Point((int)(hitboxOffset[0] * Game1.Scale), (int)(hitboxOffset[1] * Game1.Scale)), 
                    new Point((int)(spriteSize[0] * Game1.Scale) , (int)(spriteSize[1] * Game1.Scale))), 
                source, 
                Color.White,
                0f,
                Vector2.Zero,
                faceDirection == FaceDirection.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                layer);
        }

        public void DrawScreen(SpriteBatch sb, Rectangle destination, int layer = 0)
        {
            /*
            if (spritesheet == null)
            {
                return;
            }

            int x = animationFrame * data[currentAnimation].Item1;
            int y = 0;

            foreach (KeyValuePair<T, Tuple<int, int, int, int>> item in data)
            {
                if (!item.Key.Equals(currentAnimation))
                {
                    y += item.Value.Item2;

                }
                else
                {
                    break;
                }
            }

            Rectangle source = new Rectangle(x, y, spriteSize[0], spriteSize[1]);

            sb.Draw(Game1.pixel, new Rectangle(0, 0, 1920, 1080), Color.White);
            
            sb.Draw(spritesheet,
                new Rectangle(
                    
                        new Point(0,0),
                    new Point((int)(spriteSize[0]), (int)(spriteSize[1]))),
                source,
                Color.White,
                0f,
                Vector2.Zero,
                faceDirection == FaceDirection.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                layer);

            sb.DrawString(Game1.font, source.X.ToString() + " - " + source.Y.ToString(), new Vector2(0, 0), Color.White);
            
            */
        }
    }
}
