using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Do_Not_Disturb.Classes
{
    internal abstract class Collidable
    {
        protected Vector2 position;
        protected Vector2 velocity;
        protected Vector2 acceleration;
        protected Rectangle hitbox;

        
        public Vector2 Velocity
        {
            get { return position; }
        }
        public Vector2 Acceleration
        {
            get { return position; }
        }
        public Rectangle Hitbox { get { return hitbox; } }

        public Collidable (Vector2 position, Rectangle hitbox)
        {
            this.position = position;
            this.hitbox = hitbox;
        }

        /*
        public bool IsCollidingWithTerrain()
        {

        }
        */
    }
}
