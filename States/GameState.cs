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
        private Player player;
        private List<Object> enemyArray;
        private WallSpawner spawner;
        public static int Score { get; set; }

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            _game.IsMouseVisible = false;

            hudFont = _content.Load<SpriteFont>("HudFont");
            backgroundTexture = _content.Load<Texture2D>("bg");

            wallTexture = new Texture2D(_graphicsDevice, 1, 1);
            wallTexture.SetData(new[] { Color.White });

            enemyArray = new List<Object>();
            spawner = new WallSpawner();

            player = new Player(_content);
            player.X = ScreenWidth / 2;
            player.Y = ScreenHeight / 2;
            player.Height = 64;
            player.Width = 64;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);
            
            // Replacing the normal SpriteBatch.Draw call to use the version from the "AnimatedTexture" class instead
            player.currentTexture.DrawFrame(spriteBatch, new Vector2(player.X, player.Y));

            spriteBatch.DrawString(hudFont, "" + Score, Vector2.One, Color.Black, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);

            for (int i = 0; i < enemyArray.Count; i++)
            {
                spriteBatch.Draw(wallTexture, new Rectangle(enemyArray[i].X, enemyArray[i].Y, enemyArray[i].Width, enemyArray[i].Height), null, Color.ForestGreen);
            }

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            Score += 1;

            // TODO: Add your update logic here
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            player.currentTexture.UpdateFrame(elapsed);

            for (int i = 0; i < enemyArray.Count; i++)
            {
                enemyArray[i].X -= 2;
                if (player.CheckForCollisions(enemyArray[i]))
                {
                    _game.ChangeState(new GameOverState(_game, _graphicsDevice, _content));
                }
            }

            if ((int)gameTime.TotalGameTime.TotalMilliseconds % (3000) == 0)
            {
                enemyArray.AddRange(spawner.Spawn(ScreenWidth, ScreenHeight));
            }

            MouseState state = Mouse.GetState();
            //player.Y = state.Y;
            //player.X = state.X;

            player.Y -= player.Velocity / 2;
            if ((int)gameTime.TotalGameTime.TotalMilliseconds % (2) == 0)
            {
                player.ChangeVelocity(-1);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                player.Jump();
            }

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

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                _game.ChangeState(new PauseState(_game, _graphicsDevice, _content));
            }
        }
    }
}