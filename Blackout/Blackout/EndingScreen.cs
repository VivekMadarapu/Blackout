using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Blackout
{

    class EndingScreen
    {
        public bool wonGame;
        private Button playAgainButton;
        private MousePointer mousePointer;
        private Texture2D blankTex;

        public EndingScreen(GraphicsDevice graphicsDevice)
        {
            playAgainButton = new Button(new Rectangle(100, 400, 100, 25), "Play Again");
            mousePointer = new MousePointer(50, 50, 800, 600);
            blankTex = new Texture2D(graphicsDevice, 1, 1);
            blankTex.SetData(new Color[] {Color.White});
        }

        public void Update(Game1 game, GamePadState gamePad, SpriteBatch spriteBatch)
        {
            mousePointer.Update(gamePad);
            playAgainButton.Update((float)mousePointer.x, (float)mousePointer.y);
            if (playAgainButton.overLapping && gamePad.Buttons.A == ButtonState.Pressed)
            {
                game.gameState = GameState.SETTINGS;
                game.resetLevels();
            }

            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(blankTex, new Rectangle(0, 0, 800, 600), Color.White * 0.5f);
            playAgainButton.Draw(spriteBatch);
            mousePointer.Draw(spriteBatch);
        }
    }
}
