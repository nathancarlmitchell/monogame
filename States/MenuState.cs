using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Java.Lang;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using monogame.Controls;

namespace monogame.States
{
    public class MenuState : State
    {
        private List<Button> _components;
        public static SpriteFont titleFont;
        private Menu _mainMenu;

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            new GameState(_game, _graphicsDevice, _content);

            _game.IsMouseVisible = true;

            titleFont = _content.Load<SpriteFont>("TitleFont");

            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("HudFont");

            MenuState.ControlWidthCenter = (_graphicsDevice.Viewport.Width / 2) - (buttonTexture.Width / 2);

            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(CenterWidth, 200),
                Text = "New Game",
            };

            newGameButton.Click += NewGameButton_Click;

            var loadSkinsButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(CenterWidth, 250),
                Text = "Skins",
            };

            loadSkinsButton.Click += loadSkinsButton_Click;

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(CenterWidth, 300),
                Text = "Quit",
            };

            quitGameButton.Click += QuitGameButton_Click;

            _components = new List<Button>()
            {
                newGameButton,
                loadSkinsButton,
                quitGameButton,
            };

            _mainMenu = new Menu(_components);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {          
            spriteBatch.Begin();

            // Draw background.
            Background.Draw(gameTime, spriteBatch);

            // Draw menu.
            _mainMenu.Draw(gameTime, spriteBatch);

            // Draw title.
            spriteBatch.DrawString(titleFont, "Flappy Box", new Vector2(CenterWidth / 2, 128), Color.AliceBlue, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);

            spriteBatch.End();
        }

        public void onClick()
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }

        private void loadSkinsButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new SkinsState(_game, _graphicsDevice, _content));
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // remove sprites if they're not needed
        }

        public override void Update(GameTime gameTime)
        {
            Background.Update(gameTime);

            foreach (var component in _components)
            {
                component.Update(gameTime);
            }

            // Check player input.
            if (Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
            }

            TouchCollection touchState = TouchPanel.GetState();
            if (touchState.AnyTouch())
            {
                Console.WriteLine("pressed");
                _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
            }
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }
    }
}