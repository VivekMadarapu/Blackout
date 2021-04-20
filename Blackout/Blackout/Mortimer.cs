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
        
         //morty speed
        public int speed = 4;
        //bullet stuff
        public Texture2D bulletTex;
        public List<Projectiles.Bullet> bullets;
        public int bulletSpeed = 10;
        public float bulletDirection;
        public int bulletSize = 10;
        public int bulletCooldown = 0;
        //necessary gamepadcontrols
        public GamePadState oldPad;
        
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

            //bullets
            bullets = new List<Projectiles.Bullet>();
            bulletDirection = (float)Math.PI / 2;
            oldPad = GamePad.GetState(PlayerIndex.One);
            
            spriteBatch = tempSpriteBatch;
            game = tempGame;
            double[,] locs = new double[,] { { 500, 100 }, { 600, 100 }, { -50, 50 }, { -40, 200 } };
            string[] types = new string[] { "white", "pink", "pink", "red", "pink", "red", "pink" };
            powerupManager = new PowerupManager(game, spriteBatch, locs, types);
        }
        public void setOldPad(GamePadState oldPad)
        {
            this.oldPad = oldPad;
        }
        public void Update(GamePadState newPad)
        {
            //location update
            loc.X = rect.X;
            loc.Y = rect.Y;

            //bullets
            bulletDirection = (float)Math.Atan2((double)newPad.ThumbSticks.Right.Y, (double)(newPad.ThumbSticks.Right.X));
            if (newPad.Triggers.Right==1 && oldPad.Triggers.Right!=1)
            {
               
                bullets.Add(new Projectiles.Bullet(new Rectangle((int)loc.X + rect.Width / 2, (int)loc.Y + rect.Height / 2, bulletSize, bulletSize), bulletTex, bulletSpeed, (float)bulletDirection));
                bullets[bullets.Count - 1].adjustSpeed(new Vector2(newPad.ThumbSticks.Right.X, newPad.ThumbSticks.Right.Y), bulletSpeed);
                if (bullets[bullets.Count - 1].speed.X == 0 && bullets[bullets.Count - 1].speed.Y == 0)
                    bullets.RemoveAt(bullets.Count - 1);
                bulletCooldown = 10;
            }
            for(int i=0; i<bullets.Count; i++)
            {
                bullets[i].Update();
            }

            //cooldown for bullets
            if (bulletCooldown > 0)
                bulletCooldown--;

            oldPad = newPad;
        }
        public void relationalUpdate(float mx, float my)
        {
           for(int i=0; i<bullets.Count; i++)
            {
                bullets[i].relationalBulletUpdate(mx, my);
            }
        }
        public void loadContent(Game game)
        {
            double[,] locs = new double[,] { { 500, 100 }, { 600, 100 }, { -50, 50 }, { -40, 200 } };
            string[] types = new string[] { "white", "pink", "pink", "red", "pink", "red", "pink" };
           // powerupManager = new PowerupManager(game, spriteBatch, locs, types);
            lights = new Lights(game);
            tex = game.Content.Load<Texture2D>("Mortimer");
            bulletTex = game.Content.Load<Texture2D>("mortimerProjectile");

        }
        public Boolean mortyCollision(Rectangle rect2)//collisions for mortimer
        {
            return rect.Intersects(rect2);

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, rect, sourceRect, color);
            //bullets
            for(int i=0; i<bullets.Count; i++)
            {
                bullets[i].Draw(spriteBatch);
            }
            //other
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
            // lights.checkIfLightsOff(spriteBatch, rect.X+31, rect.Y+31,nightMode);
        }
    }
}
