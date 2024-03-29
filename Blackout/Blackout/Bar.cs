﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Blackout
{
    class Bar
    {
        //texture for the bar
        public Texture2D barTex;
        //texture for the background (unfilled portion of the bar)
        public Texture2D backTex;
        //font for displaying healthbar values
        public SpriteFont font;
        
        //rectangle for the bar
        public Rectangle barRect;
        //rectangle for the background
        public Rectangle backRect;

        //dimensions of the bar
        public int width;
        public int height;

        //screen dimensions
        public int screenW;
        public int screenH;

        //The intial value of the bar
        public int curValue;
        //The maximum value the bar can reach
        public int maxValue;
        //Color of the bar
        public Color barColor;

        /// <summary>
        /// Creates the bar with the specifications
        /// </summary>
        /// <param name="game">The game object</param>
        /// <param name="startingPosition">position the bar will be placed at (update the X/Y of both barRect and backRect if you want to move the bar's position)</param>
        /// <param name="width">width of the bar</param>
        /// <param name="height">height of the bar</param>
        /// <param name="startingValue">value the bar should initially start at</param>
        /// <param name="maxValue">maximum value of the bar</param>
        /// <param name="barColor">color of the filled in portion of the bar</param>
        public Bar(Game game, Vector2 startingPosition, int width, int height, int startingValue, int maxValue, Color barColor)
        {
            //textures
            barTex = new Texture2D(game.GraphicsDevice, 1, 1);
            this.barTex.SetData(new Color[] { barColor });
            backTex = new Texture2D(game.GraphicsDevice, 1, 1);
            this.backTex.SetData(new Color[] { Color.LightGray });
            this.font = game.Content.Load<SpriteFont>("SpriteFont1");
            this.barColor = barColor;
            this.maxValue = maxValue;

            this.width = width;
            this.height = height;

            //scales bar based on maximum width
            curValue = startingValue;
            barRect = new Rectangle((int)startingPosition.X, (int)startingPosition.Y, width*startingValue/maxValue, height);
            backRect = new Rectangle((int)startingPosition.X, (int)startingPosition.Y, width, height);

            //screen dimensions
            this.screenW = 1000;
            this.screenH = 700;

        }

        /// <summary>
        /// Updates the current value of the bar 
        /// </summary>
        /// <param name="value">negative numbers decrease the value and positive numbers increase the value</param>
        public void update(int value)
        {
            if (curValue+value <= maxValue && curValue+value >= 0)
                curValue += value;
            barRect.Width = width * curValue / maxValue;
        }

        public Boolean isOnScreen()
        {
            return backRect.X >= 0 && backRect.Right <= screenW && backRect.Y >= 0 && backRect.Bottom <= screenH;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch">spritebatch to render the healthbar with</param>
        /// <param name="stats">Whether to display the bar values over the bar</param>
        public void Draw(SpriteBatch spriteBatch, bool stats)
        {
            spriteBatch.Draw(backTex, backRect, Color.White);
            spriteBatch.Draw(barTex, barRect, Color.White);
            if(stats)
                spriteBatch.DrawString(font, curValue + " / " + maxValue, new Vector2(backRect.X, backRect.Y), Color.White);
        }

    }
}
