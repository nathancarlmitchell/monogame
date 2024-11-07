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
        private static int _centerHeight;
        private static int _centerWidth;
        private static int _screenHeight;
        private static int _screenWidth;
        private static int _controlWidthCenter;

        #endregion

        #region Methods
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public abstract void PostUpdate(GameTime gameTime);

        public State(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        {
            _game = game;
            _graphicsDevice = graphicsDevice;
            _content = content;
            
            _centerWidth = (_graphicsDevice.Viewport.Width / 2);
            _centerHeight = (_graphicsDevice.Viewport.Height / 2);

            _screenHeight = _graphicsDevice.Viewport.Height;
            _screenWidth = _graphicsDevice.Viewport.Width;

            _controlWidthCenter = (_graphicsDevice.Viewport.Width / 2) - 80;
        }

        public static int ScreenHeight
        {
            get { return _screenHeight; }
        }

        public static int ScreenWidth
        {
            get { return _screenWidth; }
        }

        public static int CenterHeight
        {
            get { return _centerHeight; }
        }

        public static int CenterWidth
        {
            get { return _centerWidth; }
        }

        public static int ControlWidthCenter
        {
            get { return _controlWidthCenter; }
            set { _controlWidthCenter = value; }
        }

        public abstract void Update(GameTime gameTime);

        #endregion
    }
}