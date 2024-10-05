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
        LookLeft,
        LookRight,
        WalkLeft,
        WalkRight,
        JumpLeft,
        JumpRight,
        CrouchLeft,
        CrouchRight,
        Pushing,
        Idle,


    }
    internal class Player : Collidable
    {
        public static Texture2D spriteSheet;
        public static Keys lastPressed;
        private PlayerMovement state;
        private Vector2 position;
        private Vector2 velocity;
        private Vector2 acceleration;

        public bool Grounded{ get { return false; } }

        public Player(Vector2 position, Rectangle rect) : base (position, rect)
        {
            acceleration.Y = 10;
        }

        public void Update(GameTime gameTime)
        {
            bool pressed = false;
            var kState = Keyboard.GetState();

            if (kState.IsKeyDown(Keys.A)) {

                acceleration.X = -40;
                lastPressed = Keys.A;
                pressed = true;
            } else if(kState.IsKeyDown(Keys.D)){
                acceleration.X = 40;
                lastPressed = Keys.D;
                pressed = true;
            } else if (kState.IsKeyDown(Keys.Space)) {
                velocity.Y = -40;
                lastPressed = Keys.Space;
                pressed = true;
            } else if (kState.IsKeyDown (Keys.S) && Grounded) {
                lastPressed = Keys.S;
                pressed = true;
            }

            if (!pressed) {
                switch (lastPressed)
                {
                    case Keys.A:
                        acceleration.X = 10;
                        break;
                    case Keys.D:
                        acceleration.X = -10;
                        break;

                }
            }

            velocity += acceleration;
            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Debug.WriteLine(velocity.ToString());
            Debug.WriteLine(velocity.ToPoint().ToString());
        }
       
        public void Draw()
        {

        }
    }
}
