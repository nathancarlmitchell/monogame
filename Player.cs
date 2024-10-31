using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace monogame
{
    public class Player : Object
    {
        protected ContentManager _content;
        private Texture2D playerTexture;
        private Texture2D playerJumpTexture;
        public Texture2D currentTexture;
        public int velocity = 0;
        public int maxVelocity = 12;

        public Player(ContentManager content)
        {
            _content = content;

            playerTexture = _content.Load<Texture2D>("ball");
            playerJumpTexture = _content.Load<Texture2D>("ball_jump");

            currentTexture = playerTexture;
        }

        public void ChangeVelocity(int change)
        {
            if (Math.Abs(this.velocity) >= maxVelocity + 1)
                if (currentTexture != playerTexture)
                {
                    currentTexture = playerTexture;
                    return;
                }
            this.velocity += change;
        }

        public void Jump()
        {
            if (this.velocity > -2)
            {
                if (currentTexture != playerJumpTexture)
                {
                    currentTexture = playerJumpTexture;  
                }
                return;
            }
            this.velocity = maxVelocity;
        }
    }
}