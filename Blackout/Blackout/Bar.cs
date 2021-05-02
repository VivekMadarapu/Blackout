using System;
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

        /// <summary>
        /// Creates the bar with the specifications
        /// </summary>
        /// <param name="game">The game object</param>
        /// <param name="startingPosition">position to spawn the bar</param>
        /// <param name="width">maximum width of the bar</param>
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

            this.width = width;
            this.height = height;

            //scales bar based on maximum width
            curValue = startingValue;
            barRect = new Rectangle((int)startingPosition.X, (int)startingPosition.Y, width*startingValue/maxValue, height);
            backRect = new Rectangle((int)startingPosition.X, (int)startingPosition.Y, width, height);

            //screen dimensions
            this.screenW = 800;
            this.screenH = 500;

        }

        public void update(int value)
        {
            if (curValue+value <= maxValue && curValue+value >= 0)
                curValue += value;
        }

        public Boolean isOnScreen()
        {
            return backRect.X >= 0 && backRect.Right <= screenW && backRect.Y >= 0 && backRect.Bottom <= screenH;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backTex, backRect, Color.White);
            spriteBatch.Draw(barTex, barRect, Color.White);
        }

    }
}
