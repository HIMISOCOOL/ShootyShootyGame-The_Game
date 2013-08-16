using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace shipgame_windows
{
    class Explosion
    {
        public Animation Animation;

        Vector2 Position;

        public Explosion(Animation animation, Vector2 position)
        {
            this.Animation = animation;
            this.Position = position;
        }

        public void Update(GameTime gameTime)
        {
            Animation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Animation.Draw(spriteBatch);
        }
    }
}
