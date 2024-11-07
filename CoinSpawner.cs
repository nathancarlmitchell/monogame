using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using monogame.States;

namespace monogame
{
    public class CoinSpawner
    {
        private static Random rand = new Random();
        private Coin coin;

        public Coin Spawn(ContentManager content)
        {
            coin = new Coin(content);
            
            int heightBuffer = 64;
            int minHeight = heightBuffer + coin.Height;
            int maxHeight = GameState.ScreenHeight - (heightBuffer + coin.Height);

            int height = (int)Math.Floor(rand.NextDouble() * (maxHeight - minHeight + 1) + minHeight);

            coin.X = GameState.ScreenWidth;
            coin.Y = height;

            return coin;
        }
    }
}