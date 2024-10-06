using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Do_Not_Disturb.Classes
{
    public abstract class GameObject
    {
        protected enum FaceDirection
        {
            Left,
            Right
        }
        protected FaceDirection faceDirection;

        protected Vector2 position;
        protected Vector2 velocity;
        protected Vector2 acceleration;
        protected Rectangle hitbox;

        protected float maxXVelocity = 100;
        protected float maxYVelocity = 60;
        protected float gravity = 70;

        public bool Grounded
        {
            get
            {
                if (Game1.CheckGrounded(this))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public virtual int CollisionAccuracy
        {
            get
            {

                return 20;
                // Min accuracy is 1
                if (velocity.X == 0 &&
                    velocity.Y == 0)
                {
                    return 1;
                }

                return (int)(velocity.Length() / 4f);  //Use the magnitude of the velocity to get the accuracy




            }
        }

        public Vector2 Position
        {
            get { return position; }
        }
        public Vector2 Velocity
        {
            get { return velocity; }
            set
            {
                velocity = value;
            }
        }
        public Vector2 Acceleration
        {
            get { return acceleration; }
            set
            {
                acceleration = value;
            }
        }
        public Rectangle Hitbox { get { return hitbox; } }

        public GameObject(Vector2 position, Rectangle hitbox)
        {
            this.position = position;
            this.hitbox = new Rectangle(position.ToPoint(), hitbox.Size);
            Game1.objects.Add(this);
        }



        public void Movement(GameTime gt)
        {
            if (Grounded)
            {
                acceleration.Y = 0;
            }
            else
            {
                acceleration.Y = gravity;
            }

            float time = (float)gt.ElapsedGameTime.TotalSeconds;

            int iterationCounter = 1;       // Number of collision checks we've done

            Point lastSafePosition = new Point((int)Math.Round(position.X), (int)Math.Round(position.Y));        //Last point before a collision

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

                if (!Game1.Collide(hitbox))
                {
                    lastSafePosition = new Point((int)Math.Round(position.X), (int)Math.Round(position.Y));      //Store old position in case we collide
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

                if (Game1.Collide(hitbox))        // Check if there was a collision
                {
                    if (Game1.ObjectCollide(hitbox) != null)
                    hitbox = new Rectangle(lastSafePosition, hitbox.Size);    // Revert hitbox position back to before collision
                    position = lastSafePosition.ToVector2();                      // Revert position
                    velocity.Y = 0;
                    break;
                }

                Geometry geo = null;
                GameObject obj = null;

                if (Game1.Collide(hitbox, out geo, out obj))
                {
                    hitbox = new Rectangle(lastSafePosition, hitbox.Size);
                    position = lastSafePosition.ToVector2();


                    if (geo != null)
                    {
                        velocity.Y = 0;
                    }
                    if (obj != null && obj is Collidable)
                    {
                        ((Collidable)obj).OnCollision_V(this);
                    }

                    break;
                }


                iterationCounter++;
            }


            //Do the same thing but in the X direction
            iterationCounter = 1;

            while (iterationCounter <= CollisionAccuracy)
            {

                if (!Game1.Collide(hitbox))
                {
                    lastSafePosition = new Point((int)Math.Round(position.X), (int)Math.Round(position.Y));
                }

                //Cap velocity
                if (Math.Abs(velocity.X) > maxXVelocity)
                {
                    velocity.X = maxXVelocity * Math.Sign(velocity.X);
                }


                position.X += tempVelocity.X * (time * iterationCounter / CollisionAccuracy);

                hitbox = new Rectangle(
                    (int)Math.Round(position.X),
                    (int)Math.Round(position.Y),
                    hitbox.Width,
                    hitbox.Height);

                Geometry geo = null;
                GameObject obj = null;

                if (Game1.Collide(hitbox, out geo, out obj))
                {
                    hitbox = new Rectangle(lastSafePosition, hitbox.Size);
                    position = lastSafePosition.ToVector2();
                    

                    if (geo != null)
                    {
                        velocity.X = 0;
                    }
                    if (obj != null && obj is Collidable)
                    {
                        ((Collidable)obj).OnCollision_H(this);
                    }

                    bool temp = Game1.Collide(hitbox);

                    break;
                }

                

                iterationCounter++;

            }
        }


        public virtual void Update(GameTime gameTime)
        {
            Movement(gameTime);
        }

        public abstract void Draw(SpriteBatch sb);



    }
}
