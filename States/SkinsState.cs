using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using monogame.Controls;

namespace monogame.States
{
    public class SkinsState : State
    {
        private List<Button> _components;
        private Menu _menu;
        private AnimatedTexture _arrowSprite;
        public static List<Skin> Skins { get; set; }

        public SkinsState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        : base(game, graphicsDevice, content)
        {
            _game.IsMouseVisible = true;

            Background.SetAlpha(0.5f);

            _arrowSprite = new AnimatedTexture(new Vector2(0,0), 0, 1f, 0.5f);
            _arrowSprite.Load(_content, "arrow", 4, 4);

            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("HudFont");

            var backButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(CenterWidth, 250),
                Text = "Back",
            };

            backButton.Click += BackButton_Click;

            _components = new List<Button>()
            {
                backButton
            };

            _menu = new Menu(_components);

            Util.LoadSkinData(content);

            for (int i = 0; i < Skins.Count; i++)
            {
                Skins[i].Click += Skin_Click;

                if (Skins[i].Selected)
                {
                    Skins[i].Activate();
                }           
            }

            Skins = Skins.OrderBy(o=>o.Cost).ToList();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Background.Draw(gameTime, spriteBatch);

            spriteBatch.DrawString(GameState.hudFont, " x " + GameState.Coins, new Vector2(GameState.coinHUD.X + 16, GameState.coinHUD.Y - 8),
                Color.Black, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);
            GameState.coinHUD.coinTexture.DrawFrame(spriteBatch, new Vector2(GameState.coinHUD.X, GameState.coinHUD.Y));

            for (int i = 0; i < Skins.Count; i++)
            {
                int _centerComponent = Skins.Count / 2;
                int _difference = i - _centerComponent;
                int _x = MenuState.CenterWidth + _difference * 100 - (64 / Skins.Count);
                int _y = MenuState.CenterHeight - 128 - 16;

                if (Skins[i].Locked)
                {
                    Skins[i].LoadTexture(_content, "anim_idle_locked");
                    spriteBatch.DrawString(GameState.hudFont, " x " + Skins[i].Cost, new Vector2(_x, _y + 72),
                        Color.Black, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);
                }

                if (Skins[i].Selected)
                {
                    _arrowSprite.DrawFrame(spriteBatch, new Vector2(_x, _y - 16 - 64));
                    Skins[i].Position = new Vector2(_x, _y - 16);
                }
                else
                {
                    Skins[i].Position = new Vector2(_x, _y);
                }
                
                Skins[i].Draw(spriteBatch);
            }

            _menu.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        private void Skin_Click(object sender, EventArgs e)
        {
            Skin skin = (Skin)sender;

            if (!skin.Locked)
            {
                for (int i = 0; i < Skins.Count; i++)
                {
                    Skins[i].Deactivate();
                }

                skin.Activate();
            } 
            else
            {
                if (GameState.Coins >= skin.Cost)
                {
                    for (int i = 0; i < Skins.Count; i++)
                    {
                        Skins[i].Deactivate();
                    }
                    GameState.Coins -= skin.Cost;
                    skin.Locked = false;
                    skin.Activate();
                    Console.WriteLine("Skin unlocked.");
                }
                Console.WriteLine("Skin Locked.");        
            }
            
            Util.SaveGameData(GameState.Score, GameState.Coins);
            Util.SaveSkinData();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
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

            foreach (var skin in Skins)
            {
                skin.Update(gameTime);
            }

            _arrowSprite.UpdateFrame(elapsed);
        }
    }
}