using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics; 

namespace shipgame_windows
{
    class ParallaxingBackground
    {
        Texture2D texture;// The image representing the parallaxing background

        Vector2[] positions;// An array of positions of the parallaxing background

        int speed;// The speed which the background is moving

        int bgHeight;// Height of the background

        int bgWidth;// Width of the background

        public void Initialize(ContentManager content, String texturePath, int screenWitdth, int screenHeight, int speed)
        {
            bgHeight = screenHeight;
            bgWidth = screenWitdth;

            texture = content.Load<Texture2D>(texturePath);// Load the texture we will be using

            this.speed = speed;// Set the speed

            positions = new Vector2[screenWitdth / screenHeight + 1];// Divide screen with texture width to determine number of tiles needed, +1 so there is no gaps

            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = new Vector2(i * texture.Width, 0);
            }

        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < positions.Length; i++)//update the positions of the background
            {
                positions[i].X += speed;// Update the position of the screen by adding speed

                if (speed <= 0)
                {
                    if (positions[i].X <= -texture.Width)// Check texture is out of view then puts texture at the end of the screen
                    {
                        positions[i].X = texture.Width * (positions.Length - 1);
                    }
                }
                else// If speed has background moving right
                {
                    if (positions[i].X >= texture.Width * (positions.Length -1))// Check if texture is out of view then puts texture at the start of the screen
                    {
                        positions[i].X = -texture.Width;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                Rectangle rectBg = new Rectangle((int)positions[i].X, (int)positions[i].Y, bgWidth, bgHeight);
                spriteBatch.Draw(texture, rectBg, Color.White);
            }
        }
    }
}
