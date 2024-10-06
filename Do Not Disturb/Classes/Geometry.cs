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
        RBlueBlock,
        EGreenBlock,
        TopVertLongBlock,
        TopLeftPinkPil,
        TopMidPinkPil,
        TopRightPinkPil,
        TopGreenVertBlock,
        TopRedVertBlock,
        TopBlueVertBlock,
        TopYellowVertBlock,
        LeftHorLongBlock,
        RightHorLongBlock   ,
        BotVertLongBlock,
        CenterLeftPinkPil,
        CenterMidPinkPil,
        CenterBotPinkPil,
        BotGreenVertBlock,
        BotRedVertBlock,
        BotBlueVertBlock,
        BotYellowVertBlock,
        ARedBlock,
        DYellowBlock,
        ABlueBlock,
        BotLeftPinkPil,
        BotMidPinkPil,
        BotRightPinkPil,
        LeftHorRedBlock,
        RightHorRedBlock,
        LeftHorYellowBlock,
        RightHorYellowBlock,
        TopLeftBluePil,
        TopMidBluePil,
        TopRightBluePil,
        TopLeftGreenPil,
        TopMidGreenPil,
        TopRightGreenPil,
        LeftHorGreenBlock,
        RightHorGreenBlock,
        LeftHorBlueBlock,
        RightHorBlueBlock,
        CenterLeftBluePil,
        CenterMidBluePil,
        CenterRightBluePil,
        CenterLeftGreenPil,
        CenterMidGreenPil,
        CenterRightGreenPil,
        LeftHorOrangeBlock,
        RightHorOrangeBlock,
        LeftHorAquaBlock,
        RightHorAquaBlock,
        BotLeftBluePil,
        BotMidBluePil,
        BotRightBluePil,
        BotLeftGreenPil,
        BotMidGreenPil,
        BotRightGreenPil,
        LeftHorPurpleBlock,
        RightHorPurpleBlock,
        LeftHorPinkBlock,
        RightHorPinkBlock
    }
    public class Geometry
    {
        

        public static List<Geometry> map = new List<Geometry>();
        public static Dictionary<BlockTypes, Point> boxDimensions = new();
        public static Texture2D tileset;
        private Rectangle boundBox;
        private Rectangle source;
        private BlockTypes type;


        public Rectangle BoundBox
        {
            get { return boundBox; }
        }

        public Geometry(Point location, BlockTypes type)
        {
            this.boundBox = new Rectangle(location.X, location.Y, 66, 66);
            this.type = type;
            var enumArr = Enum.GetValues(typeof(BlockTypes)).Cast<BlockTypes>().ToArray();
            for (int i = 0; i < enumArr.Length; i++)
            {
                if (enumArr[i] == type)
                {
                    this.source = new Rectangle((i % 10) * 66, (i / 10) * 66, 66, 66);
                    break;
                }
            }
            map.Add(this);
        }

        public void Draw(SpriteBatch sb)
        {
            Vector2 temp = Camera.RelativePosition(boundBox.Location.ToVector2());
            sb.Draw(tileset, 
                new Rectangle(temp.ToPoint(), BoundBox.Size), 
                source, 
                Color.White);
        }

       


    }
}
