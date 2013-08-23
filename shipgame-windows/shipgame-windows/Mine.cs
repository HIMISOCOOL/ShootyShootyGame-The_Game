using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace shipgame_windows
{
    /// <summary>
    /// An enemy object designed to be shot at and explode
    /// </summary>
    class Mine : Entity
    {
        public Texture2D Texture;

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
        public Mine(Animation animation, Texture2D texture, Vector2 position)
            : base(position, animation)
        {
            this.Texture = texture;
            this.startPosition = position;
            HitBox = setHitBox();
        }

        private Rectangle setHitBox()
        {
            return new Rectangle((int)Position.X - (this.Width / 2) + 12, (int)Position.Y - (this.Height / 2) - 20, this.Width - 12, this.Height - 20);
        }

        public void Update(GameTime gameTime, GameWindow window)
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

            if (this.startPosition.Y > window.ClientBounds.Height)
            {
                this.Position = new Vector2(this.Position.X,
                        this.Position.Y - FallSpeed);
            }

            if (this.startPosition.X > window.ClientBounds.Width)
            {
                this.Position = new Vector2(this.Position.X - FallSpeed,
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
