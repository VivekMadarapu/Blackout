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

        //powerups
        PowerupManager powerupManager;

        public Level(SpriteBatch tempSpriteBatch,Game tempGame)
        {
            mapX = 0;
            mapY = 0;
            mortimerX = 200;
            mortimerY = 200;

            game = tempGame;

            tiles = new Tile[50, 50];
            offsets = new Vector2[6];

            spriteBatch = tempSpriteBatch;

        }
        public void mapMoved() { }
        public void Update(GamePadState gamePad)
        {
            double changeX = Math.Round(gamePad.ThumbSticks.Left.X * player.speed);
            double changeY = -Math.Round(gamePad.ThumbSticks.Left.Y * player.speed);
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

            //scrolls enemies with the map
            foreach (Enemy enemy in enemies)
            {
                if (enemy.GetType() == typeof(Cat))
                {
                    if (!hitATileWallX)
                    {
                        double move = ((Cat) enemy).rectangle.X + -changeX;
                        ((Cat)enemy).rectangle.X = (int)move;
                    }

                    if (!hitATileWallY)
                    {
                        double move = ((Cat) enemy).rectangle.Y + -changeY;
                        ((Cat)enemy).rectangle.Y = (int)move;
                    }
                    
                }
            }
            double tempYChange = changeY;
            double tempXChange = changeX;
            if (mortimerMovesInX || hitATileWallX) {
                tempXChange = 0;
            }
            if (mortimerMovesInY || hitATileWallY) {
                tempYChange = 0;
            }
          //  player.mortimerMoved(changeY, changeX);
            player.mortimerMoved(tempYChange, tempXChange);
            if (!hitATileWallX && mortimerMovesInX) mortimerX += mortimerChangeX;
            if (!hitATileWallY && mortimerMovesInY) mortimerY += mortimerChangeY;
            if (!hitATileWallX)
            {
                mapX += changeX;
            }
            if (!hitATileWallY) 
            {
                mapY += changeY;
            } 

            //bullet-enemy collision
            for (int i = 0; i < enemies.Count ;i++)
            {
                if (enemies[i].GetType() == typeof(Cat))
                {
                    for (int j = 0; j < player.bullets.Count; j++)
                    {
                        Bullet bullet = player.bullets[j];
                        if ((Cat)enemies[i] == null) break;
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

            //spawns entities from the entity map
            List<Vector2> locs = new List<Vector2>();
            List<String> types = new List<String>();
            using (StreamReader reader = new StreamReader(@"Content/EntityMap.txt"))
                {

                    for (int i = 0; i < tiles.GetLength(0); i++)
                    {
                        string line = reader.ReadLine();
                        string[] data = line.Split(' ');
                        for (int j = 0; j < tiles.GetLength(1); j++)
                        {
                            //decodes the id from the text file
                            int entityid = Convert.ToInt32(data[j]);

                            //interprets ids into entities
                            switch (entityid)
                            {
                             // entity ids
                                case 1:
                                    //offsets are in the map file. They offset the enemy position to match the position of the map.
                                    //Locations are loaded with the equations in the Vector2. They spawn them based on their locations in the entity map file and correspond with the tile locations in the map file. You can copy the equations directly for all entities.
                                    enemies.Add(new Cat(game, new Vector2(j*64-(int)offsets[0].X, i*64-(int)offsets[0].Y)));
                                    break;  
                              //add as many entities (enemies or powerups) as needed, but don't reuse ids
                                case 2://blue cheese
                                    types.Add("blue");
                                    locs.Add(new Vector2(j * 64 - (int)offsets[0].X, i * 64 - (int)offsets[0].Y));
                                    break;
                               case 3://white cheese
                                    types.Add("white");
                                    locs.Add(new Vector2(j * 64 - (int)offsets[0].X, i * 64 - (int)offsets[0].Y));
                                    break;
                               case 4://pink cheese
                                    types.Add("pink");
                                    locs.Add(new Vector2(j * 64 - (int)offsets[0].X, i * 64 - (int)offsets[0].Y));
                                    break;
                               case 5://green cheese
                                    types.Add("green");
                                    locs.Add(new Vector2(j * 64 - (int)offsets[0].X, i * 64 - (int)offsets[0].Y));
                                    break;
                               case 6://purple cheese
                                    types.Add("purple");
                                    locs.Add(new Vector2(j * 64 - (int)offsets[0].X, i * 64 - (int)offsets[0].Y));
                                    break;
                               case 7://yellow cheese
                                    types.Add("yellow");
                                    locs.Add(new Vector2(j * 64 - (int)offsets[0].X, i * 64 - (int)offsets[0].Y));
                                    break;
                               case 8://red cheese
                                types.Add("red");
                                locs.Add(new Vector2(j * 64 - (int)offsets[0].X, i * 64 - (int)offsets[0].Y));
                                break;

                        }
                        }
                    }
           
            }
            powerupManager = new PowerupManager(game, locs, types);
            player = new Mortimer(new Vector2(200, 200), game, powerupManager);

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
