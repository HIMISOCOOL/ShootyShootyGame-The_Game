using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace shipgame_windows
{
    class Explosion: Entity
    {
        public Explosion(Animation animation, Vector2 position)
            :base(position, animation) { }

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
