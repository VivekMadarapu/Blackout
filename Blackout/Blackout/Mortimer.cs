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
    class Mortimer: AnimatedSprite
    {
        public Texture2D tex;
        public Vector2 loc;
        public Color color;
        
        public Mortimer(Texture2D tex, Vector2 loc)
        {
            this.tex = tex;
            this.loc = loc;
            color = Color.White;
        }
        //public void loadContent(Microsoft.Xna.Framework.Game game)
        //{
        //    game.Content.Load<Texture2D>("Mortimer");
        //}
        public void Draw(SpriteBatch spriteBatch)
        {
          //  spriteBatch.Draw()
        }
    }
}
