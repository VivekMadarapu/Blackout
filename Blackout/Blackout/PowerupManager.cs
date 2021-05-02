﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Blackout.Enemies;
using Blackout.Projectiles;
using System;
using System.Collections.Generic;

namespace Blackout
{
    class PowerupManager
    {
        Texture2D yellowTexture;
        Texture2D whiteTexture;
        Texture2D blueTexture;
        Texture2D redTexture;
        Texture2D pinkTexture;
        Texture2D purpleTexture;
        Texture2D greenTexture;
        Texture2D finalTexture;
        SpriteBatch spriteBatch;

        public Texture2D greyTexture;
        public Texture2D blackTexture;

        Game game;
        List<Vector2> powerupLoc;
        List<string> powerupType;
        public bool powerupApplied = false;
      //  double[,] powerupLoc;
       // string[] powerupType;
        //Game and SpriteBatch are used for basic functionality. powerupLocTemp stores powerup locations,and powerupTypeTemp stores types
        public PowerupManager(Game gameTemp,SpriteBatch spriteBatchTemp,List<Vector2> powerupLocTemp,List<string> powerupTypeTemp) {
            game = gameTemp;
            spriteBatch = spriteBatchTemp;
            powerupLoc = powerupLocTemp;
            powerupType = powerupTypeTemp;
            loadContent(game);
        }
        public void togglePowerupApplied(bool state) {
            powerupApplied = state;
        }
        //Loads the textures for the powerups
        public void loadContent(Game game)
        {
            yellowTexture = game.Content.Load<Texture2D>("yellowcheese");
            whiteTexture = game.Content.Load<Texture2D>("whitecheese");
            blueTexture = game.Content.Load<Texture2D>("bluecheese");
            redTexture = game.Content.Load<Texture2D>("redcheese");
            pinkTexture = game.Content.Load<Texture2D>("pinkcheese");
            purpleTexture = game.Content.Load<Texture2D>("purplecheese");
            greenTexture = game.Content.Load<Texture2D>("greencheese");
            blackTexture = game.Content.Load<Texture2D>("blacksquare");
            greyTexture = game.Content.Load<Texture2D>("greysquare");
        }
        public Texture2D getPowerUpTexture(String effect) {
            Texture2D tempTexture;
            switch (effect)
            {
                case "yellow":
                    tempTexture = yellowTexture;
                    break;
                case "white":
                    tempTexture = whiteTexture;
                    break;
                case "red":
                    tempTexture = redTexture;
                    break;
                case "blue":
                    tempTexture = blueTexture;
                    break;
                case "pink":
                    tempTexture = pinkTexture;
                    break;
                case "purple":
                    tempTexture = purpleTexture;
                    break;
                case "green":
                    tempTexture = greenTexture;
                    break;
                default:
                    tempTexture = yellowTexture;
                    break;
            }
            return tempTexture;
        }
        //Draws the powerups,and checks for collisions
        public string updatePowerups(int yMovement,int xMovement,double playerX,double playerY) {
            //Stores effec to apply to player,stays empty if there aren't collisions
            string effect = "";
            string finalEffect = "";
            //Loops through the powerups
            for (int x = 0; x < powerupLoc.Count; x++)
            {
                //Moves the powerups depending on player movement
                //powerupLoc[x, 0] -= yMovement;
                //powerupLoc[x, 1] -= xMovement;
                Vector2 tempVector = powerupLoc[x];
                tempVector.Y -= yMovement;
                tempVector.X -= xMovement;
                powerupLoc[x] = tempVector;
               //Creates a rectangle to store powerup position
                Rectangle powerupRectangle = new Rectangle((int)powerupLoc[x].X, (int)powerupLoc[x].Y, 54, 33);
                //Drawing takes place after this
                try
                {
                    spriteBatch.Begin();
                }
                catch {
                    spriteBatch.End();
                    spriteBatch.Begin();
                }
                //Gets the effect for the current powerup
                effect = powerupType[x];
                //checks the effect value and loads the correct texture
                finalTexture = getPowerUpTexture(effect);
              
                //Draws the powerup
                spriteBatch.Draw(finalTexture, powerupRectangle, Color.White);
                //Checks if the player intersects with the powerup
                if (yMovement == 0 && xMovement == 0)
                {
                    if (powerupRectangle.Intersects(new Rectangle((int)playerX, (int)playerY, 20, 30)))
                    {
                       // if (finalEffect.Equals(""))
                        //{
                            finalEffect = effect;
                        //}
                        //finalEffect = effect;
                        //used for cloning the array,and removing an element
                        int indexInArray = 0;
                        //clones location and type arrays to temp variables
                        /*double[,] tempPowerUpLocs = (double[,])powerupLoc.Clone();
                        string[] tempPowerupType = (string[])powerupType.Clone();*/
                        powerupLoc.RemoveAt(x);
                        powerupType.RemoveAt(x);
                        //shrinks location and type arrays
                        /* powerupLoc = new double[tempPowerUpLocs.Length / 2 - 1, 2];
                         powerupType = new string[powerupType.Length - 1];*/
                        //loops through both arrays
                        /*for (int y = 0; y < tempPowerUpLocs.Length/2;y++) {
                            //executes unless the powerup needs to be removed
                            if (y != x) {
                                //copies a location from tempPowerUpLocs to powerupLoc
                                powerupLoc[indexInArray, 0] = tempPowerUpLocs[y, 0];
                                powerupLoc[indexInArray, 1] = tempPowerUpLocs[y, 1];
                                //copies a type from tempPowerupType to powerupType
                                powerupType[indexInArray] = tempPowerupType[y];
                                //updates this for the next iteration
                                indexInArray++;
                            }
                        }*/
                    }
                }
                spriteBatch.End();
            }
            //returns the effect applied to the player          
            return finalEffect;
        }
    }
}
