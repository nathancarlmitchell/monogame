using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace monogame.States
{
    public class GameState : State
    {
        public static SpriteFont hudFont, debugFont;
        private Texture2D wallTexture, boostTexture;
        public static Player player;
        public static List<Skin> Skins { get; set; }
        public static Coin coinHUD;
        private List<Wall> wallArray;
        private List<Coin> coinArray;
        private WallSpawner wallSpawner;
        private CoinSpawner coinSpawner;
        private bool _debug = true;

        public static int Score { get; set; }
        public static int HiScore { get; set; }
        public static int Coins { get; set; }

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            _game.IsMouseVisible = false;

            Util.LoadGameData();

            Background.SetAlpha(1.0);          

            hudFont = _content.Load<SpriteFont>("HudFont");
            debugFont = _content.Load<SpriteFont>("DebugFont");

            wallTexture = new Texture2D(_graphicsDevice, 1, 1);
            wallTexture.SetData(new[] { Color.White });

            boostTexture = _content.Load<Texture2D>("boost");

            wallArray = new List<Wall>();
            coinArray = new List<Coin>();

            wallSpawner = new WallSpawner(_content);
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
                for (int i = 0; i < GameState.Skins.Count; i++)
                {
                    if (GameState.Skins[i].Selected)
                    {
                        player.ChangeSkin(GameState.Skins[i].Name, GameState.Skins[i].Frames, GameState.Skins[i].FPS);
                        player.SkinName = GameState.Skins[i].Name;
                    }
                }
            }
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            // Draw background.
            Background.Draw(gameTime, spriteBatch);

            // Draw boost button on mobile.
            if (OperatingSystem.IsAndroid())
            {
                spriteBatch.Draw(boostTexture, new Vector2(0, ScreenHeight - 128), Color.White);
            }

            // Draw player.
            player.Draw(spriteBatch);

            spriteBatch.End();


            // Draw walls.
            for (int i = 0; i < wallArray.Count; i++)
            {
                wallArray[i].Draw(spriteBatch);
            }


            spriteBatch.Begin();

            // Draw coins.
            for (int i = 0; i < coinArray.Count; i++)
            {
                coinArray[i].coinTexture.DrawFrame(spriteBatch, new Vector2(coinArray[i].X, coinArray[i].Y));
            }

            // Draw HUD.
            var color = Color.Black;
            if (Score >= HiScore)
            {
                color = Color.Yellow;
            }
            spriteBatch.DrawString(hudFont, "Score: " + Score, new Vector2(32, 64), color, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(hudFont, "Hi Score: " + HiScore, new Vector2(32, 92), color, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(hudFont, " x " + Coins, new Vector2(coinHUD.X + 16, coinHUD.Y - 8), Color.Black, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);
            coinHUD.coinTexture.DrawFrame(spriteBatch, new Vector2(coinHUD.X, coinHUD.Y));

            // Draw debug.
            if (_debug)
            {
                spriteBatch.DrawString(debugFont, "Coin Length: " + coinArray.Count, new Vector2(64, ScreenHeight - 96), Color.Black, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);
                spriteBatch.DrawString(debugFont, "Array Length: " + wallArray.Count, new Vector2(64, ScreenHeight - 80), Color.Black, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);

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

        private int wallCooldown = 0;
        private int coinCooldown = 0;
        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //float currentTime = (float)gameTime.TotalGameTime.TotalSeconds;

            Score += 1;
            if (Score > HiScore)
            {
                HiScore = Score;
            }

            Background.Update(gameTime);

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

                if (coinArray[i].X + coinArray[i].Width < 0)
                {
                    coinArray.Remove(coinArray[i]);
                }
            }

            // Update wall positions and check collisions.
            for (int i = 0; i < wallArray.Count; i++)
            {
                // Check collisions.
                if (player.CheckForCollisions(wallArray[i]))
                {
                    _game.ChangeState(new GameOverState(_game, _graphicsDevice, _content));
                }

                wallArray[i].Move();

                // Remove walls off screen.
                if (wallArray[i].X + wallArray[i].Width < 0)
                {
                    wallArray.Remove(wallArray[i]);
                }
            }

            // Spawn a wall.
            wallCooldown++;
            if (wallCooldown >= 180)
            {
                wallCooldown = 0;
                wallArray.AddRange(wallSpawner.Spawn(_content));
            }

            // Spawn a coin.
            coinCooldown++;
            if (coinCooldown >= 360)
            {
                coinCooldown = 0;
                coinArray.Add(coinSpawner.Spawn(_content));
            }

            // Update player.
            player.Update(elapsed, gameTime);

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

            // Check touch input.
            TouchCollection touchState = TouchPanel.GetState();
            if (touchState.AnyTouch())
            {
                int x = (int)touchState.GetPosition().X;
                int y = (int)touchState.GetPosition().Y;

                if (x < 128 && y > GameState.ScreenHeight - 128)
                {
                    player.Jump(player.JumpVelocity * 2);
                    return;
                }
                player.Jump(player.JumpVelocity);
            }

            // Check player bounds
            // if (player.X > ScreenWidth - player.Width / 2)
            // {
            //     player.X = ScreenWidth - player.Width / 2;
            // }
            // else if (player.X < player.Width / 2)
            // {
            //     player.X = player.Width / 2;
            // }
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