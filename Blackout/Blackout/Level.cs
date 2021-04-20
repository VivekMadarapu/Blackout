using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Blackout.Enemies;
using Blackout.Enemies.Mobs;
using Blackout.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Blackout
{
    class Level
    {
        Tile[,] tiles;

        public static int WIDTH = 50, HEIGHT = 50;

        public Mortimer player;
        public SpriteBatch spriteBatch;
        private int playerTexHeight = 31, playerTexWidth = 31;

        public List<Enemy> enemies = new List<Enemy>();
        public static Vector2[] offsets; 

        double mapX;
        double mapY;
        Game game;
        double mortimerX, mortimerY;

        public Level(SpriteBatch tempSpriteBatch,Game tempGame)
        {
            mapX = 0;
            mapY = 0;
            mortimerX = 200;
            mortimerY = 200;

            game = tempGame;
            player = new Mortimer(new Vector2(200, 200),tempSpriteBatch,game);

            tiles = new Tile[50, 50];
            offsets = new Vector2[6];

            spriteBatch = tempSpriteBatch;
        }

        public void Update(GamePadState gamePad)
        {
            double changeX = gamePad.ThumbSticks.Left.X * 4;
            double changeY = -gamePad.ThumbSticks.Left.Y * 4;
            double mortimerChangeX = changeX;
            double mortimerChangeY = changeY;
            bool mortimerMovesInX = false;
            bool mortimerMovesInY = false;
          
            if (changeX + mapX < 0 || mortimerX < 475 ||
                changeX + mapX + 1000 > Tile.TILE_SIZE * WIDTH || mortimerX > 525)
            {
                if (mortimerX + playerTexWidth + changeX <= 1000 &&
                    mortimerX + changeX >= 0)
                {
                    mortimerMovesInX = true;
                }
                changeX = 0;         
            }
          
            if (changeY + mapY < 0 || mortimerY < 225 ||
                changeY + mapY + 700 > Tile.TILE_SIZE * HEIGHT || mortimerY > 275)
            {
                if (mortimerY + playerTexHeight + changeY <= 700 &&
                    mortimerY + changeY >= 0)
                {
                    mortimerMovesInY = true;
                }
                changeY = 0;
            }

            bool hitATileWallX = false;
            bool hitATileWallY = false;
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    Rectangle leftRect = new Rectangle((int)(mortimerX + mortimerChangeX), (int)(mortimerY + mortimerChangeY + 5), 5, player.rect.Height - 10);
                    Rectangle rightRect = new Rectangle((int)(mortimerX + mortimerChangeX + player.rect.Width - 5), (int)(mortimerY + mortimerChangeY + 5), 5, player.rect.Height - 10);
                    Rectangle topRect = new Rectangle((int)(mortimerX + mortimerChangeX + 5), (int)(mortimerY + mortimerChangeY), player.rect.Width - 10, 5);
                    Rectangle bottomRect = new Rectangle((int)(mortimerX + mortimerChangeX + 5), (int)(mortimerY + mortimerChangeY + player.rect.Height - 5), player.rect.Width - 10, 5);
                    Rectangle tileRect = new Rectangle((int)(tiles[i, j].x - changeX), (int)(tiles[i, j].y - changeY), Tile.TILE_SIZE, Tile.TILE_SIZE);
                    if (tiles[i, j].tileState == TileState.IMPASSABLE && (tileRect.Intersects(leftRect) || tileRect.Intersects(rightRect))) hitATileWallX = true;
                    if (tiles[i, j].tileState == TileState.IMPASSABLE && (tileRect.Intersects(topRect) || tileRect.Intersects(bottomRect))) hitATileWallY = true;
                     
                }
            }
            //if (!hitATileWallX || !hitATileWallY)
            //{
                for (int i = 0; i < tiles.GetLength(0); i++)
                {
                    for (int j = 0; j < tiles.GetLength(1); j++)
                    {
                        tiles[i, j].Update((hitATileWallX) ? 0 : -changeX, (hitATileWallY) ? 0 : -changeY);
                    }
                }
            //}
            if (!hitATileWallX && mortimerMovesInX) mortimerX += mortimerChangeX;
            if (!hitATileWallY && mortimerMovesInY) mortimerY += mortimerChangeY;
            if (!hitATileWallX)
            {
                mapX += changeX;
                foreach (Enemy enemy in enemies)
                {
                    if (enemy.GetType() == typeof(Cat))
                    {
                        ((Cat)enemy).rectangle.X += (int)-changeX;
                    }
                }
            }
            if (!hitATileWallY) 
            {
                mapY += changeY;
                foreach (Enemy enemy in enemies)
                {
                    if (enemy.GetType() == typeof(Cat))
                    {
                        ((Cat)enemy).rectangle.Y += (int)-changeY;
                    }
                }
            } 

            //bullet-enemy collision
            for (int i = 0; i < enemies.Count ;i++)
            {
                if (enemies[i].GetType() == typeof(Cat))
                {
                    for (int j = 0; j < player.bullets.Count; j++)
                    {
                        Projectiles.Bullet bullet = player.bullets[j];
                        if (((Cat)enemies[i]).rectangle.Intersects(bullet.rectangle))
                        {
                            enemies[i] = null;
                            player.bullets.Remove(bullet);
                        }
                    }
                }
            }
            //removes null instances in the enemies array
            while (enemies.Contains(null))
                enemies.Remove(null);

            player.rect.X = (int)mortimerX;
            player.rect.Y = (int)mortimerY;
            player.Update(gamePad, tiles);
        }

        public void loadContent(Game1 game)
        {
            
                using (StreamReader reader = new StreamReader(@"Content/TileMap.txt"))
                {
                    string[] offStrings = reader.ReadLine().Split(' ');
                    offsets[0] = new Vector2(Convert.ToInt32(offStrings[0]), Convert.ToInt32(offStrings[1]));
                    mapX = offsets[0].X;
                    mapY = offsets[0].Y;
                    for (int i = 0; i < tiles.GetLength(0); i++)
                    {
                        string line = reader.ReadLine();
                        string[] data = line.Split(' ');
                        for (int j = 0; j < tiles.GetLength(1); j++)
                        {
                            string tileName = data[j];
                            tiles[i, j] = new Tile(tileName, game, i, j, (int)-mapX, (int)-mapY);
                        }
                    }
                }
                using (StreamReader reader = new StreamReader(@"Content/EntityMap.txt"))
                {
                    for (int i = 0; i < tiles.GetLength(0); i++)
                    {
                        string line = reader.ReadLine();
                        string[] data = line.Split(' ');
                        if (data.Length > 50)
                        {
                            Console.WriteLine("Yes");
                        }
                        for (int j = 0; j < tiles.GetLength(1); j++)
                        {
                            int entityid = Convert.ToInt32(data[j]);
                            switch (entityid)
                            {
                                case 1:
                                    enemies.Add(new Cat(game, new Vector2(j*64-(int)offsets[0].X, i*64-(int)offsets[0].Y)));
                                    break;

                                //fill in ids for all enemies and powerups

                            }
                        }
                    }
                }

                player.loadContent(game);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    tiles[i, j].Draw(spriteBatch);
                }
            }

            foreach (Enemy enemy in enemies)
            {
                if (enemy.GetType() == typeof(Cat))
                {
                    ((Cat)enemy).Draw(spriteBatch);
                }
            }

            player.Draw(spriteBatch);
        }

    }
}
