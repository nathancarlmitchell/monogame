using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using monogame.Controls;

namespace monogame.States
{
    public class GameOverState : State
    {
        private List<Button> _components;
        private Menu _menu;

        public GameOverState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        : base(game, graphicsDevice, content)
        {
            _game.IsMouseVisible = true;

            Util.SaveGameData(GameState.Score, GameState.Coins);
            Util.SaveSkinData();

            Background.SetAlpha(0.33f);

            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("HudFont");

            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(CenterWidth, 250),
                Text = "New Game",
            };

            newGameButton.Click += NewGameButton_Click;

            var mainMenuButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(CenterWidth, 250),
                Text = "Main Menu",
            };

            mainMenuButton.Click += MainMenuButton_Click;

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(CenterWidth, 250),
                Text = "Quit",
            };

            quitGameButton.Click += QuitGameButton_Click;

            _components = new List<Button>()
            {
                newGameButton,
                mainMenuButton,
                quitGameButton
            };

            _menu = new Menu(_components);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Background.Draw(gameTime, spriteBatch);

            // Draw HUD.
            var color = Color.Black;
            if (GameState.Score >= GameState.HiScore)
            {
                color = Color.Yellow;
            }
            spriteBatch.DrawString(GameState.hudFont, "Score: " + GameState.Score, new Vector2(32, 64), color, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(GameState.hudFont, "Hi Score: " + GameState.HiScore, new Vector2(32, 92), color, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(GameState.hudFont, " x " + GameState.Coins, new Vector2(GameState.coinHUD.X + 16, GameState.coinHUD.Y - 8),
                Color.Black, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);
            GameState.coinHUD.coinTexture.DrawFrame(spriteBatch, new Vector2(GameState.coinHUD.X, GameState.coinHUD.Y));

            spriteBatch.DrawString(MenuState.titleFont, "Game Over", new Vector2(CenterWidth / 2, 128), Color.AliceBlue, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);

            _menu.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void MainMenuButton_Click(object sender, EventArgs e)
        {
            Game1._gameState = null;
            GameState.Score = 0;
            GameState.Coins = 0;
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Background.Update(gameTime);

            GameState.coinHUD.coinTexture.UpdateFrame(elapsed);

            foreach (var component in _components)
            {
                component.Update(gameTime);
            }

            // Check player input.
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                NewGame();
            }

            TouchCollection touchState = TouchPanel.GetState();
            if (touchState.AnyTouch())
            {
                NewGame();
            }
        }
        public void NewGame()
        {
            Game1._gameState = null;
            GameState.Score = 0;
            GameState.Coins = 0;
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }
    }
}