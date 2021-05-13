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
        public string tempEffect = "";


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
        public int bulletDamage = 10;
        //necessary gamepadcontrols
        public GamePadState oldPad;


        //turning mouse direction
        public float direction;

        //other
        Lights lights;
        public PowerupManager powerupManager;
        Game game;

        int prevX = 0;
        int prevY = 0;

        //power bars
        Bar healthBar;

        public Mortimer(Vector2 loc,Game tempGame,PowerupManager powerupManager): base(50,50,20)
        {   
            this.loc = loc;
            color = Color.White;
            rect = new Rectangle((int)loc.X, (int)loc.Y, 50, 50);
            sourceRect = new Rectangle(0, 0, 31, 31);

            //bullets
            bullets = new List<Projectiles.Bullet>();
            bulletDirection = (float)Math.PI / 2;
            oldPad = GamePad.GetState(PlayerIndex.One);

            //direction
            direction = 0;
            
            //powerup stuff
        
            game = tempGame;

            //powerbar
            healthBar = new Bar(game, new Vector2(0, 0), 100, 20, health, health, Color.Green);



            this.powerupManager = powerupManager;
            //double[,] locs = new double[,] { { 100, 100 }, { 500, 100 }, { 600, 100 }, { -50, 50 }, { -40, 200 } };
            //string[] types = new string[] { "white", "pink", "pink", "red", "pink", "red", "pink" };
            //powerupManager = new PowerupManager(game, spriteBatch, locs, types);
        }
        public void setOldPad(GamePadState oldPad)
        {
            this.oldPad = oldPad;
        }
        public void mortimerMoved(double yDir,double xDir) {
            if(prevX != rect.X) {
                xDir = 0;
            }
            if (prevY != rect.Y) {
                yDir = 0;
            }
          //  string tempEffect = "white";
          //  string tempEffect = powerupManager.updatePowerups(yDir, xDir, rect.X, rect.Y);
            prevX = rect.X;
            prevY = rect.Y;
        }
        public void Update(GamePadState newPad, Tile[,] tiles)
        {
            //location update
            loc.X = rect.X;
            loc.Y = rect.Y;
            //direction update
            if(newPad.ThumbSticks.Left.Y!=0 || newPad.ThumbSticks.Left.X!=0)
            {

                if (newPad.ThumbSticks.Left.X == 0)
                    direction = (float)(Math.PI / 2) + (float)Math.Atan2(newPad.ThumbSticks.Left.Y, newPad.ThumbSticks.Left.X);
                else if (newPad.ThumbSticks.Left.Y == 0)
                    direction = (-1) * (float)(Math.PI / 2) + (float)Math.Atan2(newPad.ThumbSticks.Left.Y, newPad.ThumbSticks.Left.X);
                else
                {
                    direction = (float)Math.Atan2(newPad.ThumbSticks.Left.Y, newPad.ThumbSticks.Left.X);
                    if((newPad.ThumbSticks.Left.Y>0 && newPad.ThumbSticks.Left.X>0)|| (newPad.ThumbSticks.Left.Y < 0 && newPad.ThumbSticks.Left.X < 0))
                    {
                        direction +=(float) Math.PI;
                    }
                }
            }

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
            for(int i=bullets.Count - 1; i>=0; i--)
            {
                bool collided = false;
                for (int x = 0; x < tiles.GetLength(0) & !collided; x++)
                {
                    for (int y = 0; y < tiles.GetLength(1) && !collided; y++)
                    {
                        Rectangle tileRect = new Rectangle((int)(tiles[x, y].x), (int)(tiles[x, y].y), Tile.TILE_SIZE, Tile.TILE_SIZE);
                        if (tiles[x, y].tileState == TileState.IMPASSABLE && tileRect.Intersects(bullets[i].rectangle)) collided = true;
                    }
                }

                if (collided)
                {
                    bullets.Remove(bullets[i]);
                }
                 else bullets[i].Update();
            }

            //cooldown for bullets
            if (bulletCooldown > 0)
                bulletCooldown--;

            //effects
            //other
            string tempEffect = powerupManager.updatePowerups(0, 0, rect.X, rect.Y);
            //prevX = rect.X;
            //prevY = rect.Y;
            switch (tempEffect)
            {
                case "white":
                    effect = tempEffect;
                    effectLength = 180000;
                    break;
                case "yellow":
                    effect = tempEffect;
                    break;
                case "red":
                    effect = tempEffect;
                    break;
                case "blue":
                    effect = tempEffect;
                    effectLength = 1800;
                    break;
                case "green":
                    effect = tempEffect;
                    effectLength = 1;
                    break;
            }
            Boolean nightMode = false;
            if (effectLength > 0)
            {
                if (effect.Equals("white"))
                {
                    nightMode = true;
                }
                else if (effect.Equals("blue"))
                {
                    speed = 8;
                }
                else if(effect.Equals("green"))
                {
                    health += 20;
                }
                effectLength--;

            }
            else
            {
                speed = 4;
            }
            lights.checkIfLightsOff(rect.X + 31, rect.Y + 31, nightMode);

            //healtbar;
            healthBar.update(health);

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
            double[,] locs = new double[,] { { 100, 100 }};
            string[] types = new string[] {"white"};
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
            //Rectangle rect2 = new Rectangle(rect.X + rect.Width / 2, rect.Y + rect.Height / 2, rect.Width, rect.Height);
            powerupManager.Draw(spriteBatch);
            spriteBatch.Draw(tex, rect, sourceRect, color, direction, new Vector2(sourceRect.Width/2, sourceRect.Height/2), SpriteEffects.None, 0);
            //spriteBatch.Draw(tex, rect, sourceRect, color);
            //bullets
            for(int i=0; i<bullets.Count; i++)
            {
                bullets[i].Draw(spriteBatch);
            }

            powerupManager.Draw(spriteBatch);

            lights.Draw(spriteBatch);


            //powerups
            healthBar.Draw(spriteBatch);
        }
    }
}
