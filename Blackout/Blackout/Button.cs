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
    public class Button
    {
        public static int PIXELS_PER_CHARACTER = 10;
        public static SpriteFont font;
        public static Texture2D buttonImage;

        public static double paddingConstant = 2;

        public Rectangle location;
        public String text;
        public Vector2 textLocation;

        public bool overLapping;

        public Button(Rectangle location, String text)
        {
            this.location = location;
            this.text = text;
            double y = location.Y + (location.Height - PIXELS_PER_CHARACTER * paddingConstant) / 2;
            double x = location.X + (location.Width - PIXELS_PER_CHARACTER * text.Length) / 2;
            this.textLocation = new Vector2((float)x, (float)y);
            overLapping = true;
        }

        public static void loadContent(Microsoft.Xna.Framework.Game game)
        {
            font = game.Content.Load<SpriteFont>("LabelPromptFont");
            Texture2D texture = new Texture2D(game.GraphicsDevice, 1, 1);
            texture.SetData(new Color[]{Color.White});
            buttonImage = texture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(buttonImage, location, (overLapping) ? Color.White : Color.Black);
            spriteBatch.DrawString(font, text, textLocation, (overLapping) ? Color.Black : Color.White);
        }

        public void Update(float x, float y)
        {
            overLapping = x > location.Left && x < location.Right && y > location.Top && y < location.Bottom;
        }

    }


    public class LabelPrompt
    {
        public static SpriteFont font;

        public Vector2 location;
        public String text;

        public LabelPrompt(Vector2 location, String text)
        {
            this.location = location;
            this.text = text;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, text, location, Color.Black);

        }

        public static void loadSpriteFont(Microsoft.Xna.Framework.Game game)
        {
            font = game.Content.Load<SpriteFont>("LabelPromptFont");
        }


    }
}
