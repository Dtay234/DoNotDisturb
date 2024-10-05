using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Do_Not_Disturb.Classes
{
    public abstract class Collidable
    {
        protected enum FaceDirection
        {
            Left,
            Right
        }
        protected Vector2 position;
        protected Vector2 velocity;
        protected Vector2 acceleration;
        protected Rectangle hitbox;

        public bool Grounded
        {
            get
            {
                if (IsCollidingWithTerrain(new Rectangle(hitbox.X, hitbox.Y + 5, hitbox.Width, hitbox.Height)))
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
        }
        public Rectangle Hitbox { get { return hitbox; } }

        public Collidable (Vector2 position, Rectangle hitbox)
        {
            this.position = position;
            this.hitbox = hitbox;
        }

        
       
        
        public bool IsCollidingWithTerrain()
        {
            foreach (Geometry box in Geometry.map)
            {
                if (box.BoundBox.Intersects(Hitbox))
                {
                    
                    return true;
                }
            }

            

            return false;
        }

        public bool IsCollidingWithTerrain(Rectangle rect)
        {
            foreach (Geometry box in Geometry.map)
            {
                if (box.BoundBox.Intersects(rect))
                {

                    return true;
                }
            }



            return false;
        }

        public Collidable IsCollidingWithObject()
        {
            foreach (Collidable collidable in Game1.collidableList)
            {
                if (collidable != this)
                {
                    if (collidable.Hitbox.Intersects(hitbox))
                    {
                        return collidable;
                    }
                }
            }

            return null;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch sb);
    }
}
