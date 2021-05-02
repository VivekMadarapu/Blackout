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
        public bool extraDamage = false;
        int visionLevel = 0;

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

        public Texture2D greyTexture;
        public Texture2D blackTexture;
        public Texture2D greenTexture;

        int speedyTasks = 0;
        Boolean invisible = false;

        int prevX = 0;
        int prevY = 0;

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
            //double[,] locs = new double[,] { { 100, 100 }, { 500, 100 }, { 600, 100 }, { -50, 50 }, { -40, 200 } };
            //string[] types = new string[] { "white", "pink", "pink", "red", "pink", "red", "pink" };
           /* List<Vector2> locs = new List<Vector2>{new Vector2(100,100),new Vector2(100,500),new Vector2(100,600),new Vector2(50,-50),new Vector2(200,-40)};
            List<String> types = new List<String> {"white","pink","pink","red","pink","pink","pink" };
            powerupManager = new PowerupManager(game, spriteBatch, locs, types);*/
        }
        public void setOldPad(GamePadState oldPad)
        {
            this.oldPad = oldPad;
        }
        public void mortimerMoved(int yDir,int xDir) {
            if(prevX != rect.X) {
                xDir = 0;
            }
            if (prevY != rect.Y) {
                yDir = 0;
            }
          //  string tempEffect = "white";
              string tempEffect = powerupManager.updatePowerups(yDir, xDir, rect.X, rect.Y);
            prevX = rect.X;
            prevY = rect.Y;
        }
        public void Update(GamePadState newPad, Tile[,] tiles)
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
            /*double[,] locs = new double[,] { { 100, 100 }};
            string[] types = new string[] { "white"};*/
            List<Vector2> locs = new List<Vector2> { new Vector2(100, 150), new Vector2(100, 300), new Vector2(100, 600), new Vector2(50, -50), new Vector2(200, -40) };
            List<String> types = new List<String> { "purple", "yellow", "pink", "red", "pink", "pink", "pink" };
            powerupManager = new PowerupManager(game, spriteBatch, locs, types);
            // powerupManager = new PowerupManager(game, spriteBatch, locs, types);
            lights = new Lights(game);
            tex = game.Content.Load<Texture2D>("Mortimer");
            bulletTex = game.Content.Load<Texture2D>("mortimerProjectile");
            blackTexture = game.Content.Load<Texture2D>("blacksquare");
            greyTexture = game.Content.Load<Texture2D>("greysquare");
            greenTexture = game.Content.Load<Texture2D>("greensquare");
        }
        public Boolean mortyCollision(Rectangle rect2)//collisions for mortimer
        {
            return rect.Intersects(rect2);

        }
        public Boolean areTasksSpeedy() {
            Boolean dataToReturn = speedyTasks > 0;
            if (speedyTasks>0) { speedyTasks--; }
            if (speedyTasks == 0) { effectLength = 0;effect = ""; }
            return dataToReturn;
        }
        public Boolean isMortyStrong() {
            return extraDamage;
        }
        public Boolean isMortimerInvisible()
        {
            return invisible;
        }
        public void clearEffect() {
            if (health > 100) { health = 100; }
            extraDamage = false;
            visionLevel = 0;
            invisible = false;
            effect = "";
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
            string tempEffect = powerupManager.updatePowerups(0, 0, rect.X,rect.Y);
            if (!tempEffect.Equals("")) {
                clearEffect();
            }
            switch (tempEffect) {
                case "white":
                    effect = "white";
                    effectLength = 1200;
                    visionLevel = 1;
                    break;
                case "yellow":
                    effect = "yellow";
                    effectLength = 1500;
                    health+=30;
                    break;
                case "pink":
                    effect = "pink";
                    speedyTasks = 2;
                    effectLength = -5;
                    break;
                case "green":
                    effect = "green";
                    effectLength = 1800;
                    invisible = true;
                    break;
                case "red":
                    effect = "red";
                    effectLength = 1800;
                    extraDamage = true;
                    break;
                case "purple":
                    effect = "purple";
                    effectLength = 1800;
                    health += 30;
                    visionLevel = 2;
                    extraDamage = true;
                    break;
            }
            if (effectLength > 0)
            {
                effectLength--;
                if (effectLength == 0) {
                    clearEffect();
                }
            }
             lights.checkIfLightsOff(spriteBatch, rect.X+31, rect.Y+31,visionLevel);
            if (!effect.Equals("") || speedyTasks > 0) {
                spriteBatch.Begin();
                spriteBatch.Draw(greyTexture, new Rectangle(10, 60, 50, 50), Color.White);
                spriteBatch.Draw(blackTexture, new Rectangle(15, 65, 40, 40), Color.Black);
                if (effect.Equals("pink"))
                {
                    spriteBatch.Draw(greenTexture, new Rectangle(10, 110, speedyTasks * 25, 5), Color.White);
                }
                else {
                    spriteBatch.Draw(greenTexture, new Rectangle(10, 110, effectLength / 36, 5), Color.White);
                }
                spriteBatch.Draw(powerupManager.getPowerUpTexture(effect), new Rectangle(18, 75, 34,20), Color.White);
                spriteBatch.End();
            }
        }
    }
}
