using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

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
        private bool sfx = false;

        public Car(Vector2 position) : base(position, new Rectangle(position.ToPoint(), new Point(163,73)))
        {
            gravity = 90;
            animation = new("car.txt", sheet);
            maxXVelocity = 100;
            faceDirection = FaceDirection.Right;
            animation.ChangeAnimation(Animations.Static, (int)faceDirection, true);
        }

        public override void Update(GameTime gameTime)
        {
            if (timer > 0)
            {
                timer -= gameTime.ElapsedGameTime.TotalSeconds;
                animation.ChangeAnimation(Animations.Wind, (int)faceDirection, false);

                
                
                
            }
            else
            {
                if (wound == true)
                {
                    animation.ChangeAnimation(Animations.Move, (int)faceDirection, true);
                    
                    if (faceDirection == FaceDirection.Left)
                    {
                        velocity.X = -maxXVelocity;
                        acceleration.X = -maxXVelocity;
                    }
                    else
                    {
                        velocity.X = maxXVelocity;
                        acceleration.X = maxXVelocity;
                    }

                    
                }

                if (velocity.X != 0)
                {
                    foreach (GameObject obj in GetTower())
                    {
                        OnCollision_V(obj);
                    }
                }
            }

            animation.Update(gameTime);
            base.Update(gameTime);
        }


        public override void OnCollision_H(GameObject obj)
        { 

        }
        public override void OnCollision_V(GameObject obj)
        {
            if (!wound && obj is Player)
            {
                timer = 3;
                wound = true;
            }

            if (velocity.X == 0)
            obj.Velocity = new Vector2(velocity.X + obj.Velocity.X, 0);
            else
            {

                obj.Velocity = new Vector2(velocity.X, velocity.Y < 0 ? velocity.Y : obj.Velocity.Y);
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            animation.Draw(sb, hitbox);
        }

    }
}
