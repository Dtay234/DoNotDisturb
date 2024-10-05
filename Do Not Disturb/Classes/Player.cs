using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Security.Principal;
using System.Diagnostics;

namespace Do_Not_Disturb.Classes
{
    enum PlayerMovement
    {
        LookLeft,
        LookRight,
        WalkLeft,
        WalkRight,
        JumpLeft,
        JumpRight,
        CrouchLeft,
        CrouchRight,
        Pushing,
        Idle,


    }
    internal class Player : Collidable
    {
        public static Texture2D spriteSheet;
        public static Keys lastPressed;
        private PlayerMovement state;
        const float maxXVelocity = 100;
        const float maxYVelocity = 60;
        const float gravity = 70;

        

        public Player(Vector2 position, Rectangle rect) : base (position, rect)
        {
            acceleration.Y = 70;
            Camera.Focus = this;
        }

        public void Update(GameTime gameTime, KeyboardState kState)
        {
           if (Grounded)
            {
                acceleration.Y = 0;
            }
           else
            {
                acceleration.Y = gravity;
            }

            //Horizontal movement
            if (kState.IsKeyDown(Keys.A) &&
                !kState.IsKeyDown(Keys.D))
            {
                // acceleration is higher if the player is moving in the opposite direction for smoother movement
                if (Math.Sign(velocity.X) >= 0)
                velocity.X = -maxXVelocity / 2;
                acceleration.X = velocity.X > 0 ? -maxXVelocity * 5f : -maxXVelocity * 2f;
               
                //faceDirection = FaceDirection.Left;
            }
            else if (kState.IsKeyDown(Keys.D) &&
                !kState.IsKeyDown(Keys.A))
            {
                // acceleration is higher if the player is moving in the opposite direction for smoother movement
                if (Math.Sign(velocity.X) <= 0)
                    velocity.X = maxXVelocity / 2;
                acceleration.X = velocity.X < 0 ? maxXVelocity * 5f : maxXVelocity * 2f;
                
                //faceDirection = FaceDirection.Right;
            }


            //Jump 
            if ((kState.IsKeyDown(Keys.Space)))
            {
                if (Grounded)
                {
                    velocity.Y = -50;

                    /*
                    //remove any buffered jumps from the list
                    BufferedInput buffer = input.Buffered.Find(item => item.Key == Keys.Space);
                    input.Buffered.Remove(buffer);
                    */
                }
                else
                {
                    /*
                    //can buffer a jump, if they hit the ground while it is buffered they will immediately jump
                    input.Buffer(Keys.Space);
                    */
                }
            }
            
            //Fine tuning movement
            int sign = 0;

            //Slow down if not pressing anything
            if (!kState.IsKeyDown(Keys.D) &&
                !kState.IsKeyDown(Keys.A)
                && velocity.X != 0)
            {
                sign = Math.Sign(velocity.X);
                acceleration.X = -Math.Sign(velocity.X) * 80;
            }

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
            
            /*
            velocity += acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            */

            



            //hitbox = new Rectangle((int)Math.Round(position.X), (int)Math.Round(position.Y), hitbox.Width, hitbox.Height);

            

            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            int iterationCounter = 1;       // Number of collision checks we've done

            Point lastSafePosition = new Point((int)position.X, (int)position.Y);        //Last point before a collision

            if (!Grounded)
            {
                acceleration.Y = gravity * (1 + (100 - Math.Abs(velocity.Y)) / 50);
            }

            velocity += acceleration * time;                                   //Update velocity
            Vector2 tempVelocity = new Vector2(velocity.X, velocity.Y);        //For if the player is airborne

            if (velocity.X > maxXVelocity)
            {
                velocity.X = maxXVelocity;
            }
            if (velocity.X < -maxXVelocity)
            {
                velocity.X = -maxXVelocity;
            }

            //Vertical
            while (iterationCounter <= CollisionAccuracy)                      //Scaling number of checks
            {

                if (!IsCollidingWithTerrain())
                {
                    lastSafePosition = new Point((int)position.X, (int)position.Y);      //Store old position in case we collide
                }

                //Cap velocity
                if (Math.Abs(velocity.Y) > maxYVelocity)
                {
                    velocity.Y = maxYVelocity * Math.Sign(tempVelocity.Y);
                }

                position.Y += tempVelocity.Y * (time * iterationCounter / CollisionAccuracy);     // Increment position

                hitbox = new Rectangle(
                    (int)Math.Round(position.X),
                    (int)Math.Round(position.Y),
                    hitbox.Width,
                    hitbox.Height);                      // Update hitbox location

                if (IsCollidingWithTerrain())        // Check if there was a collision
                {
                    hitbox = new Rectangle(lastSafePosition, hitbox.Size);    // Revert hitbox position back to before collision
                    position = lastSafePosition.ToVector2();                      // Revert position
                    velocity.Y = 0;
                    break;
                }

                iterationCounter++;
            }


            //Do the same thing but in the X direction
            iterationCounter = 1;

            while (!IsCollidingWithTerrain() && iterationCounter <= CollisionAccuracy)
            {

                if (!IsCollidingWithTerrain())
                {
                    lastSafePosition = new Point((int)position.X, (int)position.Y);
                }

                //Cap velocity
                if (Math.Abs(velocity.X) > maxXVelocity)
                {
                    velocity.X = maxXVelocity * Math.Sign(velocity.X);
                }

                if (!Grounded)
                {
                    tempVelocity.X = velocity.X / 1.2f;
                }

                position.X += tempVelocity.X * (time * iterationCounter / CollisionAccuracy);

                hitbox = new Rectangle(
                    (int)Math.Round(position.X),
                    (int)Math.Round(position.Y),
                    hitbox.Width,
                    hitbox.Height);

                if (IsCollidingWithTerrain())
                {
                    hitbox = new Rectangle(lastSafePosition, hitbox.Size);
                    position = lastSafePosition.ToVector2();
                    velocity.X = 0;
                    break;
                }
                iterationCounter++;

            }

        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(spriteSheet, new Rectangle(Camera.RelativePosition(position).ToPoint(), hitbox.Size), new Rectangle(0, 0, 32, 32), Color.White);
            sb.DrawString(Game1.font, velocity.X.ToString(), new Vector2(0,0), Color.Black);
            sb.DrawString(Game1.font, acceleration.X.ToString(), new Vector2(0, 100), Color.Black);
        }

    }
}
