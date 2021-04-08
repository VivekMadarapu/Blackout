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
        private bool hasHit;

        public Projectile(Rectangle rectangle, Texture2D texture)
        {
            this.rectangle = rectangle;
            this.texture = texture;
        }


    }
}
