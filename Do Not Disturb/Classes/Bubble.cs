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
        private double timer;
        private Animation<Animations> anim;
        public bool popped = false;
        private bool active = false;

        public static double cooldown = 0;

        public Bubble(Vector2 position, Rectangle hitbox, FaceDirection face) : base(position, hitbox)
        {
            timer = 0.5;
            gravity = 0;
            anim = new Animation<Animations>("bubble.txt", sprite);
            faceDirection = face;
            anim.ChangeAnimation(Animations.Spawn, (int)faceDirection, true);
            anim.ChangeAnimation(Animations.Wobble, (int)faceDirection, false);


            
        }

        public override void Update(GameTime gameTime)
        {
            
            timer -= gameTime.ElapsedGameTime.TotalSeconds;
            

            if (timer <= 0)
            {
                active = true;
            }
            
            this.hitbox = new Rectangle(position.ToPoint(), hitbox.Size);
            if (Game1.Collide(this.hitbox))
            {
                hitbox = new Rectangle(0, 0, 0, 0);
                popped = true;
                
            }

            anim.Update(gameTime);
        }



        public override void Draw(SpriteBatch sb)
        {
            anim.Draw(sb, hitbox);
        }

        public override void OnCollision_H(GameObject obj)
        {
            if (obj != null && active)
            {
                (obj).Velocity = new Vector2((obj).Velocity.X, -200);
                hitbox = new Rectangle(0,0,0,0);
                popped = true;

                cooldown = 2;

                anim.ChangeAnimation(Animations.Pop, 0, true);
                anim.ChangeAnimation(Animations.None, 0, false);
            }
        }

        public override void OnCollision_V(GameObject obj)
        {
            OnCollision_H(obj);
        }
    }
}
