using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace shipgame_windows
{
    /// <summary>
    /// Allows for enemies an players to fire bullets
    /// </summary>
    public enum BulletType { Player, Enemy }

    public class Bullet
    {
        public BulletType Type { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle HitBox;
        public Color[] TextureData;
        public float Angle { get; set; }
        public float Speed { get; set; }
        public int ActiveTime { get; set; }
        public int TotalActiveTime { get; set; }

        /// <summary>
        /// A bullet which takes down the health of a player or enemy
        /// </summary>
        /// <param name="texture">The texture of the bullet</param>
        /// <param name="position">The position of the bullet reletive to the window</param>
        /// <param name="direction">The Direction of the bullet</param>
        /// <param name="speed">The speed the bullet is moving</param>
        /// <param name="activeTime">The time the bullet may be active for</param>
        /// <param name="type">The type of bullet</param>
        public Bullet(Texture2D texture, Vector2 position, Vector2 direction, float angle, float speed, int activeTime, BulletType type)
        {
            this.Texture = texture;
            this.Position = position;
            this.Angle = angle;
            this.Speed = speed;
            this.ActiveTime = activeTime;
            this.Type = type;
            HitBox = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Texture.Width, this.Texture.Height);
            this.TotalActiveTime = 0;
            TextureData = new Color[Texture.Width * Texture.Height];
            Texture.GetData(TextureData);
        }

        public void Update(GameTime gameTime)
        {
            Vector2 direction = new Vector2((float)Math.Cos(Angle), (float)Math.Sin(Angle));
            this.Position += direction * Speed;// -Angle, Angle fires at / this angle, Angle, Angle fires at \ this angle neither are correct..
            this.HitBox = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Texture.Width, this.Texture.Height);
            this.TotalActiveTime += gameTime.ElapsedGameTime.Milliseconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, Angle, new Vector2(Texture.Width / 2, Texture.Height / 2), 1.0f, SpriteEffects.None, 0.8f);
        }
    }
}
