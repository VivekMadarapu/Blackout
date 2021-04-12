﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Blackout
{
    class Level
    {
        Tile[,] tiles;

        public static int WIDTH = 100, HEIGHT = 50;

        public Mortimer player;

        double mapX;
        double mapY;

        double gingerManX, gingerManY;

        public Level()
        {
            tiles = new Tile[50, 100];
        }

        public void Update(GamePadState gamePad)
        {
            double changeX = gamePad.ThumbSticks.Left.X * 6;
            double changeY = -gamePad.ThumbSticks.Left.Y * 6;

            if (changeX + mapX < 0 || gingerManX < 475 ||
                changeX + mapX + 400 > Tile.TILE_SIZE * WIDTH || gingerManX > 525)
            {
                if (gingerManX + player.tex.Width + changeX <= 400 && 
                    gingerManX + changeX >= 0)
                    gingerManX += changeX;
                changeX = 0;
            }

            if (changeY + mapY < 0 || gingerManY < 225 ||
                changeY + mapY + 400 > Tile.TILE_SIZE * HEIGHT || gingerManY > 275)
            {
                if (gingerManY + player.tex.Height + changeY <= 400 &&
                    gingerManY + changeY >= 0)
                    gingerManY += changeY;
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
        }

        public void loadContent(Microsoft.Xna.Framework.Game game, Game1 game1)
        {
            try
            {
                using (StreamReader reader = new StreamReader(@"Content/TileMap.txt"))
                {
                    for (int i = 0; i < tiles.GetLength(0); i++)
                    {
                        string line = reader.ReadLine();
                        for (int j = 0; j < tiles.GetLength(1); j++)
                        {
                            string tileName = line.Substring(j * 4, 3);
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
            
        }

    }
}
