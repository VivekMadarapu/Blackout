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
    class Task
    {
        public Texture2D tex;
        private SpriteFont font;

        public Rectangle rectangle;
        int counter = 0;
        float elapsedTime = 0;
        private int oldSec;
        public Bar progressBar;

        //speed/dimensions
        public const int SIZE = 32;
        
        //timer
        public int timeRemaining;
        public bool hasCompleted;

        //screen dimensions
        public int screenW;
        public int screenH;

        public Task(Game game, Vector2 startingPosition)
        {
            //tex
            tex = game.Content.Load<Texture2D>("ic1");
            //Rectangles
            rectangle = new Rectangle((int)startingPosition.X+16, (int)startingPosition.Y+16, SIZE, SIZE);

            progressBar = new Bar(game, new Vector2(startingPosition.X - 16, startingPosition.Y - 16), 4, 32, 0, 50, Color.Green);

            //screen dimensions
            this.screenW = 800;
            this.screenH = 500;

            //time to complete tasks
            timeRemaining = 5*60;

        }

        public void Update(Level level, GamePadState newPad, Mortimer player)
        {
            if (rectangle.Intersects(player.rect) && newPad.IsButtonDown(Buttons.X) && !hasCompleted)
            {
                if (timeRemaining <= 0)
                {
                    hasCompleted = true;
                }
                else
                {
                    timeRemaining--;
                    if (timeRemaining%60 == 0)
                    {
                        progressBar.update(10);
                    }
                }
            }
        }

        public Boolean isOnScreen()
        {
            return rectangle.X >= 0 && rectangle.Right <= screenW && rectangle.Y >= 0 && rectangle.Bottom <= screenH;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, rectangle, Color.White);
            if (!hasCompleted)
            {
                progressBar.Draw(spriteBatch);
            }
        }
    }
}
