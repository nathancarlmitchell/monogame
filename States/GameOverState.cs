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
using monogame.Controls;

namespace monogame.States
{
    public class GameOverState : State
    {
        private List<Button> _components;
        private Menu _menu;
        //private Texture2D borderTexture;
        public GameOverState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        : base(game, graphicsDevice, content)
        {
            _game.IsMouseVisible = true;

            Util.SaveGameData(GameState.Score, GameState.Coins);
            Util.SaveSkinData();

            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            //borderTexture = _content.Load<Texture2D>("border");
            var buttonFont = _content.Load<SpriteFont>("HudFont");

            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(CenterWidth, 250),
                Text = "New Game",
            };

            newGameButton.Click += NewGameButton_Click;

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(CenterWidth, 250),
                Text = "Quit",
            };

            quitGameButton.Click += QuitGameButton_Click;

            _components = new List<Button>()
            {
                newGameButton,
                quitGameButton
            };

            _menu = new Menu(_components);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            //spriteBatch.Draw(borderTexture, new Rectangle(0, 0, borderTexture.Height, borderTexture.Width), null, Color.White);
                        // Draw HUD.
            spriteBatch.DrawString(GameState.hudFont, "Score: " + GameState.Score, new Vector2(32, 64), Color.Black, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(GameState.hudFont, "Hi Score: " + GameState.HiScore, new Vector2(32, 92), Color.Black, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(GameState.hudFont, " x " + GameState.Coins, new Vector2(GameState.coinHUD.X + 16, GameState.coinHUD.Y - 8),
                Color.Black, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);
            GameState.coinHUD.coinTexture.DrawFrame(spriteBatch, new Vector2(GameState.coinHUD.X, GameState.coinHUD.Y));

            _menu.Draw(gameTime, spriteBatch);

            spriteBatch.DrawString(MenuState.titleFont, "Game Over", new Vector2(CenterWidth / 2, 128), Color.AliceBlue, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);

            // spriteBatch.DrawString(GameState.hudFont, "Coins: " + GameState.Coins, new Vector2(ControlWidthCenter, CenterHeight/4),
            //     Color.Black, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);

            spriteBatch.End();
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            Game1._gameState = null;
            GameState.Score = 0;
            GameState.Coins = 0;

            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
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

            GameState.coinHUD.coinTexture.UpdateFrame(elapsed);

            foreach (var component in _components)
            {
                component.Update(gameTime);
            }

            // Check player input.
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
            }
        }
    }
}