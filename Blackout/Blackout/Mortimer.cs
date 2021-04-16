using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Blackout.Projectiles
{
    class Mortimer : AnimatedSprite
    {
        //drawing stuff
        public Texture2D tex;
        public Vector2 loc;
        public Color color;
        public Rectangle rect;
        public Rectangle sourceRect;
        //morty speed
        public int speed = 4;
        //bullet stuff
        public Texture2D bulletTex;
        public List<Bullet> bullets;
        public int bulletSpeed = 10;
        public float bulletDirection;
        public int bulletSize = 10;
        public int bulletCooldown = 0;
        //necessary gamepadcontrols
        public GamePadState oldPad;
     
        public Mortimer(Vector2 loc): base(50,50,20)
        {
           
            this.loc = loc;
            color = Color.White;
            rect = new Rectangle((int)loc.X, (int)loc.Y, 50, 50);
            sourceRect = new Rectangle(0, 0, 31, 31);

            //bullets
            bullets = new List<Bullet>();
            bulletDirection = (float)Math.PI / 2;
            oldPad = GamePad.GetState(PlayerIndex.One);
        }
        public void setOldPad(GamePadState oldPad)
        {
            this.oldPad = oldPad;
        }
        public void Update(GamePadState newPad)
        {
            //location update
            loc.X = rect.X;
            loc.Y = rect.Y;

            //bullets
            bulletDirection = (float)Math.Atan2((double)newPad.ThumbSticks.Right.Y, (double)(newPad.ThumbSticks.Right.X));
            if (newPad.Triggers.Right==1 && oldPad.Triggers.Right!=1)
            {
               
                bullets.Add(new Bullet(new Rectangle((int)loc.X + rect.Width / 2, (int)loc.Y + rect.Height / 2, bulletSize, bulletSize), bulletTex, bulletSpeed, (float)bulletDirection));
                bullets[bullets.Count - 1].adjustSpeed(new Vector2(newPad.ThumbSticks.Right.X, newPad.ThumbSticks.Right.Y), bulletSpeed);
                if (bullets[bullets.Count - 1].speed.X == 0 && bullets[bullets.Count - 1].speed.Y == 0)
                    bullets.RemoveAt(bullets.Count - 1);
                bulletCooldown = 10;
            }
            for(int i=0; i<bullets.Count; i++)
            {
                bullets[i].Update();
            }

            //cooldown for bullets
            if (bulletCooldown > 0)
                bulletCooldown--;

            oldPad = newPad;
        }
        public void relationalUpdate(float mx, float my)
        {
           for(int i=0; i<bullets.Count; i++)
            {
                bullets[i].relationalBulletUpdate(mx, my);
            }
        }
        public void loadContent(Game game)//load textures
        {
            tex = game.Content.Load<Texture2D>("Mortimer");
            bulletTex = game.Content.Load<Texture2D>("mortimerProjectile");

        }
        public Boolean mortyCollision(Rectangle rect2)//collisions for mortimer
        {
            return rect.Intersects(rect2);

        }
        public void Draw(SpriteBatch spriteBatch)//draw mortimer and bullets
        {
            spriteBatch.Draw(tex, rect, sourceRect, color);
            for(int i=0; i<bullets.Count; i++)
            {
                bullets[i].Draw(spriteBatch);
            }

        }
    }
}