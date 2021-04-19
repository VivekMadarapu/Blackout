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
    class Mortimer : AnimatedSprite
    {
        public int effectLength = 0;
        public int health = 100;
        public string effect = "";
        public Texture2D tex;
        public Vector2 loc;
        public Color color;
        public Rectangle rect;
        public Rectangle sourceRect;
        Lights lights;
        PowerupManager powerupManager;
        SpriteBatch spriteBatch;
        Game game;

        public Mortimer(Vector2 loc,SpriteBatch tempSpriteBatch,Game tempGame): base(50,50,20)
        {
           
            this.loc = loc;
            color = Color.White;
            rect = new Rectangle((int)loc.X, (int)loc.Y, 50, 50);
            sourceRect = new Rectangle(0, 0, 31, 31);
            spriteBatch = tempSpriteBatch;
            game = tempGame;
            double[,] locs = new double[,] { { 500, 100 }, { 600, 100 }, { -50, 50 }, { -40, 200 } };
            string[] types = new string[] { "white", "pink", "pink", "red", "pink", "red", "pink" };
            powerupManager = new PowerupManager(game, spriteBatch, locs, types);
        }
        //public void Update(Level level)
        //{
        //    GamePadState newPad = GamePad.GetState(PlayerIndex.One);
        //    level.Update(newPad);
        //}

        public void loadContent(Game game)
        {
            double[,] locs = new double[,] { { 500, 100 }, { 600, 100 }, { -50, 50 }, { -40, 200 } };
            string[] types = new string[] { "white", "pink", "pink", "red", "pink", "red", "pink" };
           // powerupManager = new PowerupManager(game, spriteBatch, locs, types);
            lights = new Lights(game);
            tex = game.Content.Load<Texture2D>("Mortimer");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, rect, sourceRect, color);
            string tempEffect = powerupManager.updatePowerups(0, 0, 200, 0);
            switch (tempEffect) {
                case "white":
                    effect = "white";
                    effectLength = 1800;
                    break;
            }
            Boolean nightMode = false;
            if (effectLength > 0)
            {
                if (effect.Equals("white"))
                {
                    nightMode = true;
                }
                effectLength--;
            }
            lights.checkIfLightsOff(spriteBatch, 100, 100,nightMode);
        }
    }
}