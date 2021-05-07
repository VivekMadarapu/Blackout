using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blackout.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Blackout
{
    class EndZone
    {
        public Texture2D tex;
        
        public Rectangle rectangle;
        int counter = 0;
        float elapsedTime = 0;
        private int oldSec;

        //speed/dimensions
        public const int SIZE = 64;
        
        //random
        public Random rand;

        //screen dimensions
        public int screenW;
        public int screenH;

        private int fireTimer;


        public EndZone(Game game, Vector2 startingPosition)
        {
            //tex
            tex = new Texture2D(game.GraphicsDevice, 1, 1);
            this.tex.SetData(new Color[] { new Color(255, 255, 0, 100) });
            //Rectangles
            rectangle = new Rectangle((int)startingPosition.X, (int)startingPosition.Y, SIZE, SIZE);

            //screen dimensions
            this.screenW = 800;
            this.screenH = 500;

        }

        public void Update(Level level, GamePadState newPad, Mortimer player)
        {
            if (rectangle.Intersects(player.rect))
            {
                
            }

        }

        public Boolean isOnScreen()
        {
            return rectangle.X >= 0 && rectangle.Right <= screenW && rectangle.Y >= 0 && rectangle.Bottom <= screenH;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, rectangle, Color.White);
        }

    }
}
