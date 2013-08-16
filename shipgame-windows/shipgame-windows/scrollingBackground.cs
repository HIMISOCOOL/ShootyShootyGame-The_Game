using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace shipgame_windows
{
    class ScrollingBackground
    {
        Texture2D[] Screens;
        int[] Left;
        int Width;
        int Height;

        public ScrollingBackground(int width, int height, params Texture2D[] screens)
        {
            this.Width = width;
            this.Height = height;
            this.Screens = screens;
            Left = new int[this.Screens.Length];
            Left[0] = 0;
            for (int i = 1; i < this.Screens.Length ; i++)
            {
                this.Left[i] = this.Left[i - 1] + this.Width;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Screens.Length; i++)
            {
                spriteBatch.Draw(Screens[i], new Rectangle(Left[i], 0, Width, Height), Color.White);
            }
        }

        public void shiftLeft(int dx)
        {
            for (int i = 0; i < Left.Length; i++)
            {
                if (Left[i] < -Width)
                {
                    Left[i] = Left[i] + Width * Left.Length;
                }
            }
            for (int i = 0; i < Left.Length; i++)
            {
                Left[i] -= dx;
            }
        }

        public void shiftRight(int dx)
        {
            for (int i = 0; i < Left.Length; i++)
            {
                if (Left[i] > Width)
                {
                    Left[i] = Left[i] - Width * Left.Length;
                }
            }
            for (int i = 0; i < Left.Length; i++)
            {
                Left[i] += dx;
            }
        }
    }
}
