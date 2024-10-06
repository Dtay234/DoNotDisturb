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
        public Block(Vector2 position,  BlockTypes type) : base(position, new Rectangle(position.ToPoint(), new Point(66, 66)))
        {
            this.type = type;
            gravity = 100;
            maxXVelocity = 50;
        }

        public override void Update(GameTime gameTime)
        {
           base.Update(gameTime);

            int sign = 0;

            
            if (true)
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
            sb.DrawString(Game1.font, velocity.X.ToString(), new Vector2(200, 0), Color.Black);
        }

        public override void OnCollision_H(GameObject obj)
        {
            if (Math.Sign(obj.Velocity.X * obj.Acceleration.X) >= 0)
            {
                velocity.X = obj.Acceleration.X / 4;

                obj.Velocity = new Vector2(
                    //maxXVelocity * Math.Sign(obj.Acceleration.X) / 3, 
                    velocity.X,
                    obj.Velocity.Y);
            }
            
        }

        public override void OnCollision_V(GameObject obj)
        {
            obj.Velocity = new Vector2(obj.Velocity.X, 0);
        }
    }
}
