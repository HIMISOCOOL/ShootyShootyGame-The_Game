using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace shipgame_windows
{
    /// <summary>
    /// An animation using a spritestrip
    /// </summary>
    class Animation
    {
        Texture2D spriteStrip;// The image representing the collection of images used for animation

        public Color[] TextureData;

        float scale;// The scale used to display the sprite strip

        public int elapsedTime;// The time since we last updated the frame

        int frameTime;// The time we display a frame until the next one in milliseconds

        int frameCount;// The number of frames that the animation contains

        int currentFrame;// The index of the current frame we are displaying

        public Color color;// The color of the frame we will be displaying

        Rectangle sourceRect = new Rectangle();// The area of the image strip we want to display

        Rectangle destinationRect = new Rectangle();// The area where we want to display the image strip in the game

        public int FrameWidth;// Width of a given frame        

        public int FrameHeight;// Height of a given frame

        public bool Active;// The state of the Animation

        public bool Looping;// Determines if the animation will keep playing or deactivate after one run

        public Vector2 Position;// Width of a given frame

        public Vector2 Origin;

        /// <summary>
        /// Initializes the Animation to a useable state
        /// </summary>
        /// <param name="sheet">The sprite strip used in the animation</param>
        /// <param name="texture">A single frame in the sprite strip used to calculate collision</param>
        /// <param name="position">The position of the animation on screen</param>
        /// <param name="frameWidth">The width of each frame</param>
        /// <param name="frameHeight">The height of each frame</param>
        /// <param name="frameCount">The number of frames on the sheet</param>
        /// <param name="frametime">The time it takes to complete the animaton</param>
        /// <param name="color">The color which is used to draw the object(white means no change)</param>
        /// <param name="scale">The Scale of the animation on screen</param>
        /// <param name="looping">Whether the animation loops</param>
        public void Initialize(Texture2D sheet, Texture2D texture, Vector2 position, int frameWidth, int frameHeight, int frameCount, int frameTime, Color color, float scale, bool looping)
        {
            // Keep a local copy of the values passed in

            this.color = color;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.frameCount = frameCount;
            this.frameTime = frameTime;
            this.scale = scale;
            TextureData = new Color[frameWidth * frameHeight];
            texture.GetData(TextureData);

            this.Looping = looping;
            this.Position = position;
            this.Origin = new Vector2(frameWidth / 2, frameHeight / 2);
            this.spriteStrip = sheet;
            
            // Set the time to zero
            this.elapsedTime = 0;
            this.currentFrame = 0;
            
            // Set the Animation to active by default
            this.Active = true; 
        }
        public void Initialize(Texture2D sheet, Vector2 position, int frameWidth, int frameHeight, int frameCount, int frameTime, Color color, float scale, bool looping)
        {
            // Keep a local copy of the values passed in

            this.color = color;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.frameCount = frameCount;
            this.frameTime = frameTime;
            this.scale = scale;

            this.Looping = looping;
            this.Position = position;
            this.Origin = new Vector2(frameWidth / 2, frameHeight / 2);
            this.spriteStrip = sheet;

            // Set the time to zero
            this.elapsedTime = 0;
            this.currentFrame = 0;

            // Set the Animation to active by default
            this.Active = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!this.Active) return;//if not active then don't update
            this.elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;//update the elapsed time

            if (this.elapsedTime > this.frameTime)//if elapsed time is larger than the frame time, need to switch frames
            {
                this.currentFrame++;//move to the next frame

                if (this.currentFrame == this.frameCount)//if the current frame is equal to the framecount, reset currentFrame
                {
                    this.currentFrame = 0;

                    if (!this.Looping)//if we arent looping stop the animation
                    {
                        this.Active = false;
                    }

                    this.elapsedTime = 0;//reset the elapsed time
                }
            }
            // Grab the correct frame in the image strip by multiplying the currentFrame index by the Frame width
            this.sourceRect = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);

            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            this.destinationRect = new Rectangle((int)Position.X - (int)(FrameWidth * scale) / 2, (int)Position.Y - (int)(FrameHeight * scale) / 2, (int)(FrameWidth * scale), (int)(FrameHeight * scale)); 
        }

        /// <summary>
        /// Draws the animation using the angle input by the controls
        /// </summary>
        /// <param name="spriteBatch">The engine which puts the animation on screen</param>
        /// <param name="angle">The angle at which the animation is drawn</param>
        public void Draw(SpriteBatch spriteBatch, float angle)
        {
            if (this.Active)//only draw the animation if active
            {
                spriteBatch.Draw(this.spriteStrip, this.destinationRect, this.sourceRect, this.color, angle, Origin, SpriteEffects.None, 1);
            }
        }

        /// <summary>
        /// Draws the animation on screen
        /// </summary>
        /// <param name="spriteBatch">The engine which puts the animation on screen</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.Active)//only draw the animation if active
            {
                spriteBatch.Draw(this.spriteStrip, this.destinationRect, this.sourceRect, this.color, 0, Origin, SpriteEffects.None, 1f);
            }
        }
    }
}
