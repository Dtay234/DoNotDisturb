using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Do_Not_Disturb.Classes
{
    

    public abstract class Collidable : GameObject
    {
        
        public Collidable(Vector2 position, Rectangle hitbox) : base(position, hitbox)
        {
            
        }

        public abstract void OnCollision_H(GameObject obj);
        public abstract void OnCollision_V(GameObject obj);
    }
}
