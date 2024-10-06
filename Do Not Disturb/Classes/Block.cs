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
        Rectangle source;
        private BlockTypes type;

        public Block(Vector2 position,  BlockTypes type) : base(position, new Rectangle(position.ToPoint(), new Point(66, 66)))
        {
            this.type = type;
            gravity = 100;
            maxXVelocity = 50;

            var enumArr = Enum.GetValues(typeof(BlockTypes)).Cast<BlockTypes>().ToArray();
            for (int i = 0; i < enumArr.Length; i++)
            {
                if (enumArr[i] == type)
                {
                    this.source = new Rectangle((i % 10) * 66, (i / 10) * 66, 66, 66);
                    break;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
           base.Update(gameTime);

            int sign = 0;

            
            
                sign = Math.Sign(velocity.X);

            int temp = 5;
            if(Grounded)
            {
                temp = 80;
            }
                acceleration.X = -Math.Sign(velocity.X) * temp;

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

        public override void Draw(SpriteBatch sb)
        {
            Vector2 temp = Camera.RelativePosition(hitbox.Location.ToVector2());
            sb.Draw(Geometry.tileset,
                new Rectangle(temp.ToPoint().X, temp.ToPoint().Y, (int)(hitbox.Width * Game1.Scale), (int)(hitbox.Height * Game1.Scale)),
                source,
                Color.White);
        }

        public override void OnCollision_H(GameObject obj)
        {
            if (Math.Sign(obj.Velocity.X * obj.Acceleration.X) >= 0)
            {
                velocity.X = obj.Acceleration.X / 4;
                acceleration.X = obj.Acceleration.X / 2;

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
