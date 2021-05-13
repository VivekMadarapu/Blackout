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
using Blackout.Enemies;
using Blackout.Projectiles;

namespace Blackout
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        public GameState gameState;

        private StartingScreen startingScreen;
        private SettingsScreen settingsScreen;
        private Lights lights;
        PowerupManager powerupManager;

        Level[] levels;

        private EndingScreen endingScreen;


        private GamePadState oldPadState;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            gameState = GameState.START;
            Game game = this;

            // levelOne = new Level(spriteBatch,this);
            startingScreen = new StartingScreen(graphics);
            settingsScreen = new SettingsScreen(startingScreen.mousePointer);
            endingScreen = new EndingScreen(GraphicsDevice);
            oldPadState = GamePad.GetState(PlayerIndex.One);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            resetLevels();

            MousePointer.loadPointerImage(this);
            Button.loadContent(this);
            StartingScreen.loadTitleScreenImage(this);
            LabelPrompt.loadSpriteFont(this);
            /*locs is a list of the coords of all the powerups,coords are like (Y,X)
             * types stores the powerup type for each powerup. 
             */
            /*double[,] locs = new double[,] { { 500, 100 }, { 600, 100 }, { -50, 50 }, { -40, 200 } };
            string[] types = new string[] {"white", "pink","pink","red","pink","red","pink"};
            powerupManager = new PowerupManager(this,spriteBatch,locs,types);*/
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            if (gameState == GameState.START) startingScreen.Update(this, graphics, gamePadState, oldPadState);
            else if (gameState == GameState.SETTINGS) settingsScreen.Update(this, gamePadState, oldPadState);
            else if (gameState == GameState.END) endingScreen.Update(this, gamePadState, spriteBatch);
            if (gameState != GameState.START && gameState != GameState.SETTINGS && gameState != GameState.END)
            {
                // powerupManager.updatePowerups(0, 0, 200, 0);

                levels[(int)gameState - 2].Update(gameTime, gamePadState, gameState);

                bool canAdvance = true;
                foreach (var task in levels[(int) gameState - 2].tasks)
                {
                    if (!task.hasCompleted)
                    {
                        canAdvance = false;
                    }
                }
                if (canAdvance)
                {
                    EndZone[] winAreaInstance = new EndZone[levels[(int)gameState-2].winArea.Count];
                    levels[(int) gameState - 2].winArea.CopyTo(winAreaInstance);
                    foreach (var endzone in winAreaInstance)
                    {
                        if (levels[(int) gameState - 2].player.rect.Intersects(endzone.rectangle))
                        {
                            gameState++;
                            break;
                        }
                    }
                }

                if (gamePadState.DPad.Up == ButtonState.Pressed && oldPadState.DPad.Up == ButtonState.Released)
                {
                    gameState++;
                }
                else if (gamePadState.DPad.Down == ButtonState.Pressed && oldPadState.DPad.Down == ButtonState.Released)
                {
                    gameState++;
                }

            }

            oldPadState = gamePadState;
            // powerupManager.updatePowerups(0, 0, 200, 0);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            if (gameState == GameState.START) startingScreen.Draw(spriteBatch);
            else if (gameState == GameState.SETTINGS) settingsScreen.Draw(spriteBatch);
            else if (gameState != GameState.START && gameState != GameState.SETTINGS)
            {
                levels[(int)gameState - 2].Draw(spriteBatch);
                if (gameState == GameState.END) endingScreen.Draw(spriteBatch);
            }
            spriteBatch.End();
            /*This shuts off the light randomly for 11 seconds each time
            The parameters require spriteBatch,x position of mouse,and y position of mouse(center pos not top left)
            Uncomment line below to test*/
            //lights.checkIfLightsOff(spriteBatch,100,100);

            /*This moves the powerups when the player moves and checks for collisions
             * When there is a collision,the powerup value is returned(yellow,red,etc)
             * Parameters:yMovement(pixels moved in vertical direction),xMovement,playerX(Mortimer x coord),playerY
             */
            //String effect = powerupManager.updatePowerups(0, 0, 200, 0);
            base.Draw(gameTime);
        }

        public void resetLevels()
        {
            levels = new Level[3];
            for (int i = 0; i < levels.Length; i++)
            {
                levels[i] = new Level(spriteBatch, this);
                levels[i].loadContent(this, "TileMap" + (i + 1) + ".txt", "EntityMap" + (i + 1) + ".txt");
            }
            lights = new Lights(this);
        }
    }

    public enum GameState
    {
        START, SETTINGS, LEVEL_ONE, LEVEL_TWO, BOSS_LEVEL_ONE, LEVEL_FOUR, LEVEL_FIVE, BOSS_LEVEL_TWO, END,
    }
}
