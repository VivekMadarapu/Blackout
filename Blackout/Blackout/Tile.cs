﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blackout
{
    class Tile
    {
        public static String DEFAULT_TILE = "n1";
        public static int TICKS_PER_FRAME = 4;
        public static int TILE_SIZE = 64;
        Texture2D tileTexture;
        int animationFrames;
        public double x, y;
        Rectangle sourceRectangle;
        int animationCounter;
        public TileState tileState;
        

        public Tile(string texName, Game1 game, int row, int col, int offsetX, int offsetY)
        {
            if (texName != "   ")
            {
                tileTexture = game.Content.Load<Texture2D>(texName);
                if (texName == "w1")
                    tileState = TileState.IMPASSABLE;
                else
                    tileState = TileState.PASSABLE;
            }
            else
            {
                tileTexture = game.Content.Load<Texture2D>(DEFAULT_TILE);
                tileState = TileState.IMPASSABLE;
            }
            animationFrames = tileTexture.Width / TILE_SIZE;
            sourceRectangle = new Rectangle(0, 0, TILE_SIZE, TILE_SIZE);
            x = col * TILE_SIZE + offsetX;
            y = row * TILE_SIZE + offsetY;
            animationCounter = 0;
        }

        public void Update(double xChange, double yChange)
        {
            x += xChange;
            y += yChange;

            if (animationCounter % TICKS_PER_FRAME == 0)
                sourceRectangle.X = (sourceRectangle.X + TILE_SIZE) % tileTexture.Width;
            animationCounter++;
        }

        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tileTexture, new Rectangle((int)x, (int)y, TILE_SIZE, TILE_SIZE),
                sourceRectangle, Color.White * ((tileState == TileState.PASSABLE) ? 0.75f : 1f));
        }
    }

    public enum TileState
    {
        PASSABLE, IMPASSABLE,
    }

}
