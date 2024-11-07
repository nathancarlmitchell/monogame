using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace monogame.States
{
    public class GameState : State
    {
        public static SpriteFont hudFont;
        private Texture2D wallTexture, backgroundTexture;
        private float alpha = 0.0f;
        private Player player;
        private Coin coinHUD;
        private List<Object> wallArray;
        private List<Coin> coinArray;
        private WallSpawner spawner;
        private CoinSpawner coinSpawner;
        public static int Score { get; set; }
        public static int Coins { get; set; }

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            _game.IsMouseVisible = false;

            hudFont = _content.Load<SpriteFont>("HudFont");
            backgroundTexture = _content.Load<Texture2D>("bg");

            wallTexture = new Texture2D(_graphicsDevice, 1, 1);
            wallTexture.SetData(new[] { Color.White });

            wallArray = new List<Object>();
            coinArray = new List<Coin>();

            spawner = new WallSpawner();
            coinSpawner = new CoinSpawner();

            coinHUD = new Coin(_content);
            coinHUD.X = 32;
            coinHUD.Y = 32;

            player = new Player(_content);
            player.X = ScreenWidth / 2;
            player.Y = ScreenHeight / 2;
            player.Height = 64;
            player.Width = 64;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            // Draw background.
            spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White * alpha);

            // Draw player.
            player.currentTexture.DrawFrame(spriteBatch, new Vector2(player.X, player.Y));

            // Draw coins.
            for (int i = 0; i < coinArray.Count; i++)
            {
                coinArray[i].coinTexture.DrawFrame(spriteBatch, new Vector2(coinArray[i].X, coinArray[i].Y));
            }

            // Draw walls.
            for (int i = 0; i < wallArray.Count; i++)
            {
                spriteBatch.Draw(wallTexture, new Rectangle(wallArray[i].X, wallArray[i].Y, wallArray[i].Width, wallArray[i].Height), null, Color.ForestGreen);
            }

            // Draw HUD.
            //spriteBatch.DrawString(hudFont, "" + Score, Vector2.One, Color.Black, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(hudFont, " x " + Coins, new Vector2(coinHUD.X + 16, coinHUD.Y - 8), Color.Black, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);
            coinHUD.coinTexture.DrawFrame(spriteBatch, new Vector2(coinHUD.X, coinHUD.Y));

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            Score += 1;

            if (alpha < 1.0f)
            {
                alpha += 0.005f;
            }

            // Update player animation.
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            player.currentTexture.UpdateFrame(elapsed);

            // Update coin positions.
            coinHUD.coinTexture.UpdateFrame(elapsed);
            for (int i = 0; i < coinArray.Count; i++)
            {
                coinArray[i].coinTexture.UpdateFrame(elapsed);
                coinArray[i].X -= 1;
                coinArray[i].Hover();
                // Get coin.
                if (player.CheckForCollisions(coinArray[i]))
                {
                    coinArray.RemoveAt(i);
                    Coins += 1;
                }
            }

            // Update wall positions.
            for (int i = 0; i < wallArray.Count; i++)
            {
                wallArray[i].X -= 2;
                if (player.CheckForCollisions(wallArray[i]))
                {
                    _game.ChangeState(new GameOverState(_game, _graphicsDevice, _content));
                }
            }

            // Spawn an enemy.
            if ((int)gameTime.TotalGameTime.TotalMilliseconds % (3000) == 0)
            {
                wallArray.AddRange(spawner.Spawn(ScreenWidth, ScreenHeight));
            }

            // Spawn a coin.
            if ((int)gameTime.TotalGameTime.TotalMilliseconds % (6000) == 0)
            {
                coinArray.Add(coinSpawner.Spawn(_content));
            }

            // Update player velocity.
            player.Y -= player.Velocity / 2;
            if ((int)gameTime.TotalGameTime.TotalMilliseconds % (2) == 0)
            {
                player.ChangeVelocity(-1);
            }

            // Check player input.
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                player.Jump();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                _game.ChangeState(new PauseState(_game, _graphicsDevice, _content));
            }

            // Check player bounds
            if (player.X > ScreenWidth - player.Width / 2)
            {
                player.X = ScreenWidth - player.Width / 2;
            }
            else if (player.X < player.Width / 2)
            {
                player.X = player.Width / 2;
            }

            if (player.Y > ScreenHeight - player.Height / 2)
            {
                player.Y = ScreenHeight - player.Height / 2;
            }
            else if (player.Y < player.Height / 2)
            {
                player.Y = player.Height / 2;
            }
        }
    }
}