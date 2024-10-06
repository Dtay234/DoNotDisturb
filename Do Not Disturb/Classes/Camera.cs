using System;
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
            return (position - Focus.Hitbox.Center.ToVector2()) + globalOffset;
        }

        public static Vector2 Parallax(float factor)
        {
            return -(Focus.Position - OriginalFocus) / factor;
        }
    }
}
