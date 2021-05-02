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
    class Lights
    {
        public Boolean lightsOff = false;
        public Boolean nightVision = false;
        public int lightCooldown = 0;
        public int darkRemaining = 0;
        public Texture2D texture;
        public Game game;
        public Lights(Game tempGame) {
            game = tempGame;
            loadContent(game);
        }
        //Loads the textureu
        public void loadContent(Game game)
        {
            texture = game.Content.Load<Texture2D>("hollowcircle");
        }
        //Takes the X and Y location of the mouse,and blacks everything out except for a small radius
      /*  public void Draw(SpriteBatch spriteBatch,int xPos,int yPos)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, new Rectangle(xPos-1350,yPos-1350,2700,2700),Color.White);
            spriteBatch.End();
        }*/
        public void checkIfLightsOff(SpriteBatch spriteBatch,int xPos,int yPos,int visionStrength) {
            if (lightCooldown > 0) { lightCooldown--; }
            if (lightCooldown == 0) {
                if (darkRemaining == 0) {
                    Random random = new Random();
                    int lightsOff = random.Next(1, 170);
                    if (lightsOff == 1)
                    {
                        darkRemaining = 660;
                    }
                }
                else
                {
                    try
                    {
                        spriteBatch.Begin();
                    }
                    catch {
                        spriteBatch.End();
                        spriteBatch.Begin();
                    }
                    if (visionStrength == 0) {
                        spriteBatch.Draw(texture, new Rectangle(xPos - 1350, yPos - 1350, 2700, 2700), Color.White);
                    } else if (visionStrength == 1) {
                        spriteBatch.Draw(texture, new Rectangle(xPos - 3100, yPos - 3100, 6200, 6200), Color.White);
                    }
                    /*{
                        spriteBatch.Draw(texture, new Rectangle(xPos - 3100, yPos - 3100, 6200, 6200), Color.White);
                    }
                    else {
                        spriteBatch.Draw(texture, new Rectangle(xPos - 1350, yPos - 1350, 2700, 2700), Color.White);
                    }*/
                    spriteBatch.End();
                    darkRemaining--;
                    if (darkRemaining ==0) { lightCooldown = 240; }
                }
            }
        }
        //Code used to generate a texture(may be used in the future)
        /*Texture2D createCircleText(int radius)
        {
            Texture2D texture = new Texture2D(graphics, radius, radius);
            Color[] colorData = new Color[radius * radius];

            float diam = radius / 2f;
            float diamsq = diam * diam;

            for (int x = 0; x < radius; x++)
            {
                for (int y = 0; y < radius; y++)
                {
                    int index = x * radius + y;
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }
            texture.SetData(colorData);
            return texture;
        }*/
    }
}
