using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Do_Not_Disturb.Classes
{
    internal class Car : Collidable
    {
        private enum Animations
        {
            Wind,
            Static,
            Move
        }

        public static Texture2D sheet;
        double timer;
        private Animation<Animations> animation;
        bool wound = false;

        public Car(Vector2 position) : base(position, new Rectangle(position.ToPoint(), new Point(200, 100)))
        {
            animation = new("car.txt", sheet);
            maxXVelocity = 100;
        }

        public override void Update(GameTime gameTime)
        {
            if (timer > 0)
            {
                timer -= gameTime.ElapsedGameTime.TotalSeconds;

            }
            else
            {
                if (wound == true)
                {
                    if (faceDirection == FaceDirection.Left)
                    {
                        velocity.X = -maxXVelocity;
                    }
                    else
                    {
                        velocity.X = maxXVelocity;
                    }
                }
            }
            base.Update(gameTime);
        }


        public override void OnCollision_H(GameObject obj)
        { 

        }
        public override void OnCollision_V(GameObject obj)
        {
            if (!wound)
            {
                timer = 3;
                wound = true;
            }


            obj.Velocity = new Vector2(velocity.X, 0);
        }

        public override void Draw(SpriteBatch sb)
        {
            animation.Draw(sb, hitbox);
        }

    }
}
