using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blackout.Projectiles
{
    public class Bullet : Projectile
    {
        public Bullet(Rectangle rectangle, Texture2D texture) : base(rectangle, texture)
        {
            this.rectangle = rectangle;
            this.texture = texture;
        }
    }
}
