using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using monogame.States;

namespace monogame.Controls
{
    public class Menu
    {
        private List<Button> _components;
        private int _difference;
        private int _centerHeight;
        private int _centerWidth;
        
        public Menu(List<Button> components)
        {
            _components = components;
            _centerHeight = MenuState.CenterHeight;
            _centerWidth = MenuState.ControlWidthCenter;

            for (int i = 0; i < _components.Count; i++)
            {
                int totalComponents = _components.Count;
                int centerComponent = totalComponents / 2;
                _difference = i - centerComponent;
                _components[i].Position = new Vector2(_centerWidth, _centerHeight + _difference * 50);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].Draw(gameTime, spriteBatch);
            }
        }
    }
}