using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Do_Not_Disturb.Classes
{
    internal class Bubble : Collidable
    {
        private Texture2D sprite;
        private const float bubbleVelocityY = -40;
        public Bubble(Vector2 position, Rectangle hitbox) : base(position, hitbox)
        {
        }

        public void Update(GameTime gameTime)
        {

        }

        public bool IsColliding(Collidable collidable)
        {
            if(this.hitbox.Intersects(collidable.Hitbox))
            {
                return true;
            }
            return false;
        }

        public void Pop(Collidable collidable)
        {
            collidabl
        }


    }
}
