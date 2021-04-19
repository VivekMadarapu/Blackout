using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Blackout.Enemies;
using Blackout.Enemies.Mobs;
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
                     
                //    Rectangle rect1 = new Rectangle((int)(mortimerX + mortimerChangeX), (int)(mortimerY + mortimerChangeY), player.rect.Width, player.rect.Height);
                //    Rectangle rect2 = new Rectangle((int)(tiles[i, j].x - changeX), (int)(tiles[i, j].y - changeY), Tile.TILE_SIZE, Tile.TILE_SIZE);
                //    if (tiles[i, j].tileState == TileState.IMPASSABLE && rect1.Intersects(rect2) /* || (rect1.X > rect2.X && rect1.X < rect2.X + rect2.Width)*/)
                //    {
                //        hitATileWallX = true;
                //    }
                //    if (tiles[i, j].tileState == TileState.IMPASSABLE && ((rect1.Bottom > rect2.Top && rect1.Bottom < rect2.Bottom) || (rect1.Top > rect2.Top && rect1.Top < rect2.Bottom)))
                //    {
                //        hitATileWallY = true;
                //    }
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
            if (!hitATileWallX) mapX += changeX;
            if (!hitATileWallY) mapY += changeY;
            player.rect.X = (int)mortimerX;
            player.rect.Y = (int)mortimerY;
        }

        public void loadContent(Game1 game)
        {
            try
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
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading file: ");
                Console.WriteLine(e.Message);
                Console.WriteLine(tiles.GetLength(0) + " " + tiles.GetLength(1));
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
                    ((Cat)enemy).Draw();
                }
            }

            player.Draw(spriteBatch);
        }

    }
}
