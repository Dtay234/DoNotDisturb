using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Do_Not_Disturb.Classes
{
    internal class Block : Collidable
    {
        private BlockTypes type;
        private const float maxXVelocity = 40;
        private const float maxYVelocity = 60;
        private const float gravity = 100;
        public Block(Vector2 position, Rectangle hitbox, BlockTypes type) : base(position, hitbox)
        {
            this.type = type;
        }

        public override void Update(GameTime gameTime)
        {
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
    }
}
