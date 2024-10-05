using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using System.Net.Mime;

namespace Do_Not_Disturb.Classes
{
    internal class Geometry
    {
        public enum BlockTypes
        {
            ARedBlock,
            RBlueBlock,
            EGreenBlock,

        }

        public static List<Geometry> map = new List<Geometry>();
        public static Dictionary<BlockTypes, Texture2D> boxSprites;
        private Rectangle boundBox;
        

        public Rectangle BoundBox
        {
            get { return boundBox; }
        }

        public Geometry(Rectangle boundBox)
        {
            this.boundBox = boundBox;
            map.Add(this);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Player.spriteSheet, BoundBox, Color.Red);
        }

        public static void LoadBlocks()
        {
            foreach (BlockTypes block in Enum.GetValues(typeof(BlockTypes)))
            {
                
            }
        }
    }
}
