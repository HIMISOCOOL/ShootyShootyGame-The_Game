using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace shipgame_windows
{
    /// <summary>
    /// An object to be dodged (eventually will explode on contact and be destroyable
    /// </summary>
    class Mine
    {
        public Animation Animation;

        public Texture2D Texture;

        public Vector2 Position;

        Vector2 startPosition;

        public Rectangle HitBox;

        public const float FallSpeed = 2;

        public int Width// The players width
        {
            get { return Animation.FrameWidth; }
        }

        public int Height// The players height
        {
            get { return Animation.FrameHeight; }
        }

        /// <summary>
        /// Creates a new Mine
        /// </summary>
        /// <param name="animation">The animation of the mine</param>
        /// <param name="texture">A single frame of the mine to allow for collision detection</param>
        /// <param name="position">The position of the frame on screen</param>
        public Mine(Animation animation,Texture2D texture, Vector2 position)
        {
            this.Animation = animation;
            this.Texture = texture;
            this.Position = position;
            this.startPosition = position;
            HitBox = setHitBox();
        }

        private Rectangle setHitBox()
        {
            return new Rectangle((int)Position.X - (this.Width / 2) + 12, (int)Position.Y - (this.Height / 2)-20, this.Width - 12, this.Height - 20);
        }

        public void Update(GameTime gameTime)
        {
            if (this.startPosition.Y < 0)
            {
                this.Position = new Vector2(this.Position.X,
                        this.Position.Y + FallSpeed);   
            }

            if (this.startPosition.X < 0)
            {
                this.Position = new Vector2(this.Position.X + FallSpeed,
                    this.Position.Y);
            }
            
            this.Animation.Position = this.Position;
            this.HitBox = setHitBox();
            this.Animation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.Animation.Draw(spriteBatch);
        }
    }
}
