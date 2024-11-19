using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using monogame.Controls;

namespace monogame.States
{
    public static class Background
    {
        private static Texture2D backgroundTexture, cloudTexture;
        private static Bird bird;
        private static Object cloud, bg;
        private static double alpha, targetAlpha;

        static Background()
        {
            alpha = 0.0;
        }

        public static void SetAlpha(double _targetAlpha)
        {
            targetAlpha = _targetAlpha;
        }

        public static void UpdateAlpha()
        {
            if (alpha < targetAlpha)
            {
                alpha += 0.005;
                if (alpha > 1.0)
                {
                    alpha = 1.0;
                }
            }
            else
            {
                alpha -= 0.005;
                if (alpha < 0.0)
                {
                    alpha = 0;
                }
            }
        }

        public static void LoadContent(ContentManager content)
        {
            backgroundTexture = content.Load<Texture2D>("bg");
            cloudTexture = content.Load<Texture2D>("cloud");

            cloud = new Object();
            cloud.X = 200;
            cloud.Y = 200;

            // Load background objects
            bg = new Object();
            bg.X = 0;
            bg.Y = 0;

            bird = new Bird(content);
            bird.Reset();
        }

        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {          
            // Draw background.
            spriteBatch.Draw(backgroundTexture, new Vector2(bg.X, bg.Y), Color.White * (float)alpha);

            // Draw cloud.
            spriteBatch.Draw(cloudTexture, new Vector2(cloud.X, cloud.Y), Color.White * ((float)alpha - 0.25f));

            // Draw bird.
            bird.currentTexture.DrawFrame(spriteBatch, new Vector2(bird.X, bird.Y), (float)0.75);
        }

        public static void PostUpdate(GameTime gameTime)
        {
            // remove sprites if they're not needed
        }

        public static void Update(GameTime gameTime)
        {
            UpdateAlpha();

            if ((int)gameTime.TotalGameTime.TotalMilliseconds % (1000) == 0)
            {
                // Update cloud posistion.
                cloud.X -= 1;
            }

            if ((int)gameTime.TotalGameTime.TotalMilliseconds % (3000) == 0)
            {
                // Update background position.
                bg.X -= 2;
            }

            // Update bird animation.
            if (bird.X < 0)
            {
                bird.Reset();
            }
            bird.currentTexture.UpdateFrame((float)gameTime.ElapsedGameTime.TotalSeconds);
            bird.X -= 1;
        }
    }
}