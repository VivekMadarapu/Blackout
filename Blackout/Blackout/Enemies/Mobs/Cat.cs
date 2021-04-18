using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blackout.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blackout.Enemies.Mobs
{
    class Cat : Enemy
    {
        private int health;
        private int baseRotation = 0;

        private Texture2D texture;
        public int textureWidth;
        public int textureHeight;

        public Rectangle rect;
        private Rectangle sourceRect;

        // Bullet bullet = new Bullet();
        private int delay;
        private int soundTime = 0;

        public Cat(Game1 game, Rectangle rect)
        {
            this.texture = game.Content.Load<Texture2D>("cat");
            this.rect = rect;
            //TODO: Add cat texture
            this.sourceRect = new Rectangle(0, 0, 0, 0);
        }
    }
}
