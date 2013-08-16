using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace shipgame_windows// Shooty shooty game the game
{
    enum State
    {
        One,Two
    }
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class RunGame : Microsoft.Xna.Framework.Game
    {
        #region properties
        // System properties
        GraphicsDeviceManager graphicsManager;
        SpriteBatch spriteBatch;

        // Game properties
        State state;
        ShipGame game;
        Matrix spriteScale;

        // Splash Screen properties
        Texture2D splashScreen;
        Rectangle fullScreenRectangle;

        #endregion 

        public RunGame()
            : base()
        {
            graphicsManager = new GraphicsDeviceManager(this);
            graphicsManager.IsFullScreen = true;
            graphicsManager.PreferredBackBufferHeight = 720;
            graphicsManager.PreferredBackBufferWidth = 1280;
            Content.RootDirectory = "Content";
            state = State.One;
            Window.Title = "Shooty Shooty Game-The Game";
            game = new ShipGame(Content, Window);
        }

        #region Initialize
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            game.Initialize();
            base.Initialize();
        }
        #endregion

        #region Load/Unload
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            float screenscale = graphicsManager.GraphicsDevice.Viewport.Width / 1280f;
            spriteScale = Matrix.CreateScale(screenscale, screenscale, 1);
            splashScreen = Content.Load<Texture2D>("Graphics\\mainMenu");
            fullScreenRectangle = new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height);
            game.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        #endregion

        #region Update
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            this.IsMouseVisible = true;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (state)
            {
                case State.One:
                    // Menu State
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter)||GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
                    {
                        state = State.Two;
                    }
                    break;
                case State.Two:
                    // Game State
                    game.Update(gameTime);
                    break;
                default:
                    break;
            }
            base.Update(gameTime);
        }
        #endregion

        #region Draw
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // Start drawing
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, spriteScale);

            switch (state)
            {
                case State.One:
                    // Menu State
                    spriteBatch.Draw(splashScreen, fullScreenRectangle, Color.White);
                    break;
                case State.Two:
                    // Game State
                    game.Draw(spriteBatch);
                    break;
                default:
                    break;
            }

            // Stop drawing
            spriteBatch.End();
            base.Draw(gameTime);
        }
        #endregion
    }
}