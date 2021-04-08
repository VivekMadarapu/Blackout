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
    class AnimatedSprite
    {
        class AnimatedSprite
        {
            public List<Texture2D> SpriteTextures { get; set; }
            public int FrameWidth;
            public int FrameHeight;
            private int framesPerRow;
            public Dictionary<string, Animation> SpriteAnimations;
            public AnimatedSprite(int _frameWidth, int _frameHeight, int _framesPerRow)
            {
                FrameWidth = _frameWidth;
                FrameHeight = _frameHeight;
                framesPerRow = _framesPerRow;
                SpriteTextures = new List<Texture2D>();
                SpriteAnimations = new Dictionary<string, Animation>();
            }
            public Vector2 Origin
            {
                get { return new Vector2(FrameWidth / 2.0f, FrameHeight / 2.0f); }

            }
            public Rectangle GetFrameRectangle(int _frameNumber)
            {
                return new Rectangle(_frameNumber % framesPerRow * FrameWidth,
                    _frameNumber / framesPerRow * FrameHeight,
                    FrameWidth,
                    FrameHeight);
            }

        }
    }
