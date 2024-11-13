using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using monogame.States;

namespace monogame
{
    public class Wall : Object
    {
        protected ContentManager _content;

        // Tiling Texture
        private Texture2D wallTexture;
        // How many tiles wide
        private int tileCountWidth = 1;
        // How many tiles high
        public int TileCountHeight { get; set; }
        // Rectangle to draw tiles in
        private Rectangle targetRectangle;
        // Position to draw the tiled Rectangle at

        public Wall(ContentManager content)
        {
            _content = content;

            // Load the texture to tile.
            wallTexture = _content.Load<Texture2D>("wall");

            // Define a drawing rectangle based on the number of tiles wide and high, using the texture dimensions.
            targetRectangle = new Rectangle(0, 0, wallTexture.Width * tileCountWidth, wallTexture.Height * TileCountHeight);
        }

        public void Move()
        {
            this.X -= 2;
        }

        public void Update()
        {
            // Define a drawing rectangle based on the number of tiles wide and high, using the texture dimensions.
            targetRectangle = new Rectangle(0, 0, wallTexture.Width * tileCountWidth, wallTexture.Height * TileCountHeight);
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearWrap,
                DepthStencilState.Default, RasterizerState.CullNone);

            _spriteBatch.Draw(wallTexture, new Vector2(this.X, this.Y), targetRectangle, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            _spriteBatch.End();
        }

    }
}