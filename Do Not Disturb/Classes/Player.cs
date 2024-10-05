using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

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
    internal class Player
    {
        public static Texture2D spriteSheet;
        private PlayerMovement state;
        private Vector2 position;
        private Vector2 velocity;
        private Vector2 accerlation;
       
    }
}
