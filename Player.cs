using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using monogame.States;

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
        private const int framesPerSec = 4;

        private const int maxVelocity = 64;
        public int JumpVelocity { get; } = 14;
        public int Velocity { get; set; } = 14;
        public String CurrentSkin { get; set; }

        public Player(ContentManager content)
        {
            _content = content;

            this.Height = 64;
            this.Width = 64;

            playerIdleTexture = new AnimatedTexture(new Vector2(this.Height / 2, this.Width / 2), rotation, scale, depth);
            playerIdleTexture.Load(_content, "anim_idle_default", frames, framesPerSec);

            playerJumpTexture = new AnimatedTexture(new Vector2(this.Height / 2, this.Width / 2), rotation, scale, depth);
            playerJumpTexture.Load(_content, "anim_jump_default", frames, framesPerSec);

            currentTexture = playerIdleTexture;
        }

        public void ChangeVelocity(int change)
        {
            if (currentTexture != playerIdleTexture)
            {
                currentTexture = playerIdleTexture;
                //currentTexture.Reset();
            }

            if (Math.Abs(this.Velocity) >= maxVelocity + 1)
            {
                return;
            }
            this.Velocity += change;
            Bounce();
        }

        public void Jump(int _jumpVelocity)
        {
            if (this.Velocity > -2)
            {
                if (currentTexture != playerJumpTexture)
                {
                    currentTexture = playerJumpTexture;
                    //currentTexture.Reset();
                }
                return;
            }
            this.Velocity = _jumpVelocity;
        }


        public void Bounce()
        {
            if (this.Y >= GameState.ScreenHeight - this.Height / 2)
            {
                if (this.Velocity == 0)
                {
                    this.Velocity = 2;
                    return;
                }
                this.Velocity = Math.Abs(this.Velocity) / 2;
            }
        }

        public void ChangeSkin(string skin)
        {
            switch (skin)
            {
                case "anim_idle_pink":
                    playerIdleTexture.Load(_content, "anim_idle_pink", frames, framesPerSec);
                    playerJumpTexture.Load(_content, "anim_jump_pink", frames, framesPerSec);
                    break;
                case "anim_idle_red":
                    playerIdleTexture.Load(_content, "anim_idle_red", frames, framesPerSec);
                    playerJumpTexture.Load(_content, "anim_jump_red", frames, framesPerSec);
                    break;
                case "anim_idle":
                    playerIdleTexture.Load(_content, "anim_idle_default", frames, framesPerSec);
                    playerJumpTexture.Load(_content, "anim_jump_default", frames, framesPerSec);
                    break;
            }

            currentTexture = playerIdleTexture;   
        }

    }
}