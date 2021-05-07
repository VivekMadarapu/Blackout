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
    public class MousePointer
    {
        public static Texture2D circleImage;
        public static int POINTER_SIZE = 30;
        public static double MOUSE_SENSITIVITY = 4.5;
        public Vector2 screenSize;

        public double x, y;
        public Rectangle container;

        public MousePointer(double x, double y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.container = new Rectangle((int)x, (int)y, POINTER_SIZE, POINTER_SIZE);
            this.screenSize = new Vector2(width, height);
        }

        public static void loadPointerImage(Microsoft.Xna.Framework.Game game)
        {
            circleImage = game.Content.Load<Texture2D>("CirclePointer");
        }

        public void Update(GamePadState gamePad)
        {
            double xChange = gamePad.ThumbSticks.Left.X * MOUSE_SENSITIVITY;
            double yChange = gamePad.ThumbSticks.Left.Y * -MOUSE_SENSITIVITY;
            if (x + xChange < screenSize.X && x + xChange > 0) x += xChange;
            if (y + yChange < screenSize.Y && y + yChange > 0) y += yChange;
            this.container.X = (int)x;
            this.container.Y = (int)y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(circleImage, container, null, Color.White, 0, new Vector2(circleImage.Width / 2, circleImage.Height / 2), SpriteEffects.None, 0);
        }
    }
}