using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Blackout.Projectiles
{
    public class Bullet : Projectile
    {

      // public Direction direction;//up==0//down==1//left==2//right==3
        public Vector2 speed;
        public Vector2 loc;
        float direction;//radians

        //health system --> 
        public Bullet(Rectangle rectangle, Texture2D texture, int speed, float direct) : base(rectangle, texture)
        {
            //d accounts for messups in direction
            this.rectangle = rectangle;
            loc = new Vector2(rectangle.X, rectangle.Y);
            this.texture = texture;
            //sets direction and speed of the bullet
            direction = direct;
            this.speed = new Vector2((float)(speed * Math.Cos(direction)), (float)(speed * Math.Sin(direction)* -1));
        }
        public void adjustSpeed(Vector2 d, int bulletSpeed)//for morty
        {
            if(d.Y!=0 && d.X==0)
            {
                if (d.Y > 0)
                    speed = new Vector2(0, -1 * bulletSpeed);
                else
                    speed = new Vector2(0, bulletSpeed);
            }
            else if(d.X!=0 && d.Y==0)
            {
                if (d.X > 0)
                    speed = new Vector2(bulletSpeed, 0);
                else
                    speed = new Vector2(-1 * bulletSpeed, 0);
            }
            else if(d.X==0 && d.Y==0)
            {
                speed = new Vector2(0, 0);
            }
         
        }
        public void Update()//updates individual bullets
        {

            if (!hasHit)
            {
                
                loc.X += speed.X;
                loc.Y += speed.Y;
               
            }
            rectangle.X = (int)loc.X;
            rectangle.Y = (int)loc.Y;
           
        }
        public void relationalBulletUpdate(float mx, float my)
        {
            loc.X += mx;
            loc.Y += my;
        }
        public void Draw(SpriteBatch spriteBatch)//draws individual bullets
        {
            if (!hasHit)
                spriteBatch.Draw(texture, rectangle, Color.White);
        }
   
    }
}
