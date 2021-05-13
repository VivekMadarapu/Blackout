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
        private LabelPrompt winPrompt;
        private Button playAgainButton;
        private MousePointer mousePointer;
        private Texture2D blankTex;

        private int timer, blackoutStartTimer;
        private bool blackingOut;

        public EndingScreen(GraphicsDevice graphicsDevice, MousePointer mouse)
        {
            playAgainButton = new Button(new Rectangle(187, 375, 125, 40), "Play Again");
            mousePointer = mouse;
            blankTex = new Texture2D(graphicsDevice, 1, 1);
            blankTex.SetData(new Color[] {Color.White});
            timer = 0; blackoutStartTimer = 0;
            blackingOut = false;

        }

        public void Update(Game1 game, GamePadState gamePad, GraphicsDeviceManager graphics)
        {
            graphics.PreferredBackBufferWidth = StartingScreen.TITLESCREEN_SIZE;
            graphics.PreferredBackBufferHeight = StartingScreen.TITLESCREEN_SIZE;
            graphics.ApplyChanges();
            mousePointer.Update(gamePad);
            playAgainButton.Update((float)mousePointer.x, (float)mousePointer.y);
            if (playAgainButton.overLapping && gamePad.Buttons.A == ButtonState.Pressed)
            {

                game.gameState = GameState.START;
                game.resetLevels();
            }
            if (timer % 200 == 0)
            {
                blackingOut = true;
                blackoutStartTimer = 1;
            }
            if (blackoutStartTimer == 7) blackingOut = false;
            else if (blackoutStartTimer == 15) blackingOut = true;
            else if (blackoutStartTimer == 25) blackingOut = false;
            else if (blackoutStartTimer == 40) blackingOut = true;
            else if (blackoutStartTimer == 60)
            {
                blackingOut = false;
                blackoutStartTimer = 0;
            }


            timer++;
            if (blackoutStartTimer != 0) blackoutStartTimer++;

        }
        public void setWinPrompt(bool won)
        {
            winPrompt = new LabelPrompt(new Vector2(185, 250), won ? "YOU WON" : "YOU LOST");
           
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(blankTex, new Rectangle(0, 0, 800, 600), Color.White * 0.5f);
            playAgainButton.Draw(spriteBatch);
            winPrompt.Draw(spriteBatch);
            mousePointer.Draw(spriteBatch);
            if (blackingOut) spriteBatch.Draw(Button.buttonImage, new Rectangle(0, 0, 500, 500), Color.Black);
        }
    }
}
