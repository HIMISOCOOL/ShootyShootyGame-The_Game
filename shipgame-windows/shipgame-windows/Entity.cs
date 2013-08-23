using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace shipgame_windows
{
    class Entity
    {
        public Animation Animation;
        public Vector2 Position;
        public Entity(Vector2 position, Animation animation)
        {
            this.Position = position;
            this.Animation = animation;
        }
        public Point GetPoint()
        {
            return new Point(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y));
        }
    }
}
