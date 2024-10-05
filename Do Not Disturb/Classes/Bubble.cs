using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Do_Not_Disturb.Classes
{
    internal class Bubble : Collidable
    {
        private enum Animations
        {
            Spawn,
            Wobble,
            Pop
        }

        private Animations animation;
        private Texture2D sprite;
        private const float bubbleVelocityY = -40;
        private double timer;

        public Bubble(Vector2 position, Rectangle hitbox) : base(position, hitbox)
        {
            timer = 10;
            gravity = 0;
        }

        public override void Update(GameTime gameTime)
        {
           
            timer -= gameTime.ElapsedGameTime.TotalSeconds;
            this.hitbox = new Rectangle(position.ToPoint(), hitbox.Size);

        }



        public override void Draw(SpriteBatch sb)
        {

        }

        public override void OnCollision_H(GameObject obj)
        {
            if (obj != null)
            {
                (obj).Velocity = new Vector2((obj).Velocity.X, -40);
                hitbox = new Rectangle(0,0,0,0);
            }
        }

        public override void OnCollision_V(GameObject obj)
        {
            OnCollision_H(obj);
        }
    }
}
