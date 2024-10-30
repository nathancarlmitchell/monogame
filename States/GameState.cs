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
  public class GameState : State
  {

    public static SpriteFont hudFont;
    private Texture2D playerTexture;
    public static int score = 0;
    private Object player;
    private List<Object> enemyArray;
    private WallSpawner spawner;

    private int ScreenHeight;
    private int ScreenWidth;

    public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) 
      : base(game, graphicsDevice, content)
    {
        ScreenHeight = _graphicsDevice.Viewport.Height;
        ScreenWidth = _graphicsDevice.Viewport.Width;

        game.IsMouseVisible = false;

        hudFont = _content.Load<SpriteFont>("HudFont");
        playerTexture = _content.Load<Texture2D>("ball");

        enemyArray = new List<Object>();

        spawner = new WallSpawner();

        player = new Object();
        player.X = ScreenWidth / 2;
        player.Y = ScreenHeight / 2;
        player.Height = 64;
        player.Width = 64;

        //score = 0;
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();

        spriteBatch.DrawString(hudFont, ""+score, Vector2.One, Color.Black, 0, Vector2.One, 1.0f, SpriteEffects.None, 0.5f);

        spriteBatch.Draw(
                playerTexture,
                new Vector2(player.X, player.Y),
                null, // An optional region on the texture which will be rendered. If null - draws full texture.
                Color.White, // color mask
                0f, // rotation
                new Vector2(player.Width / 2, player.Width / 2), // origin
                Vector2.One, // scale
                SpriteEffects.None,
                0f // rotation
            );

        for (int i = 0; i < enemyArray.Count; i++){
            spriteBatch.Draw(playerTexture, new Rectangle(enemyArray[i].X, enemyArray[i].Y, enemyArray[i].Width, enemyArray[i].Height), null, Color.Red);
        }

        spriteBatch.End();
    }

    public override void PostUpdate(GameTime gameTime)
    {

    }

    public override void Update(GameTime gameTime)
    {
        score += 1;

        for (int i = 0; i < enemyArray.Count; i++){
            enemyArray[i].X -= 1;
            if (player.CheckForCollisions(enemyArray[i]))
                _game.ChangeState(new GameOverState(_game, _graphicsDevice, _content));
        }

        if ((int)gameTime.TotalGameTime.TotalMilliseconds % (3000) == 0) {
            enemyArray.AddRange(spawner.Spawn(ScreenWidth, ScreenHeight));
        }

        MouseState state = Mouse.GetState();
        player.Y = state.Y;
        player.X = state.X;

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

        if (Keyboard.GetState().IsKeyDown(Keys.Escape)){
            _game.ChangeState(new PauseState(_game, _graphicsDevice, _content));
        }
    }
  }
}