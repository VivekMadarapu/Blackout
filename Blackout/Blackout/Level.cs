using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blackout
{
    class Level
    {
        Tile[,] tiles;

        public Level()
        {
            tiles = new Tile[50, 100];
        }

        public void Update()
        {
            
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
