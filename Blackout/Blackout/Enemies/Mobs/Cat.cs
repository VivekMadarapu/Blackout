using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blackout.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Blackout.Enemies.Mobs
{
    class Cat : Enemy
    {
        public Texture2D tex;
        public Texture2D bulletTex;

        public Rectangle rectangle;
        public Rectangle sourceRectangle;
        int counter = 0;
        float elapsedTime = 0;
        private int oldSec;

        //fireballs
        public List<Bullet> bullets;

        //speed/dimensions
        public const int SIZE = 64;
        public const int SPEED = 5;
        //random
        public Random rand;
        public int switchDirectionTime = 0;
        public Vector2 speed;

        //screen dimensions
        public int screenW;
        public int screenH;

        private int fireTimer;


        public Cat(Game game, Vector2 startingPosition)
        {
            //tex
            this.tex = game.Content.Load<Texture2D>("Mortimer");
            this.bulletTex = game.Content.Load<Texture2D>("mortimerProjectile");
            //Rectangles
            rectangle = new Rectangle((int)startingPosition.X, (int)startingPosition.Y, SIZE, SIZE);
            sourceRectangle = new Rectangle(0, 0, 31, 31);

            bullets = new List<Bullet>();

            rand = new Random();

            //screen dimensions
            this.screenW = 800;
            this.screenH = 500;

            speed = new Vector2(2, 2);

            fireTimer = 120;
        }

        public void Update(double gameTime, Level level, GamePadState newPad, Mortimer player)
        {
            if (switchDirectionTime == 0)
            {
                switchDirectionTime = rand.Next(30, 120);
                int x = rand.Next(1, 101);
                int y = rand.Next(1, 101);
                if (x > 50)
                    speed.X *= -1;
                if (y > 50)
                    speed.Y *= -1;
            }
            else
                switchDirectionTime--;

            if (fireTimer <= 0 && isOnScreen())
            {
                bullets.Add(new Bullet(new Rectangle(), bulletTex, 1, 0));
                fireTimer = 120;
            }
            
            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].rectangle.Y > screenW+10 || bullets[i].rectangle.Y < -10 || bullets[i] == null)
                    bullets[i] = null;
                else
                    bullets[i].Update();
            }
            while (bullets.Contains(null))
                bullets.Remove(null);

            fireTimer--;

            if (rectangle.Right >= screenW)
                speed.X *= -1;
            else if (rectangle.Left <= 0)
                speed.X *= -1; ;

            if (rectangle.Y >= screenH - rectangle.Height)
                speed.Y *= -1;
            else if (rectangle.Y <= 0)
                speed.Y *= -1;

            rectangle.X += (int)speed.X;
            rectangle.Y += (int)speed.Y;

        }

        public Boolean isOnScreen()
        {
            return rectangle.X >= 0 && rectangle.Right <= screenW && rectangle.Y >= 0 && rectangle.Bottom <= screenH;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, rectangle, sourceRectangle, Color.White);
            foreach (Bullet bullet in bullets)
                spriteBatch.Draw(tex, bullet.rectangle, Color.White);
        }
    }
}
