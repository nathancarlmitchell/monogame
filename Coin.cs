using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace monogame
{
    public class Coin : Object
    {
        protected ContentManager _content;
        private Texture2D coinTexture;
        public Coin(ContentManager content)
        {
            _content = content;

            this.Height = 32;
            this.Width = 32;
            this.X = 0;
            this.Y = 0;

            coinTexture = _content.Load<Texture2D>("coin");
        }

    }
}