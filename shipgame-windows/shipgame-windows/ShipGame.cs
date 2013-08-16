using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace shipgame_windows
{
    class ShipGame
    {
        #region Properties

        // System properties
        ContentManager Content;
        GameWindow Window;
        Viewport Viewport;

        // Explosion properties
        Texture2D explosionTexture;
        List<Explosion> Explosions;

        // Player properties
        Texture2D playerSpriteSheet;
        Texture2D playerTexture;
        Player[] players;
        public const int NumberOfPlayers = 1;
        Texture2D BulletTexture;

        // Sound
        SoundEffect bgSound;
        SoundEffectInstance bgInstance;
        SoundEffect fireFX;
        SoundEffect exploFX;

        // Mine properties
        Texture2D mineSpriteSheet;
        Texture2D mineTexture;
        List<Mine> Mines;
        float BlockSpawnProbability = 0.01f;
        Random random;

        // Font
        SpriteFont font;
        int hit;

        // Background
        Texture2D mainBackground;
        Rectangle rectBackground;
        ParallaxingBackground bgLayer1;
        ParallaxingBackground bgLayer2;
        //ScrollingBackground background;


        // Keyboard states used to determine key presses
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;


        // Gamepad states used to determine button presses
        GamePadState currentGamePadState;
        GamePadState previousGamePadState;


        // Thumbstick states used to determine player position and rotation respectivley
        Vector2 leftStick;
        Vector2 rightStick;

        // Mouse states used to find the mouse position
        MouseState currentMouseState;
        MouseState previousMouseState;
        Vector2 mouseLocation;


        // A movement speed for the player
        float playerMoveSpeed;
        float playerThumbMoveSpeed;
        #endregion

        public ShipGame(ContentManager content, GameWindow window)
        {
            this.Content = content;
            this.Window = window;
            this.Viewport = new Viewport(0, 0, 1280, 800);
        }

        #region Initialize
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public void Initialize()
        {
            Mines = new List<Mine>();
            Explosions = new List<Explosion>();
            random = new Random();
            rectBackground = new Rectangle();
            makePlayers();

            //Background
            bgLayer1 = new ParallaxingBackground();
            bgLayer2 = new ParallaxingBackground();

            playerMoveSpeed = 10.0f;
            playerThumbMoveSpeed = 10.0f;
            hit = 0;
            
        }

        private void makePlayers()
        {
            players = new Player[NumberOfPlayers];
            for (int i = 0; i < players.Length; i++)
            {
                players[i] = new Player();
            }
            //players[1].Keyboard = true;
        }
        #endregion

        #region Load/Unload
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public void LoadContent()
        {
            // LoadTextures
            explosionTexture = Content.Load<Texture2D>("Graphics\\explosion");
            playerTexture = Content.Load<Texture2D>("Graphics\\ship");
            playerSpriteSheet = Content.Load<Texture2D>("Graphics\\shipAnimation");
            mineTexture = Content.Load<Texture2D>("Graphics\\mine");
            mineSpriteSheet = Content.Load<Texture2D>("Graphics\\mineAnimation");
            BulletTexture = Content.Load<Texture2D>("Graphics\\laser");

            // Load Sounds
            bgSound = Content.Load<SoundEffect>("Sound\\gameMusic");
            bgInstance = bgSound.CreateInstance();
            bgInstance.IsLooped = true;
            bgInstance.Volume = 0.5f;
            bgInstance.Play();

            fireFX = Content.Load<SoundEffect>("Sound\\laserFire");
            exploFX = Content.Load<SoundEffect>("Sound\\explosion");

            // Load spriteFont
            font = Content.Load<SpriteFont>("Graphics\\gameFont");

            // Load the parallaxing background
            bgLayer1.Initialize(Content, "Graphics\\bgLayer1", Viewport.Width, Viewport.Height, -1);
            bgLayer2.Initialize(Content, "Graphics\\bgLayer2", Viewport.Width, Viewport.Height, 1);
            rectBackground = new Rectangle(0, 0, Viewport.Width, Viewport.Height);
            mainBackground = Content.Load<Texture2D>("Graphics\\mainbackground");

            // Load the scrolling background
            //background = new ScrollingBackground(Window.ClientBounds.Width, Window.ClientBounds.Height, Content.Load<Texture2D>("Graphics\\Background01"), Content.Load<Texture2D>("Graphics\\Background02"),Content.Load<Texture2D>("Graphics\\Background03"), Content.Load<Texture2D>("Graphics\\Background04"), Content.Load<Texture2D>("Graphics\\Background05"));

            loadPlayers();// Load the players
        }

        private void loadPlayers()
        {
            foreach (Player player in players)
            {
                Animation playerAnimation = new Animation();
                playerAnimation.Initialize(playerSpriteSheet, playerTexture, Vector2.Zero, 115, 69, 8, 60, Color.White, 1f, true);
                Vector2 playerPosition = new Vector2(Viewport.TitleSafeArea.X + Viewport.TitleSafeArea.Width / 2,
                Viewport.TitleSafeArea.Y + Viewport.TitleSafeArea.Height / 2);
                Gun g = new Gun(BulletTexture);
                player.Initialize(playerAnimation, g, playerPosition, true);
            }
            //players[1].Keyboard = true;
        }

        #endregion

        #region Update
        public void Update(GameTime gameTime)
        {
            if (currentKeyboardState.IsKeyDown(Keys.F1))
            {
                if (bgInstance.State == SoundState.Stopped)
                {
                    bgInstance.IsLooped = true;
                    bgInstance.Volume = 0.5f;
                    bgInstance.Play();
                }
                else bgInstance.Resume();
            }
            if (currentKeyboardState.IsKeyDown(Keys.F2))
            {
                if (bgInstance.State == SoundState.Playing) { bgInstance.Pause(); }
            }

            previousGamePadState = currentGamePadState;

            previousKeyboardState = currentKeyboardState;
            previousMouseState = currentMouseState;


            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();

            foreach (Player player in players)
            {
                UpdatePlayer(gameTime, player);
            }

            UpdateMines(gameTime);

            UpdateExplosions(gameTime);
            // Update the parallaxing background
            bgLayer1.Update(gameTime);
            bgLayer2.Update(gameTime);
        }

        private void UpdateExplosions(GameTime gameTime)
        {
            for (int i = 0; i < Explosions.Count; i++)
            {
                Explosions[i].Update(gameTime);
                if (!Explosions[i].Animation.Active)
                {
                    Explosions.Remove(Explosions[i]);
                    i--;
                }
            }
        }

        private void UpdateMines(GameTime gameTime)
        {
            // Spawn new falling blocks
            if (random.NextDouble() < BlockSpawnProbability)
            {
                Animation mineAnimation = new Animation();
                mineAnimation.Initialize(mineSpriteSheet, mineTexture, Vector2.Zero, 47, 61, 8, 30, Color.White, 1f, true);
                int next = random.Next(3);
                if (next == 1)
                {
                    float x = (float)random.NextDouble() *
                    (Viewport.TitleSafeArea.Width - mineSpriteSheet.Width / 8);
                    Mines.Add(new Mine(mineAnimation, mineTexture, new Vector2(x, -mineSpriteSheet.Height)));
                }
                if (next == 2)
                {
                    float y = (float)random.NextDouble() *
                    (Viewport.TitleSafeArea.Height - mineSpriteSheet.Height);
                    Mines.Add(new Mine(mineAnimation, mineTexture, new Vector2(-mineSpriteSheet.Width / 8, y)));
                }
            }

            // Update each Mine
            for (int i = 0; i < Mines.Count; i++)
            {
                // Animate this block falling
                Mines[i].Update(gameTime);

                // Check collision with person
                foreach (Player player in players)
                {
                    if (IntersectPixels(player.HitBox, player.PlayerAnimation.TextureData,
                Mines[i].HitBox, Mines[i].Animation.TextureData))
                    {
                        player.hit = true;
                        makeExplosion(Mines[i].Position);
                        exploFX.Play();
                        Mines.RemoveAt(i);
                        hit++;
                        i--;
                        break;
                    }

                    foreach (Bullet b in player.Gun.bullets)
                    {
                        try
                        {
                            if (IntersectPixels(b.HitBox, b.TextureData, Mines[i].HitBox, Mines[i].Animation.TextureData))
                            {
                                makeExplosion(Mines[i].Position);
                                exploFX.Play();
                                Mines.RemoveAt(i);
                                i--;
                            }
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            break;
                        }
                    }
                    try
                    {
                        // Remove mine if it have fallen off the screen
                        if (Mines[i].Position.Y > Window.ClientBounds.Height || Mines[i].Position.X > Window.ClientBounds.Width)
                        {
                            Mines.RemoveAt(i);

                            // When removing a block, the next block will have the same index
                            // as the current block. Decrement i to prevent skipping a block.
                            i--;
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        break;
                    }
                }
            }
        }

        private void makeExplosion(Vector2 position)
        {
            Animation explosion = new Animation();
            explosion.Initialize(explosionTexture, position, 134, 134, 12, 30, Color.OrangeRed, 0.5f, false);
            Explosions.Add(new Explosion(explosion, position));
        }

        private void UpdatePlayer(GameTime gameTime, Player player)
        {
            if (currentKeyboardState.IsKeyDown(Keys.F3))// This will fall appart when there are 2 players
            {
                player.Keyboard = !player.Keyboard;
            }

            if (player.Keyboard)
            {
                //getMouseLocation
                mouseLocation = new Vector2(currentMouseState.X, currentMouseState.Y);

                if (currentKeyboardState.IsKeyDown(Keys.Left))
                {
                    //background.shiftRight((int)playerMoveSpeed * 2);
                    player.Position.X -= playerMoveSpeed;

                }

                if (currentKeyboardState.IsKeyDown(Keys.Right))
                {
                    //background.shiftLeft((int)playerMoveSpeed * 2);
                    player.Position.X += playerMoveSpeed;

                }

                if (currentKeyboardState.IsKeyDown(Keys.Up))
                {
                    player.Position.Y -= playerMoveSpeed;

                }

                if (currentKeyboardState.IsKeyDown(Keys.Down))
                {
                    player.Position.Y += playerMoveSpeed;

                }

                if (currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    player.elapsedTime += (int)gameTime.ElapsedGameTime.Milliseconds;
                    if (player.elapsedTime >= player.fireRate)
                    {
                        player.elapsedTime = 0;
                        player.Firing = true;
                        fireFX.Play();
                    }
                    player.Update(gameTime, mouseLocation);
                }
                else
                {
                    player.Firing = false;
                    player.Update(gameTime, mouseLocation);
                }
            }
            else
            {
                //get thumbstick controls
                leftStick = currentGamePadState.ThumbSticks.Left;
                rightStick = currentGamePadState.ThumbSticks.Right;
                if (leftStick.X > 0)
                {
                    player.Position.X += leftStick.X * playerThumbMoveSpeed;
                    //background.shiftLeft((int)playerMoveSpeed*2);
                }

                if (leftStick.X < 0)
                {
                    player.Position.X += leftStick.X * playerThumbMoveSpeed;
                    //background.shiftRight((int)playerMoveSpeed*2);
                }
                player.Position.Y -= leftStick.Y * playerThumbMoveSpeed;

                if (currentGamePadState.DPad.Left == ButtonState.Pressed)
                {
                    //background.shiftRight((int)playerMoveSpeed * 2);
                    player.Position.X -= playerMoveSpeed;
                }

                if (currentGamePadState.DPad.Right == ButtonState.Pressed)
                {
                    //background.shiftLeft((int)playerMoveSpeed * 2);
                    player.Position.X += playerMoveSpeed;
                    player.Update(gameTime, rightStick);
                }

                if (currentGamePadState.DPad.Up == ButtonState.Pressed)
                {
                    player.Position.Y -= playerMoveSpeed;
                }

                if (currentGamePadState.DPad.Down == ButtonState.Pressed)
                {
                    player.Position.Y += playerMoveSpeed;
                }

                if (currentGamePadState.Triggers.Right > 0)
                {
                    player.elapsedTime += (int)gameTime.ElapsedGameTime.Milliseconds;
                    if (player.elapsedTime >= player.fireRate)
                    {
                        player.Firing = true;
                        fireFX.Play();
                    }
                    player.Update(gameTime, rightStick);
                }
                else
                {
                    //player.Firing = false;
                    player.Update(gameTime, rightStick);
                }
            }

            //make sure the player is not out of bounds
            player.Position.X = MathHelper.Clamp(player.Position.X, player.HitBox.Width, Viewport.Width);
            player.Position.Y = MathHelper.Clamp(player.Position.Y, player.HitBox.Height, Viewport.Height);
        }

        /// <summary>
        /// Determines if there is overlap of the non-transparent pixels
        /// between two sprites.
        /// </summary>
        /// <param name="rectangleA">Bounding rectangle of the first sprite</param>
        /// <param name="dataA">Pixel data of the first sprite</param>
        /// <param name="rectangleB">Bouding rectangle of the second sprite</param>
        /// <param name="dataB">Pixel data of the second sprite</param>
        /// <returns>True if non-transparent pixels overlap; false otherwise</returns>
        static bool IntersectPixels(Rectangle rectangleA, Color[] dataA,
                                    Rectangle rectangleB, Color[] dataB)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.Left) +
                                         (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }
        #endregion

        #region Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw the Main Background Texture
            //background.Draw(spriteBatch);
            spriteBatch.Draw(mainBackground, rectBackground, Color.White);
            spriteBatch.DrawString(font, "Hit: " + hit, new Vector2(100, Viewport.TitleSafeArea.Y + Viewport.TitleSafeArea.Height / 2), Color.Black, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, "Keyboard: " + players[0].Keyboard, new Vector2(100, (Viewport.TitleSafeArea.Y + Viewport.TitleSafeArea.Height / 2) + 25), Color.Black, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);


            // Draw the moving background
            bgLayer1.Draw(spriteBatch);
            bgLayer2.Draw(spriteBatch);

            // Draw the Player
            foreach (Player player in players)
            {
                player.Draw(spriteBatch);
            }

            // Draw the mines
            foreach (Mine mine in Mines)
            {
                mine.Draw(spriteBatch);
            }

            foreach (Explosion ex in Explosions)
            {
                ex.Draw(spriteBatch);
            }
        }
        #endregion
    }
}
