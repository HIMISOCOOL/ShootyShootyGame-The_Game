using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace shipgame_windows
{
    public enum GunLevel
    {
        one, two, three
    }

    class Player : Entity
    {
        public float Angle { get; set; }// The angle the model faces

        public Boolean Keyboard { get; set; }// Whether the player uses the keyboard and mouse or not

        public Boolean hit;// Whether the player is hit

        Vector2 Direction;// The Direction the player is facing, used in drawing

        BulletType type;// The type of bullets the player uses


        public bool Active { get; set; }// Whether the player is active

        public int Health { get; set; }// The players heatlth

        public int Score { get; set; }// The players score

        public Rectangle HitBox;// The hitbox of the player

        public int fireRate { get; set; }

        public Gun[] Guns;

        GunLevel Level;

        public int elapsedTime;// The time since the last shot

        public int Width// The players width
        {
            get { return this.Animation.FrameWidth; }
        }

        public int Height// The players height
        {
            get { return this.Animation.FrameHeight; }
        }

        public Boolean Firing;
        public int modifier;

        /// <summary>
        /// Player is a controlable player
        /// </summary>
        /// <param name="animation">The animation which plays as the object is active</param>
        /// <param name="gun">The gun which fires the lasers for the player</param>
        /// <param name="Position">The position of the Player on screen</param>
        /// <param name="keyboard">Wheather or not the player uses the keyboard(only one keyboard at a time is supported)</param>
        public Player(Animation animation, Gun gun, Vector2 position, Boolean keyboard)
            : base(position, animation)
        {
            this.Guns = new Gun[3];
            this.Guns[0] = gun;
            this.Keyboard = keyboard;
        }

        /// <summary>
        /// Initalizes the Player object
        /// </summary>
        public void Initialize()//the vertical texture, the horizontal texture and the starting position of the player 
        {
            this.HitBox = setHitBox();// Generates the hitbox
            this.Active = this.Animation.Active;//the player starts active
            this.Health = 100;
            this.Angle = (float)(Math.PI * 0 / 180);// The animation will be drawn at its default rotation
            this.Firing = false;
            this.hit = false;
            this.elapsedTime = 0;
            this.fireRate = 150;
            type = BulletType.Player;
            this.Score = 0;
            modifier = 1;
            this.Level = GunLevel.one;
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
                Guns[0].AddBullet(this.Direction, this.Angle, 15f, 2000, type);
                Firing = false;
            }

            Animation.Position = this.Position;
            this.HitBox = setHitBox();
            if (hit)
            {
                Animation.color = Color.Red;
                this.hit = false;
            }
            else
            {
                if (Animation.elapsedTime > 150)
                {
                    Animation.color = Color.White;
                }
            }

            if (Level == GunLevel.two)
            {
                Guns[1] = new Gun(Guns[0].playerBulletTexture, new Vector2(this.Position.X - this.Width / 2, this.Position.Y - this.Height / 2));
            }
            if (Level == GunLevel.three)
            {
                Guns[2] = new Gun(Guns[0].playerBulletTexture, new Vector2(this.Position.X - this.Width / 2, this.Position.Y - this.Height / 2));
            }
            Animation.Update(gameTime);
            foreach (Gun g in Guns)
            {
                if (g != null)
                {
                    g.Update(gameTime, new Vector2(this.Position.X - this.Width / 2, this.Position.Y - this.Height / 2));
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Animation.Draw(spriteBatch, Angle);
            Guns[0].Draw(spriteBatch);
        }
    }
}
