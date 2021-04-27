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
        
        GameState gameState;
       
        Level levelOne;
        Lights lights;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 700;
            graphics.ApplyChanges();
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
            gameState = GameState.LEVEL_ONE;
            Game game = this;
            //levelOne = new Level(spriteBatch,this);
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
            Game gameWORKIBEG = this;
            levelOne = new Level(spriteBatch, this);
            levelOne.loadContent(this);
            lights = new Lights(this);
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
            if (gameState == GameState.LEVEL_ONE)
            {
               // powerupManager.updatePowerups(0, 0, 200, 0);

                levelOne.Update(gamePadState);

            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            if (gameState == GameState.LEVEL_ONE)
            {
                levelOne.Draw(spriteBatch);
            }
            try
            {
                spriteBatch.End();
            }
            catch {
                spriteBatch.Begin();
                spriteBatch.End();
            }
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
    }

    public enum GameState
    {
        START, LEVEL_ONE, LEVEL_TWO, BOSS_LEVEL_ONE, LEVEL_FOUR, LEVEL_FIVE, BOSS_LEVEL_TWO, END,
    }
}
