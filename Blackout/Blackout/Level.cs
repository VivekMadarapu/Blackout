using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Blackout.Projectiles;

namespace Blackout
{
    class Level
    {
        Tile[,] tiles;

        public static int WIDTH = 50, HEIGHT = 50;

        public Mortimer player;

        private int playerTexHeight = 31, playerTexWidth = 31;

        double mapX;
        double mapY;

        double mortimerX, mortimerY;

        public Level()
        {
            mapX = 0;
            mapY = 0;
            mortimerX = 500;
            mortimerY = 400;
          
            player = new Mortimer(new Vector2(500, 400));

            tiles = new Tile[50, 50];
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
                    player.relationalUpdate((float)changeX, (float)changeY);
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
                    player.relationalUpdate((float)changeX, (float)changeY);
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

            //update mortimer in relation to bullets and stuff
            player.Update(gamePad);
        }

        public void loadContent(Game game, Game1 game1)
        {
            try
            {
                using (StreamReader reader = new StreamReader(@"Content/TileMap.txt"))
                {
                    for (int i = 0; i < tiles.GetLength(0); i++)
                    {
                        string line = reader.ReadLine();
                        string[] data = line.Split(' ');
                        for (int j = 0; j < tiles.GetLength(1); j++)
                        {
                            string tileName = data[j];
                            tiles[i, j] = new Tile(tileName, game1, i, j);
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

            player.loadContent(game1);
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
            player.Draw(spriteBatch);
        }

    }
}
