using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace monogame.States
{
    public class GameOverState : State
    {     
        public GameOverState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) 
        : base(game, graphicsDevice, content)
        {
            _game.IsMouseVisible = true;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.DrawString(GameState.hudFont, "Game Over: " + GameState.score, Vector2.One, Color.Black, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }
    }
}