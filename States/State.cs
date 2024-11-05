using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace monogame.States
{
    public abstract class State
    {
        #region Fields
        protected ContentManager _content;
        protected GraphicsDevice _graphicsDevice;
        protected Game1 _game;
        public static int controlCenterHeight;
        public static int controlCenterWidth;

        #endregion

        #region Methods
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public abstract void PostUpdate(GameTime gameTime);

        public State(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        {
            _game = game;
            _graphicsDevice = graphicsDevice;
            _content = content;
            
            controlCenterWidth = (_graphicsDevice.Viewport.Width / 2) - 80;
            controlCenterHeight = (_graphicsDevice.Viewport.Height / 2);
        }

        public abstract void Update(GameTime gameTime);

        #endregion
    }
}