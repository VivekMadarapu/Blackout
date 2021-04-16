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
            mortimerX = 200;
            mortimerY = 200;

            player = new Mortimer(new Vector2(200, 200));
         

            tiles = new Tile[50, 50];
        }

        public void Update(GamePadState gamePad)
        {
            double changeX = gamePad.ThumbSticks.Left.X * 4;
            double changeY = -gamePad.ThumbSticks.Left.Y * 4;

            if (changeX + mapX < 0 || mortimerX < 475 ||
                changeX + mapX + 1000 > Tile.TILE_SIZE * WIDTH || mortimerX > 525)
            {
                if (mortimerX + playerTexWidth + changeX <= 1000 &&
                    mortimerX + changeX >= 0)
                {
                    mortimerX += changeX;
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
                    mortimerY += changeY;
                    player.relationalUpdate((float)changeX, (float)changeY);
                }
             
                changeY = 0;
            }

            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    tiles[i, j].Update(-changeX, -changeY);
                }
            }

            mapX += changeX;
            mapY += changeY;
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
