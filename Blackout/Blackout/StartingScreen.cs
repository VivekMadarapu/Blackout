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
    public class StartingScreen
    {
        public static int TITLESCREEN_SIZE = 500;
        public static Texture2D titleImage;
        public Vector2 titleImageContainer;
        public Rectangle sourceRectangle;
        public MousePointer mousePointer;
        Button settingsButton, startGameButton;
        private int timer, blackoutStartTimer;
        private bool blackingOut;

        public StartingScreen(GraphicsDeviceManager graphics)
        {
            timer = 0; blackoutStartTimer = 0;
            blackingOut = false;
            titleImage = null;
            titleImageContainer = new Vector2(75, 150);
            sourceRectangle = new Rectangle(0, 0, 361, 49);
            graphics.PreferredBackBufferWidth = TITLESCREEN_SIZE;
            graphics.PreferredBackBufferHeight = TITLESCREEN_SIZE;
            graphics.ApplyChanges();
            mousePointer = new MousePointer(400, 400, TITLESCREEN_SIZE, TITLESCREEN_SIZE);
            settingsButton = new Button(new Rectangle(100, 400, 100, 25), "Controls");
            startGameButton = new Button(new Rectangle(300, 400, 100, 25), "Play");
        }

        public static void loadTitleScreenImage(Microsoft.Xna.Framework.Game game)
        {
            titleImage = game.Content.Load<Texture2D>("BlackoutLogo");
        }

        public void Update(Game1 game, GraphicsDeviceManager graphics, GamePadState gamePad, GamePadState oldPad)
        {
            // if (sourceRectangle.Width < titleImage.Width) sourceRectangle.Width++;
            mousePointer.Update(gamePad);
            settingsButton.Update((float)mousePointer.x, (float)mousePointer.y);
            startGameButton.Update((float)mousePointer.x, (float)mousePointer.y);
            if (settingsButton.overLapping && gamePad.Buttons.A == ButtonState.Pressed) game.gameState = GameState.SETTINGS;
            if (startGameButton.overLapping && gamePad.Buttons.A == ButtonState.Pressed)
            {
                game.gameState = GameState.LEVEL_ONE;
                graphics.PreferredBackBufferWidth = 800;
                graphics.PreferredBackBufferHeight = 600;
                graphics.ApplyChanges();
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

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(titleImage, titleImageContainer, sourceRectangle, Color.White);
            settingsButton.Draw(spriteBatch);
            startGameButton.Draw(spriteBatch);
            mousePointer.Draw(spriteBatch);
            if (blackingOut) spriteBatch.Draw(Button.buttonImage, new Rectangle(0, 0, 500, 500), Color.Black);
        }
    }



    public class SettingsScreen
    {
        public static String controls =
            "Controls/Rules: \n\n" +
            "Left Thumbstick - move up and down\n" +
            "Left Trigger - Thrust\n" +
            "X - Reverse\n" +
            "Y - Fire\n" +
            "B - Drop Humanoid\n\n" +
            "Point Values:\n" +
            "Lander 150\n" +
            "Bomber 150\n" +
            "Mutant 150\n" +
            "Humanoid falls without dying 250\n" +
            "Catching Humanoid 500\n" +
            "Returning Humanoid to Ground 500\n";

        public LabelPrompt settingsPrompt;
        public Button backButton;
        public MousePointer mousePointer;

        public SettingsScreen(MousePointer mousePointer)
        {
            settingsPrompt = new LabelPrompt(new Vector2(50, 50), controls);
            backButton = new Button(new Rectangle(370, 450, 100, 25), "Back");
            this.mousePointer = mousePointer;
        }

        public void Update(Game1 game, GamePadState gamePad, GamePadState oldPad)
        {
            mousePointer.Update(gamePad);
            backButton.Update((float)mousePointer.x, (float)mousePointer.y);
            if (backButton.overLapping && gamePad.Buttons.A == ButtonState.Pressed) game.gameState = GameState.START;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            backButton.Draw(spriteBatch);
            settingsPrompt.Draw(spriteBatch);
            mousePointer.Draw(spriteBatch);
        }
    }
}
