using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using monogame.States;

namespace monogame
{
    public class Skin : Object
    {
        protected ContentManager _content;
        private AnimatedTexture _texture;
        private const int frames = 2;
        private const int framesPerSec = 2;
        public String Name { get; set; }
        private MouseState _currentMouse, _previousMouse;
        private Rectangle targetRectangle;
        private bool _isHovering;
        public bool Selected { get; set; }
        public Vector2 Position { get; set; }
        public event EventHandler Click;
        public bool Clicked { get; private set; }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 64, 64);
            }
        }

        public Skin(ContentManager content, String texture)
        {
            _content = content;

            this.Name = texture;

            // Load the texture.
            _texture = new AnimatedTexture(new Vector2(0, 0), 0, 1f, 0.5f);
            _texture.Load(_content, texture, frames, framesPerSec);

            // Define a drawing rectangle based on the number of tiles wide and high, using the texture dimensions.
            targetRectangle = new Rectangle(0, 0, 64, 64);
        }

        public void Activate()
        {
            string _name = this.Name.Split("_").Last();
            _texture.Load(_content, "anim_jump_" + _name, frames, framesPerSec);
        }

        public void Deactivate()
        {
            string _name = this.Name.Split("_").Last();
            _texture.Load(_content, "anim_idle_" + _name, frames, framesPerSec);
        }


        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            _isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;

                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }

            _texture.UpdateFrame(elapsed);

        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            var color = Color.Gray;

            if (this.Selected)
            {
                color = Color.White;
            }

            if (_isHovering)
            {
                color = Color.AntiqueWhite;
            }

            _texture.DrawFrame(_spriteBatch, this.Position, color);
        }

    }
}