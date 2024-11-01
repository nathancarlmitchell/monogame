using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using monogame.Controls;

namespace monogame.States
{
    public class GameOverState : State
    {
        private List<Component> _components;
        public GameOverState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        : base(game, graphicsDevice, content)
        {
            _game.IsMouseVisible = true;

            Game1._gameState = null;
            GameState.score = 0;

            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("HudFont");

            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(controlCenterWidth, 250),
                Text = "New Game",
            };

            newGameButton.Click += NewGameButton_Click;

            _components = new List<Component>()
            {
                newGameButton
            };
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.DrawString(GameState.hudFont, "Game Over: " + GameState.score, Vector2.One, Color.Black, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);

            foreach (var component in _components)
            {
                component.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }


        public override void PostUpdate(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
            {
                component.Update(gameTime);
            }
        }
    }
}