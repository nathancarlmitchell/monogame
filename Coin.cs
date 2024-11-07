using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using monogame.States;

namespace monogame
{
    public class Coin : Object
    {
        protected ContentManager _content;
        public AnimatedTexture coinTexture;

        private const float rotation = 0;
        private const float scale = 1f;
        private const float depth = 0.5f;
        private const int frames = 6;
        private const int framesPerSec = 12;

        private int hoverRange = 64;
        private int hover = 0;
        private bool direction = true;

        public Coin(ContentManager content)
        {
            _content = content;

            this.Height = 32;
            this.Width = 32;
            this.X = GameState.ScreenWidth;
            this.Y = GameState.CenterHeight;

            coinTexture = new AnimatedTexture(new Vector2(this.Height / 2, this.Width / 2), rotation, scale, depth);
            coinTexture.Load(_content, "coin", frames, framesPerSec);
        }

        public void Hover()
        {
            if (direction)
            {
                this.hover += 1;
                this.Y += 1;
            }
            else
            {
                this.hover -= 1;
                this.Y -= 1;
            }

            if (Math.Abs(this.hover) > hoverRange)
            {
                this.direction = !this.direction;
            }
        }

    }
}