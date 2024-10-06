using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using System.Net.Mime;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace Do_Not_Disturb.Classes
{
    public enum BlockTypes
    {
        ARedBlock,
        AYellowBlock,
        ABlueBlock,
        RBlueBlock,
        EGreenBlock,
        DYellowBlock,
        LeftHorLongBlock,
        RightHorLongBlock,
        BotVertLongBlock,
        TopVertLongBlock,
        TopLeftPinkPil,
        TopMidPinkPil,
        TopRightPinkPil,
        CenterLeftPinkPil,
        CenterMidPinkPill,
        CenterRightPinkPil,
        BotLeftPinkPil,
        BotMidPinkPil,
        BotRightPinkPil,
        TopLeftYellowPil,
        TopMidYellowPil,
        TopRightYellowPil,
        CenterLeftYellowPil,
        CenterMidYellowPill,
        CenterRightYellowPil,
        BotLeftYellowPil,
        BotMidYellowPil,
        BotRightYellowPil,
        TopLeftBluePil,
        TopMidBluePil,
        TopRightBluePil,
        CenterLeftBluePil,
        CenterMidBluePill,
        CenterRightBluePil,
        BotLeftBluePil,
        BotMidBluePil,
        BotRightBluePil,
    }
    public class Geometry
    {
        

        public static List<Geometry> map = new List<Geometry>();
        public static Dictionary<BlockTypes, Texture2D> boxSprites = new();
        public static Dictionary<BlockTypes, Point> boxDimensions = new();
        private Rectangle boundBox;
        private BlockTypes type;
        private int[] coords;


        public Rectangle BoundBox
        {
            get { return boundBox; }
        }

        public Geometry(Rectangle boundBox, BlockTypes type, int[] coords)
        {
            this.boundBox = boundBox;
            this.type = type;
            map.Add(this);
            this.coords = coords;
        }

        public void Draw(SpriteBatch sb)
        {
            Vector2 temp = Camera.RelativePosition(boundBox.Location.ToVector2());
            sb.Draw(boxSprites[type], new Rectangle(temp.ToPoint(), BoundBox.Size), new Rectangle(0, 0, 66, 66), Color.White);
        }

        public static void LoadBlocks(ContentManager Content)
        {
            foreach (BlockTypes block in Enum.GetValues(typeof(BlockTypes)))
            {

                //boxSprites.Add(block, Content.Load<Texture2D>("Images/" + block.ToString()));
                boxSprites.Add(block, Content.Load<Texture2D>("Images/" + "ARedBlock"));

                boxDimensions.Add(block, new Point(boxSprites[block].Width, boxSprites[block].Height));
                
            }
        }


    }
}
