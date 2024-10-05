﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using System.Net.Mime;
using Microsoft.Xna.Framework.Content;

namespace Do_Not_Disturb.Classes
{
    public enum BlockTypes
    {
        ARedBlock,
        RBlueBlock,
        EGreenBlock,
        NormalLongBlock
    }
    public class Geometry
    {
        

        public static List<Geometry> map = new List<Geometry>();
        public static Dictionary<BlockTypes, Texture2D> boxSprites = new();
        private Rectangle boundBox;
        private BlockTypes type;

        public Rectangle BoundBox
        {
            get { return boundBox; }
        }

        public Geometry(Rectangle boundBox, BlockTypes type)
        {
            this.boundBox = boundBox;
            this.type = type;
            map.Add(this);
        }

        public void Draw(SpriteBatch sb)
        {
            Vector2 temp = Camera.RelativePosition(boundBox.Location.ToVector2());
            sb.Draw(boxSprites[type], new Rectangle(temp.ToPoint(), BoundBox.Size), null, Color.White);
        }

        public static void LoadBlocks(ContentManager Content)
        {
            foreach (BlockTypes block in Enum.GetValues(typeof(BlockTypes)))
            {
                boxSprites.Add(block, Content.Load<Texture2D>("Images/" + block.ToString()));
            }
        }
    }
}
