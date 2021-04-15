using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blackout.Projectiles
{
    public class Projectile
    {
        public Rectangle rectangle;
        public Texture2D texture;
        public bool hasHit;

        public Projectile(Rectangle rectangle, Texture2D texture)
        {
            this.rectangle = rectangle;
            this.texture = texture;
            hasHit = false;
        }
        public Boolean collidesWith(Rectangle rect2)
        {
            if (rectangle.Intersects(rect2))
                hasHit = true; 

            return hasHit;
        }


    }
}
