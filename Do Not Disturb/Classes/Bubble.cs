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
            Pop,
            None
        }

        private Animations animation;
        public static Texture2D sprite;
        private const float bubbleVelocityY = -40;
        private double timer;
        private Animation<Animations> anim;
        public bool popped = false;

        public Bubble(Vector2 position, Rectangle hitbox) : base(position, hitbox)
        {
            timer = 10;
            gravity = 0;
            anim = new Animation<Animations>("bubble.txt", sprite);
            anim.ChangeAnimation(Animations.Spawn, 0, false);
            anim.ChangeAnimation(Animations.Wobble, 0, false);
        }

        public override void Update(GameTime gameTime)
        {
            
            timer -= gameTime.ElapsedGameTime.TotalSeconds;
            this.hitbox = new Rectangle(position.ToPoint(), hitbox.Size);
            if (Game1.Collide(this.hitbox))
            {
                hitbox = new Rectangle(0, 0, 0, 0);
                popped = true;
                anim.ChangeAnimation(Animations.Pop, 0, true);
                anim.ChangeAnimation(Animations.None, 0, false);
                Game1.stallPopped += 1;
            }

            anim.Update(gameTime);
        }



        public override void Draw(SpriteBatch sb)
        {
            anim.Draw(sb, hitbox);
        }

        public override void OnCollision_H(GameObject obj)
        {
            if (obj != null)
            {
                (obj).Velocity = new Vector2((obj).Velocity.X, -180);
                hitbox = new Rectangle(0,0,0,0);
                popped = true;
                anim.ChangeAnimation(Animations.Pop, 0, true);
                anim.ChangeAnimation(Animations.None, 0, false);
                Game1.stallPopped++;
            }
        }

        public override void OnCollision_V(GameObject obj)
        {
            OnCollision_H(obj);
        }
    }
}
