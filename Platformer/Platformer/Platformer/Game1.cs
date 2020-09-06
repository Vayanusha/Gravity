using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Platformer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState keys, oldkeys;
        Rectangle PlayerRect = new Rectangle(0, 450, 75, 75);
        List<Rectangle> Platforms;
        int upMomentum;
        int tenFrameCounter = 10;
        int stage = 1;
        bool lose = false;
        bool platformUp = true;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 1200;
        }

        /// <summary>   
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Platforms = new List<Rectangle>();
            Platforms.Add(new Rectangle(200, 0, 30, 300));
            Platforms.Add(new Rectangle(400, 300, 30, 300));
            Platforms.Add(new Rectangle(600, 0, 30, 300));
            Platforms.Add(new Rectangle(800, 300, 30, 300));
            Platforms.Add(new Rectangle(1000, 0, 30, 300));

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            keys = Keyboard.GetState();

            if (keys.IsKeyDown(Keys.Right))
            {
                PlayerRect.X += 10;
            }
            if (keys.IsKeyDown(Keys.Left) && PlayerRect.X > 0)
            {
                PlayerRect.X -= 10;
            }
            if (keys.IsKeyDown(Keys.Up) && oldkeys.IsKeyUp(Keys.Up))
            {
                upMomentum += 3;
            }
            if (keys.IsKeyDown(Keys.Down) && oldkeys.IsKeyUp(Keys.Down))
            {
                PlayerRect.Y--;
                upMomentum -= 3;
            }

            if (tenFrameCounter > 0)
            {
                tenFrameCounter--;
            }

            if (PlayerRect.Y <= 0)
            {
                upMomentum = 0;
            }
            if (tenFrameCounter == 0)
            {
                tenFrameCounter = 10;
                upMomentum--;
            }

            for (int i = 0; i < Platforms.Count(); i++)
            {
                if (PlayerRect.Intersects(Platforms[i]))
                {
                    lose = true;
                }
            }
            if (PlayerRect.Y > 600)
            {
                lose = true;
            }

            if (lose)
            {
                PlayerRect = new Rectangle(0, 450, 75, 75);
                lose = false;
                upMomentum = 0;
                tenFrameCounter = 10;
            }

            if (PlayerRect.X > 1200)
            {
                stage++;
                PlayerRect = new Rectangle(0, 450, 75, 75);
                if (stage == 3)
                {
                    Platforms[0] = new Rectangle(400, 300, 30, 300);
                    Platforms[1] = new Rectangle(800, 300, 30, 300);
                    Platforms[2] = new Rectangle(0, 9999, 900, 30);
                    Platforms[3] = new Rectangle(0, 9999, 900, 30);
                    Platforms[4] = new Rectangle(0, 9999, 900, 30);
                }
            }

            if (stage == 2)
            {
                Platforms[0] = new Rectangle(0, 350, 900, 30);
                Platforms[1] = new Rectangle(300, 100, 900, 30);
                Platforms[2] = new Rectangle(1170, 100, 30, 900);
                Platforms[3] = new Rectangle(0, 9999, 900, 30);
                Platforms[4] = new Rectangle(0, 9999, 900, 30);
            }
            if (stage == 3)
            {
                if (platformUp)
                {
                    Platforms[0] = new Rectangle(Platforms[0].X, Platforms[0].Y - 10, Platforms[0].Width, Platforms[0].Height);
                }
                if (Platforms[0].Y < 0)
                {
                    platformUp = false;
                }
                if (!platformUp)
                {
                    Platforms[0] = new Rectangle(Platforms[0].X, Platforms[0].Y + 10, Platforms[0].Width, Platforms[0].Height);
                }
                if (Platforms[0].Y > 600)
                {
                    platformUp = true;
                }
                if (platformUp)
                {
                    Platforms[1] = new Rectangle(Platforms[1].X, Platforms[1].Y - 10, Platforms[1].Width, Platforms[1].Height);
                }
                if (Platforms[1].Y < 0)
                {
                    platformUp = false;
                }
                if (!platformUp)
                {
                    Platforms[1] = new Rectangle(Platforms[1].X, Platforms[1].Y + 10, Platforms[1].Width, Platforms[1].Height);
                }
                if (Platforms[1].Y > 600)
                {
                    platformUp = true;
                }
            }
            PlayerRect.Y -= upMomentum;

            oldkeys = keys;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            spriteBatch.Draw(Content.Load<Texture2D>("red"), PlayerRect, Color.White);
            for (int i = 0; i < Platforms.Count(); i++)
            {
                spriteBatch.Draw(Content.Load<Texture2D>("brown"), Platforms[i], Color.White);
            }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
