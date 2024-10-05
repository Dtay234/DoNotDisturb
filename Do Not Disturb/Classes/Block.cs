using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Do_Not_Disturb.Classes;

namespace Do_Not_Disturb.Classes
{
    internal class Block : Collidable
    {
        private BlockTypes type;
        public Block(Vector2 position, Rectangle hitbox, BlockTypes type) : base(position, hitbox)
        {
            this.type = type;
            gravity = 100;
        }

        public override void Update(GameTime gameTime)
        {
           base.Update(gameTime);

            int sign = 0;

            
            if (Grounded)
            {
                sign = Math.Sign(velocity.X);
                acceleration.X = -Math.Sign(velocity.X) * 80;

                // if velocity changes sign in this update, set it to 0
                if (sign != Math.Sign(velocity.X + acceleration.X * gameTime.ElapsedGameTime.TotalSeconds)
                    && sign != 0)
                {
                    acceleration.X = 0;
                    velocity.X = 0;
                }

                // if velocity is small enough, set it to 0
                if (velocity.X != 0
                    && Math.Abs(velocity.X) < 1f)
                {
                    velocity.X = 0;
                    acceleration.X = 0;
                }
            }
            
        }

        public override void Draw(SpriteBatch sb)
        {
            
        }

        public override void OnCollision_H(GameObject obj)
        {
            acceleration.X = obj.Velocity.X;

            obj.Velocity = new Vector2(0, obj.Velocity.Y);
                    }

        public override void OnCollision_V(GameObject obj)
        {
            obj.Velocity = new Vector2(obj.Velocity.X, 0);
        }
    }
}
