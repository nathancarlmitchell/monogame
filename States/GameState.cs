using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace monogame.States
{
    public class GameState : State
    {
        public static SpriteFont hudFont, debugFont;
        private Texture2D wallTexture, backgroundTexture;
        private Object bgObject;
        private double alpha = 0.0;
        public static Player player;
        public static List<Skin> Skins { get; set; }
        public static Coin coinHUD;
        private Bird bird;
        private List<Wall> wallArray;
        private List<Coin> coinArray;
        private WallSpawner wallSpawner;
        private CoinSpawner coinSpawner;
        private bool _debug = true;

        public static int Score { get; set; }
        public static int HiScore { get; set; }
        public static int Coins { get; set; }

        //private Load data;

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            _game.IsMouseVisible = false;

            Util.LoadGameData();            

            hudFont = _content.Load<SpriteFont>("HudFont");
            debugFont = _content.Load<SpriteFont>("DebugFont");

            // Load background objects
            backgroundTexture = _content.Load<Texture2D>("bg");
            bgObject = new Object();
            bgObject.X = 0;
            bgObject.Y = 0;
            bird = new Bird(_content);
            bird.Reset();

            wallTexture = new Texture2D(_graphicsDevice, 1, 1);
            wallTexture.SetData(new[] { Color.White });

            wallArray = new List<Wall>();
            coinArray = new List<Coin>();

            wallSpawner = new WallSpawner();
            coinSpawner = new CoinSpawner();

            coinHUD = new Coin(_content);
            coinHUD.X = 32;
            coinHUD.Y = 32;

            player = new Player(_content);
            player.X = ScreenWidth / 2;
            player.Y = ScreenHeight / 2;
            player.Height = 64;
            player.Width = 64;

            // Set the selected player skin
            Util.LoadSkinData(_content);
            GameState.Skins = SkinsState.Skins;
            if (GameState.Skins is not null)
            {
                Console.WriteLine("Skins in not null");
                for (int i = 0; i < GameState.Skins.Count; i++)
                {
                    if (GameState.Skins[i].Selected)
                    {
                        Console.WriteLine("skins[i].Name: " + GameState.Skins[i].Name);
                        player.ChangeSkin(GameState.Skins[i].Name);
                        player.CurrentSkin = GameState.Skins[i].Name;
                    }
                }
            }
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            // Draw background.
            spriteBatch.Draw(backgroundTexture, new Vector2(bgObject.X, bgObject.Y), Color.White * (float)alpha);

            // Draw bird.
            bird.currentTexture.DrawFrame(spriteBatch, new Vector2(bird.X, bird.Y), (float)0.75);

            // Draw player.
            player.currentTexture.DrawFrame(spriteBatch, new Vector2(player.X, player.Y));

            // Draw coins.
            for (int i = 0; i < coinArray.Count; i++)
            {
                coinArray[i].coinTexture.DrawFrame(spriteBatch, new Vector2(coinArray[i].X, coinArray[i].Y));
            }

            // Draw walls.
            //for (int i = 0; i < wallArray.Count; i++)
            //{
            //spriteBatch.Draw(wallTexture, new Rectangle(wallArray[i].X, wallArray[i].Y, wallArray[i].Width, wallArray[i].Height), null, Color.ForestGreen);
            //}

            spriteBatch.End();

            // Draw walls.
            for (int i = 0; i < wallArray.Count; i++)
            {
                wallArray[i].Draw(spriteBatch);
            }


            spriteBatch.Begin();

            // Draw HUD.
            spriteBatch.DrawString(hudFont, "Score: " + Score, new Vector2(32, 64), Color.Black, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(hudFont, "Hi Score: " + HiScore, new Vector2(32, 92), Color.Black, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(hudFont, " x " + Coins, new Vector2(coinHUD.X + 16, coinHUD.Y - 8), Color.Black, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);
            coinHUD.coinTexture.DrawFrame(spriteBatch, new Vector2(coinHUD.X, coinHUD.Y));

            if (_debug)
            {
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                spriteBatch.DrawString(debugFont, "elapsed: " + elapsed, new Vector2(64, ScreenHeight - 64), Color.Black, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);

                float currentTime = (float)gameTime.TotalGameTime.TotalSeconds;
                spriteBatch.DrawString(debugFont, "currentTime: " + currentTime, new Vector2(64, ScreenHeight - 48), Color.Black, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);
            }

            spriteBatch.End();

        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float currentTime = (float)gameTime.TotalGameTime.TotalSeconds;

            Score += 1;
            if (Score > HiScore)
            {
                HiScore = Score;
            }

            if (alpha < 1.0)
            {
                alpha += 0.005;
                if (alpha > 1.0)
                {
                    alpha = 1.0;
                }
            }

            // Rotate player.
            //player.currentTexture.Rotation = (float)(alpha * Math.PI * 2) * 4;

            // Update coin positions and check collisions.
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

            // Update wall positions and check collisions.
            for (int i = 0; i < wallArray.Count; i++)
            {
                wallArray[i].Move();
                if (player.CheckForCollisions(wallArray[i]))
                {
                    _game.ChangeState(new GameOverState(_game, _graphicsDevice, _content));
                }
            }

            // Update player animation.
            player.currentTexture.UpdateFrame(elapsed);

            // Update bird animation.
            if (bird.X < 0)
            {
                bird.Reset();
            }
            bird.currentTexture.UpdateFrame(elapsed);
            bird.X -= 1;

            // Spawn a wall.
            if ((int)gameTime.TotalGameTime.TotalMilliseconds % (3000) == 0)
            {
                wallArray.AddRange(wallSpawner.Spawn(_content));
                bgObject.X -= 1;
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
                player.Jump(player.JumpVelocity);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                player.Jump(player.JumpVelocity * 2);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                _game.ChangeState(new PauseState(_game, _graphicsDevice, _content));
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                _game.ChangeState(new GameOverState(_game, _graphicsDevice, _content));
            }

            

            // if (Keyboard.GetState().IsKeyDown(Keys.L))
            // {
            //     player.ChangeSkin();
            // }

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