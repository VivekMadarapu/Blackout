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
        public Texture2D tex;
        public Vector2 loc;
        public Color color;
        public Rectangle rect;
        public Rectangle sourceRect;

        public Mortimer(Vector2 loc) : base(50, 50, 20)
        {

            this.loc = loc;
            color = Color.White;
            rect = new Rectangle((int)loc.X, (int)loc.Y, 50, 50);
            sourceRect = new Rectangle(0, 0, 31, 31);
        }
        //public void Update(Level level)
        //{
        //    GamePadState newPad = GamePad.GetState(PlayerIndex.One);
        //    level.Update(newPad);
        //}

        public void loadContent(Game game)
        {
            tex = game.Content.Load<Texture2D>("Mortimer");

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, rect, sourceRect, color);

        }
    }
}