using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace shipgame_windows
{
    public class Gun
    {
        public Vector2 Position;
        public List<Bullet> bullets = new List<Bullet>();
        public Texture2D playerBulletTexture;
        public int BulletCount
        {
            get { return bullets.Count; }
        }

        public Gun(Texture2D texture, Vector2 position)
        {
            this.playerBulletTexture = texture;
            this.Position = position;
        }

        public void Update(GameTime gameTime, Vector2 position)
        {
            this.Position = position;
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update(gameTime);

                if (bullets[i].TotalActiveTime > bullets[i].ActiveTime)
                    bullets.RemoveAt(i);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bullet b in bullets)
            {
                b.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Adds a new bullet on screen
        /// </summary>
        /// <param name="position">Bullet originates ehre</param>
        /// <param name="direction">The bullet faces this direction</param>
        /// <param name="speed">The bullet moves at this speed</param>
        /// <param name="activeTime">The time the bullet may be active</param>
        /// <param name="type">Whether the bullet is an enemies or players</param>
        public void AddBullet(Vector2 directon, float angle, float speed, int activeTime, BulletType type)
        {
            bullets.Add(new Bullet(playerBulletTexture, this.Position, directon, angle, speed, activeTime, type));
        }
    }
}
