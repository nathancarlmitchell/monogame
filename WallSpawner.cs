using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using monogame.States;

namespace monogame
{
    public class WallSpawner
    {
        protected ContentManager _content;
        private Random rand = new Random();
        private int _screenHeight = GameState.ScreenHeight;
        private int _screenWidth = GameState.ScreenWidth;
        private int minHeight = 20;
        private int width = 64;
        private int minGap = 166;
        private int maxGap = 200;
        private List<Wall> wallList;
        private Wall wall1, wall2;

        public WallSpawner(ContentManager content)
        {
            Wall.LoadTexture(content);
        }

        public List<Wall> Spawn(ContentManager content)
        {
            wallList = new List<Wall>();
            wall1 = new Wall(content);
            wall2 = new Wall(content);

            int gap = (int)Math.Floor(rand.NextDouble() * (maxGap - minGap + 1) + minGap);
            int maxHeight = _screenHeight - gap - minGap;
            int height = (int)Math.Floor(rand.NextDouble() * (maxHeight - minHeight + 1) + minHeight);
            int tileHeight = (int)Math.Ceiling((double)height / 64);
            int heightDifference = (tileHeight * 64) - height;

            wall1.X = _screenWidth;
            wall1.Y = 0;
            wall1.Width = width;
            wall1.Height = height;
            wall1.Y -= heightDifference;
            wall1.Height += heightDifference;
            wall1.TileCountHeight = tileHeight;

            // Console.WriteLine("tileHeight:" + tileHeight);
            // Console.WriteLine("heightDifference:" + heightDifference);
            // Console.WriteLine("wall1.Y:" + wall1.Y);
            // Console.WriteLine("tileHeight:" + tileHeight);
            // Console.WriteLine("height:" + height);

            wall2.X = _screenWidth;
            wall2.Y = height + gap;
            wall2.Width = width;
            wall2.Height = _screenHeight - height - gap;
            tileHeight = (int)Math.Ceiling((double)(_screenHeight - height - gap) / 64);
            wall2.TileCountHeight = tileHeight;

            // Creates a new texture with the correct TileCountHeight.
            // This can be fixed by passing the value at time of creation.
            wall1.Update();
            wall2.Update();

            wallList.Add(wall1);
            wallList.Add(wall2);
            return wallList;
        }
    }
}