﻿using Microsoft.Xna.Framework.Graphics;
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
        Standing,
        LookBack,
        Walking,
        Jumping,
        Hiding,
        Crouching,
        Idle,
        Pushing

    }
    internal class Player : GameObject
    {
        public static Texture2D spriteSheet;
        public static Keys lastPressed;
        private PlayerMovement state;
        public static Animation<PlayerMovement> animation;

        public override int CollisionAccuracy
        {
            get { return 20;  }
        }

        public Player(Vector2 position) : base (position, new Rectangle(0, 0, 75, 44))
        {
            acceleration.Y = 70;
            Camera.Focus = this;
            animation = new("player.txt", spriteSheet);
        }

        public override void Update(GameTime gt)
        {

        }

        public void Update(GameTime gameTime, KeyboardState kState, KeyboardState prev)
        {
            if (Grounded)
            {
                acceleration.Y = 0;
            }
            else
            {
                acceleration.Y = gravity;
            }

            if (kState.IsKeyDown(Keys.T) && !prev.IsKeyDown(Keys.T)) 
            {
                ShootBubble();
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

                faceDirection = FaceDirection.Right;
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

            UpdateAnimations(kState);
            animation.Update(gameTime);
            base.Update(gameTime);
            
        }
        
        public void UpdateAnimations(KeyboardState kb)
        {
            if (!Grounded && state != PlayerMovement.Jumping)
            {
                animation.ChangeAnimation(PlayerMovement.Jumping, (int)faceDirection, true);
                return;
            }

            if (acceleration.X > 0 && velocity.X > 0)
            {
                faceDirection = FaceDirection.Right;
            }
            else if (acceleration.X < 0 && velocity.X < 0)
            {
                faceDirection = FaceDirection.Left;
            }

            if (kb.IsKeyDown(Keys.S) && state != PlayerMovement.Crouching)
            {
                animation.ChangeAnimation(PlayerMovement.Crouching, (int)faceDirection, true);
            }

            else if (velocity.X != 0 && state != PlayerMovement.Pushing)
            {
                animation.ChangeAnimation(PlayerMovement.Walking, (int)faceDirection, true);
            }
            else
            {
                animation.ChangeAnimation(PlayerMovement.Standing, (int)faceDirection);
            }

            
        }
        


        public void ShootBubble()
        {
            Vector2 bubblePos = new Vector2();
            if(state.Equals(FaceDirection.Left))
            {
                bubblePos = new Vector2(position.X - 100, position.Y);
            } else
            {
                bubblePos = new Vector2(position.X + 100, position.Y);
                

            }
            new Bubble(bubblePos, new Rectangle(0, 0, 40, 40));
        }
     

        public override void Draw(SpriteBatch sb)

        {
            //sb.Draw(spriteSheet, new Rectangle(Camera.RelativePosition(position).ToPoint(), hitbox.Size), new Rectangle(0, 0, 32, 32), Color.White);
            sb.DrawString(Game1.font, velocity.X.ToString(), new Vector2(0,0), Color.Black);
            sb.DrawString(Game1.font, acceleration.X.ToString(), new Vector2(0, 100), Color.Black);
            sb.DrawString(Game1.font, CollisionAccuracy.ToString(), new Vector2(100, 0), Color.Black);
            animation.Draw(sb, hitbox);
        }

    }
}
