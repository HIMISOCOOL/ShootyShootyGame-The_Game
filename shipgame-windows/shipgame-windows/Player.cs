using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace shipgame_windows
{
    class Player
    {
        public Animation PlayerAnimation;// The animation of the player including the texture

        public float Angle { get; set; }// The angle the model faces

        public Boolean Keyboard { get; set; }// Whether the player uses the keyboard and mouse or not

        public Boolean hit;

        Vector2 Direction;

        BulletType type;

        public Vector2 Position;// The current position of the player

        public bool Active { get; set; }// Whether the player is active

        public int Health { get; set; }// The players heatlth

        public int Score { get; set; }

        public Rectangle HitBox;

        public int fireRate { get; set; }

        public Gun Gun;
        
        public int elapsedTime;// The time since the last shot

        public int Width// The players width
        {
            get { return PlayerAnimation.FrameWidth; }
        }

        public int Height// The players height
        {
            get { return PlayerAnimation.FrameHeight; }
        }

        public Boolean Firing;
        public int modifier;

        /// <summary>
        /// Initalizes the Player object
        /// </summary>
        /// <param name="animation">The animation which plays as the object is active</param>
        /// <param name="Position">The position of the Player on screen</param>
        /// <param name="keyboard">Wheather or not the player uses the keyboard(only one keyboard at a time is supported)</param>
        public void Initialize(Animation animation, Gun gun, Vector2 Position, Boolean keyboard)//the vertical texture, the horizontal texture and the starting position of the player 
        {
            this.PlayerAnimation = animation;// starts horizontal
            this.Position = Position;//starts in the centre
            this.HitBox = setHitBox();// Generates the hitbox
            this.Active = this.PlayerAnimation.Active;//the player starts active
            this.Health = 100;
            this.Angle = (float)(Math.PI * 0 / 180);// The animation will be drawn at its default rotation
            this.Keyboard = keyboard;
            this.Firing = false;
            this.hit = false;
            this.elapsedTime = 0;
            this.fireRate = 150;
            this.Gun = gun;
            type = BulletType.Player;
            this.Gun.Level = GunLevel.one;
            this.Score = 0;
            modifier = 1;
        }

        private Rectangle setHitBox()
        {
            return new Rectangle((int)this.Position.X - this.Width / 2, (int)this.Position.Y - this.Height / 2, this.Width, this.Height);
        }

        /// <summary>
        /// Updates the player object
        /// </summary>
        /// <param name="gameTime">The State of the game at the immediate second</param>
        /// <param name="rotation">The rotation of the player at the moment</param>
        public void Update(GameTime gameTime, Vector2 rotation)
        {
           Direction = (Position) - rotation;
            if (Keyboard)
            {
                Angle = (float)(Math.Atan2(-Direction.Y, -Direction.X));
            }
            else
            {
                rotation.Normalize();
                if ((rotation.X >= 0 || rotation.X <= 0) || (rotation.Y >= 0 || rotation.Y <= 0))// if there is a change in the right stick
                {
                    Angle = -(float)Math.Atan2(rotation.Y, rotation.X);
                }
            }
            if (Firing)
            {
                Gun.AddBullet(new Vector2(this.Position.X-this.Width/2,this.Position.Y-this.Height/2), this.Direction, this.Angle, 15f, 2000, type);
                Firing = false;
            }

            PlayerAnimation.Position = this.Position;
            this.HitBox = setHitBox();
            if (hit)
            {
                PlayerAnimation.color = Color.Red;
                this.hit = false;
            }
            else
            {
                if (PlayerAnimation.elapsedTime>150)
                {
                    PlayerAnimation.color = Color.White;   
                }
            }
            PlayerAnimation.Update(gameTime);
            Gun.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerAnimation.Draw(spriteBatch, Angle);
            Gun.Draw(spriteBatch);
        }
    }
}
/*
public void Update(GameTime gameTime, String keyPressed)
        {
            playeranimation.position = position;
            updateDirection(keyPressed);
            playeranimation.update(gametime);
        }

public void updateDirection(String keyPressed)//the update which occurs on a keypress
        {
            switch (keyPressed)
            {
                case ("up"):
                    {
                        this.Direction = 'u';
                        this.Angle = (float)(Math.PI * 270 / 180);
                        break;
                    }

                case ("down"):
                    {
                        this.Direction = 'd';
                        this.Angle = (float)(Math.PI * 90 / 180);
                        break;
                    }

                case ("left"):
                    {
                        this.Direction = 'l';
                        this.Angle = (float)(Math.PI * 180 / 180);
                        break;
                    }

                case ("right"):
                    {
                        this.Direction = 'r';
                        this.Angle = (float)(Math.PI * 0 / 180);
                        break;
                    }

                default:
                    break;
            }
        }
*/
