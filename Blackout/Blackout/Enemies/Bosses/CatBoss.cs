using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blackout.Enemies.Mobs;
using Blackout.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Blackout.Enemies
{
    class CatBoss : Enemy
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
        public const int SIZE = 128;
        public const int SPEED = 2;
        //random
        public static Random rand = new Random();
        public int switchDirectionTime = 0;
        public Vector2 speed;
        private float bulletHeading;

        //screen dimensions
        public int screenW;
        public int screenH;

        private int fireTimer;
        private int catTimer;

        public CatBoss(Game game, Vector2 startingPosition)
        {
            //tex
            this.tex = game.Content.Load<Texture2D>("catboss");
            this.bulletTex = game.Content.Load<Texture2D>("mortimerProjectile");
            //Rectangles
            rectangle = new Rectangle((int)startingPosition.X, (int)startingPosition.Y, SIZE, SIZE);
            sourceRectangle = new Rectangle(0, 0, 128, 128);

            bullets = new List<Bullet>();

            // rand = new Random();

            //screen dimensions
            this.screenW = 800;
            this.screenH = 500;

            speed = new Vector2(1, 1);

            fireTimer = 0;
            catTimer = 0;
        }

        public void Update(Level level, GamePadState newPad, Mortimer player, float mx, float my, bool isChangingX, bool isChangingY)
        {
            Tile[,] tiles = level.tiles;
            bool hitATileWallX = false;
            bool hitATileWallY = false;
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    Rectangle leftRect = new Rectangle((int)(rectangle.X), (int)(rectangle.Y + 5), 5, rectangle.Height - 10);
                    Rectangle rightRect = new Rectangle((int)(rectangle.X + rectangle.Width - 5), (int)(rectangle.Y + 5), 5, rectangle.Height - 10);
                    Rectangle topRect = new Rectangle((int)(rectangle.X + 5), (int)(rectangle.Y), rectangle.Width - 10, 5);
                    Rectangle bottomRect = new Rectangle((int)(rectangle.X + 5), (int)(rectangle.Y + rectangle.Height - 5), rectangle.Width - 10, 5);
                    Rectangle tileRect = new Rectangle((int)(tiles[i, j].x), (int)(tiles[i, j].y), Tile.TILE_SIZE, Tile.TILE_SIZE);
                    if (tiles[i, j].tileState == TileState.IMPASSABLE && (tileRect.Intersects(leftRect) || tileRect.Intersects(rightRect))) hitATileWallX = true;
                    if (tiles[i, j].tileState == TileState.IMPASSABLE && (tileRect.Intersects(topRect) || tileRect.Intersects(bottomRect))) hitATileWallY = true;
                }
            }


            if (hitATileWallX) speed.X *= -1;
            if (hitATileWallY) speed.Y *= -1;

            if (switchDirectionTime == 0)
            {
                switchDirectionTime = rand.Next(30, 400);
                int x = rand.Next(1, 101);
                int y = rand.Next(1, 101);
                if (x > 50)
                    speed.X *= -1;
                if (y > 50)
                    speed.Y *= -1;

                speed.X = (speed.X / Math.Abs(speed.X)) * ((int) (rand.NextDouble() * 3) + 1);
                speed.Y = (speed.Y / Math.Abs(speed.Y)) * ((int)(rand.NextDouble() * 3) + 1);
            }
            else
                switchDirectionTime--;

            fireTimer++;
            if (fireTimer % 10 == 0)
            {
                double multiplier = fireTimer / 600.0 * 3;
                bulletHeading = (float)(bulletHeading + rand.NextDouble() * multiplier) % 360;
                bullets.Add(new Bullet(new Rectangle(rectangle.X+64, rectangle.Y+64, 16, 16), bulletTex, 3, bulletHeading));

            }

            // if (rectangle.Right >= screenW)
            //     speed.X *= -1;
            // else if (rectangle.Left <= 0)
            //     speed.X *= -1; ;
            //
            // if (rectangle.Y >= screenH - rectangle.Height)
            //     speed.Y *= -1;
            // else if (rectangle.Y <= 0)
            //     speed.Y *= -1;

            rectangle.X += (int)speed.X;
            rectangle.Y += (int)speed.Y;
            for (var i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update();
                if (bullets[i].rectangle.Intersects(player.rect))
                {
                    bullets[i] = null;
                }
            }
            while (bullets.Contains(null))
                bullets.Remove(null);

            for(int i=bullets.Count - 1; i>=0; i--)
            {
                bool collided = false;
                for (int x = 0; x < tiles.GetLength(0) & !collided; x++)
                {
                    for (int y = 0; y < tiles.GetLength(1) && !collided; y++)
                    {
                        Rectangle tileRect = new Rectangle((int)(tiles[x, y].x), (int)(tiles[x, y].y), Tile.TILE_SIZE, Tile.TILE_SIZE);
                        if (tiles[x, y].tileState == TileState.IMPASSABLE && tileRect.Intersects(bullets[i].rectangle)) collided = true;
                    }
                }

                if (collided)
                {
                    bullets[i] = null;
                }
                else
                {
                    bullets[i].Update();
                    bullets[i].relationalBulletUpdate(mx, my, isChangingX, isChangingY);
                }
            }
            while (bullets.Contains(null))
                bullets.Remove(null);
             
            if (catTimer == 1200)
            {
                catTimer = 0;
                for (int i = 0; i < 3; i++)
                    level.enemies.Add(new Cat(level.game, 
                        new Vector2(rectangle.X + rand.Next(-60, 60), rectangle.Y + rand.Next(-60, 60))));
            }

            catTimer++;
        }

        public Boolean isOnScreen()
        {
            return rectangle.X >= 0 && rectangle.Right <= screenW && rectangle.Y >= 0 && rectangle.Bottom <= screenH;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, rectangle, sourceRectangle, Color.White);
            foreach (Bullet bullet in bullets)
                spriteBatch.Draw(bulletTex, bullet.rectangle, Color.Red);
        }

    }
}
