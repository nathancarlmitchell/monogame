using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace monogame
{
    public class Player : Object
    {
        protected ContentManager _content;
        private AnimatedTexture playerIdleTexture, playerJumpTexture;
        public AnimatedTexture currentTexture;
        // The rotation of the character on screen
        private const float rotation = 0;
        // The scale of the character, how big it is drawn
        private const float scale = 1f;
        // The draw order of the sprite
        private const float depth = 0.5f;
        // How many frames/images are included in the animation
        private const int frames = 2;
        // How many frames should be drawn each second, how fast does the animation run?
        private const int framesPerSec = 4;
        public int velocity = 12;
        private int jumpVelocity = 12;
        private const int maxVelocity = 64;

        public Player(ContentManager content)
        {
            _content = content;

            this.Height = 64;
            this.Width = 64;

            //playerTexture = _content.Load<Texture2D>("ball");
            //playerJumpTexture = _content.Load<Texture2D>("ball_jump");

            playerIdleTexture = new AnimatedTexture(new Vector2(this.Height / 2, this.Width / 2), rotation, scale, depth);
            // "AnimatedCharacter" is the name of the sprite asset in the project.
            playerIdleTexture.Load(_content, "anim_idle", frames, framesPerSec);

            playerJumpTexture = new AnimatedTexture(new Vector2(this.Height / 2, this.Width / 2), rotation, scale, depth);
            // "AnimatedCharacter" is the name of the sprite asset in the project.
            playerJumpTexture.Load(_content, "anim_jump", frames, framesPerSec);

            currentTexture = playerIdleTexture;
        }

        public void ChangeVelocity(int change)
        {
            if (currentTexture != playerIdleTexture)
            {
                currentTexture = playerIdleTexture;
            }

            if (Math.Abs(velocity) >= maxVelocity + 1)
            {
                return;
            }
            velocity += change;
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
            this.velocity = jumpVelocity;
        }
    }
}