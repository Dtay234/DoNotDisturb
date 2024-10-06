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
        Standing,
        LookBack,
        Walking,
        Jumping,
        Hiding,
        Crouching,
        Idle,
        Pushing

    }
    internal class Player : Collidable
    {
        public static Texture2D spriteSheet;
        public static Keys lastPressed;
        private PlayerMovement state;
        public static Animation<PlayerMovement> animation;

        public override int CollisionAccuracy
        {
            get { return 20;  }
        }

        public Player(Vector2 position) : base (position, new Rectangle((int)position.X, (int)position.Y, 55, 47))
        {
            gravity = 70;
            Camera.Focus = this;
            Camera.OriginalFocus = this.position;
            animation = new("player.txt", spriteSheet);
            animation.ChangeAnimation(PlayerMovement.Idle, (int)faceDirection, false);
            maxXVelocity = 50;
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
                {
                }
                    ShootBubble();
                
            }

            //Horizontal movement
            if (kState.IsKeyDown(Keys.A) &&
                !kState.IsKeyDown(Keys.D))
            {
                // acceleration is higher if the player is moving in the opposite direction for smoother movement
                
                acceleration.X = velocity.X > 0 ? -maxXVelocity * 5f : -maxXVelocity * 2f;

                faceDirection = FaceDirection.Left;
            }
            else if (kState.IsKeyDown(Keys.D) &&
                !kState.IsKeyDown(Keys.A))
            {
                // acceleration is higher if the player is moving in the opposite direction for smoother movement
                
                acceleration.X = velocity.X < 0 ? maxXVelocity * 5f : maxXVelocity * 2f;

                faceDirection = FaceDirection.Right;
            }


            //Jump 
            if ((kState.IsKeyDown(Keys.Space)))
            {
                if (Grounded)
                {
                    velocity.Y = -55;

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
            if (!Grounded)
            {
                if (state != PlayerMovement.Pushing && state != PlayerMovement.Idle)
                { animation.ChangeAnimation(PlayerMovement.Jumping, (int)faceDirection, true); 
                    state = PlayerMovement.Jumping; }
                if (state == PlayerMovement.Jumping && state != PlayerMovement.Idle)
                { animation.ChangeAnimation(PlayerMovement.Pushing, (int)faceDirection, false); 
                    state = PlayerMovement.Pushing; }
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
                state= PlayerMovement.Crouching;
            }

            else if (velocity.X != 0 && state != PlayerMovement.Pushing)
            {
                animation.ChangeAnimation(PlayerMovement.Walking, (int)faceDirection, true);
                state = PlayerMovement.Walking;
            }
            else
            {
                animation.ChangeAnimation(PlayerMovement.Standing, (int)faceDirection);
                state = PlayerMovement.Standing;
            }

            
        }
        


        public void ShootBubble()
        {
            if (Game1.objects.Exists(x => x is Bubble))
            {
                return;
            }

            Vector2 bubblePos = new Vector2();
            if(faceDirection == FaceDirection.Left)
            {
                bubblePos = new Vector2(position.X - 100, position.Y);
            } else
            {
                bubblePos = new Vector2(position.X + 100, position.Y);
                

            }
            new Bubble(bubblePos - new Vector2(35,35), new Rectangle(0, 0, 70, 70), 
                faceDirection);
        }
     

        public override void Draw(SpriteBatch sb)

        {
            //sb.Draw(spriteSheet, new Rectangle(Camera.RelativePosition(position).ToPoint(), hitbox.Size), new Rectangle(0, 0, 32, 32), Color.White);
            
            animation.Draw(sb, hitbox);
        }

        public override void OnCollision_H(GameObject obj)
        {
            if (Math.Sign(obj.Velocity.X * obj.Acceleration.X) >= 0)
            {
                velocity.X = obj.Acceleration.X / 4;
                acceleration.X = obj.Acceleration.X / 2;

                obj.Velocity = new Vector2(
                    //maxXVelocity * Math.Sign(obj.Acceleration.X) / 3, 
                    velocity.X,
                    obj.Velocity.Y);
            }

        }

        public override void OnCollision_V(GameObject obj)
        {
            if (velocity.X == 0)
                obj.Velocity = new Vector2(velocity.X + obj.Velocity.X, 0);
            else
            {

                obj.Velocity = new Vector2(velocity.X, velocity.Y < 0 ? velocity.Y : obj.Velocity.Y);
            }

        }

    }
}
