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
        private enum Animations
        {
            Spawn,
            Wobble,
            Pop
        }

        private Animations animation;
        private Texture2D sprite;
        private const float bubbleVelocityY = -40;
        private double timer;

        public Bubble(Vector2 position, Rectangle hitbox) : base(position, hitbox)
        {
            timer = 10;
        }

        public override void Update(GameTime gameTime)
        {
            Collidable collision = null;
            if ((collision = IsCollidingWithObject()) != null)
            {
                collision.Velocity = new Vector2(collision.Velocity.X, -40);
                animation = Animations.Pop;
                Game1.collidableList.Remove(this);
            }



            timer -= gameTime.ElapsedGameTime.TotalSeconds;


        }

        public bool IsColliding(Collidable collidable)
        {
            if(this.hitbox.Intersects(collidable.Hitbox))
            {
                return true;
            }
            return false;
        }


        public override void Draw(SpriteBatch sb)
        {
            
        }
    }
}
