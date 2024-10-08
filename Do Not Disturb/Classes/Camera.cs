﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;

namespace Do_Not_Disturb.Classes
{
    internal class Camera
    {

        public static Player Focus;
        public static Vector2 globalOffset;
        public static Vector2 OriginalFocus;
        

        public static Vector2 CameraOffset(Vector2 position)
        {
            return (position - Focus.Hitbox.Center.ToVector2()) + globalOffset;
        }

        public static Vector2 RelativePosition(Vector2 position)
        {
            return (position - Focus.Hitbox.Center.ToVector2()) * Game1.Scale + globalOffset;
        }

        public static Vector2 Parallax(float factor)
        {
            if (Focus == null)
            {
                return Vector2.Zero;
            }
            return new Vector2((-(Focus.Position - OriginalFocus) / factor).X, 0);
        }
    }
}
