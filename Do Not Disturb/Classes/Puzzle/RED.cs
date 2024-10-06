using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Do_Not_Disturb.Classes.Puzzle
{
    public delegate void LevelCompleteHandler();
    internal class RED
    {

        public Block R;
        public Block E;
        public Block D;
        public event LevelCompleteHandler LevelComplete;

        public RED(Vector2 rPos, Vector2 ePos, Vector2 dPos) 
        {
            R = new Block(rPos, BlockTypes.RBlueBlock);
            E = new Block(ePos, BlockTypes.EGreenBlock);
            D = new Block(dPos, BlockTypes.EGreenBlock);


        }

        public void Update(GameTime gameTime)
        {
            if (Complete())
            {
                LevelComplete();
            }
        }

        private bool Complete()
        {
            bool touching1 = new Rectangle(R.Hitbox.Location + new Point(5,5), R.Hitbox.Size).Intersects(E.Hitbox);
            bool touching2 = new Rectangle(D.Hitbox.Location - new Point(5, 5), D.Hitbox.Size).Intersects(E.Hitbox);

            if (touching1 && touching2 && R.Position.X <  E.Position.X && D.Position.X > E.Position.X)
            {
                return true;
            }

            return false;
        }

        
    }
}
