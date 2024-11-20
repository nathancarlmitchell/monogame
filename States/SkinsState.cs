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
        private AnimatedTexture arrowSprite;
        public static List<Skin> Skins { get; set; }
        private int _difference;
        private int _totalComponents;
        private int _centerHeight;
        private int _centerWidth;

        public SkinsState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        : base(game, graphicsDevice, content)
        {
            _game.IsMouseVisible = true;

            arrowSprite = new AnimatedTexture(new Vector2(0,0), 0, 1f, 0.5f);
            arrowSprite.Load(_content, "arrow", 4, 4);

            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("HudFont");

            var backButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(CenterWidth, 250),
                Text = "Back",
            };

            backButton.Click += backButton_Click;

            _components = new List<Button>()
            {
                backButton
            };

            _menu = new Menu(_components);

            Util.LoadSkinData(content);

            _centerHeight = MenuState.CenterHeight;
            _centerWidth = MenuState.ControlWidthCenter;

            for (int i = 0; i < Skins.Count; i++)
            {
                Skins[i].Click += Skin_Click;
                _totalComponents = Skins.Count;
                int centerComponent = _totalComponents / 2;
                _difference = i - centerComponent;                
            }

            Skins = Skins.OrderBy(o=>o.Cost).ToList();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.DrawString(GameState.hudFont, " x " + GameState.Coins, new Vector2(GameState.coinHUD.X + 16, GameState.coinHUD.Y - 8),
                Color.Black, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);
            GameState.coinHUD.coinTexture.DrawFrame(spriteBatch, new Vector2(GameState.coinHUD.X, GameState.coinHUD.Y));

            for (int i = 0; i < Skins.Count; i++)
            {
                int centerComponent = _totalComponents / 2;
                _difference = i - centerComponent;
                int _x = MenuState.CenterWidth + _difference * 100 - (64 / _totalComponents);
                int _y = _centerHeight - 128 - 16;

                if (Skins[i].Locked)
                {
                    //Console.WriteLine("Locked.");
                    Skins[i].LoadTexture(_content, "anim_idle_locked");
                    spriteBatch.DrawString(GameState.hudFont, " x " + Skins[i].Cost, new Vector2(_x, _y + 72),
                        Color.Black, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);
                }

                if (Skins[i].Selected)
                {
                    arrowSprite.DrawFrame(spriteBatch, new Vector2(_x, _y - 16 - 64));
                    Skins[i].Position = new Vector2(_x, _y - 16);
                    //Skins[i].Activate();
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

        private void backButton_Click(object sender, EventArgs e)
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

            foreach (var component in _components)
            {
                component.Update(gameTime);
            }

            foreach (var skin in Skins)
            {
                skin.Update(gameTime);
            }

            arrowSprite.UpdateFrame(elapsed);
        }
    }
}