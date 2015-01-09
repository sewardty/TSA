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

namespace Climbing_the_Corporate_Ladder
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // declares my variables - Connor
        enum GameStates
        {
            Menu,
            Help,
            Game
        }
        GameStates gameState;
        SpriteFont font;
        KeyboardState kbd, prevKbd;
        Color background;
        String gameStateTitle;
        Rectangle screen;
        Texture2D screenTexture;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // initializes my variables -Connor
            screen = new Rectangle(50,50, 700, 380);
            gameState = GameStates.Menu;
            gameStateTitle = "Menu";
            background = Color.Red;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Font");
            screenTexture = Content.Load<Texture2D>("Background");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            kbd = Keyboard.GetState();

            // Updates the game state -Connor

            // if the game is in the menu state,
            // it will move to the game when enter is pressed
            // or to help if "H" is pressed -Connor
            if (gameState == GameStates.Menu)
            {
                if (kbd.IsKeyDown(Keys.Enter) && prevKbd.IsKeyUp(Keys.Enter))
                {
                    gameState = GameStates.Game;
                    background = Color.Green;
                    gameStateTitle = "Game";
                }
                if (kbd.IsKeyDown(Keys.H) && prevKbd.IsKeyUp(Keys.H))
                {
                    gameState = GameStates.Help;
                    background = Color.Orange;
                    gameStateTitle = "Help";
                }
            }

            // if the game is in the game state,
            // it will move to the menu when escape is pressed -Connor
            if (gameState == GameStates.Game)
            {
                if (kbd.IsKeyDown(Keys.Escape) && prevKbd.IsKeyUp(Keys.Escape))
                {
                    gameState = GameStates.Menu;
                    background = Color.Red;
                    gameStateTitle = "Menu";
                }
            }

            // if the game is in the help state,
            // it will move to the menu when enter is pressed -Connor
            if (gameState == GameStates.Help)
            {
                if (kbd.IsKeyDown(Keys.Enter) && prevKbd.IsKeyUp(Keys.Enter))
                {
                    gameState = GameStates.Menu;
                    background = Color.Red;
                    gameStateTitle = "Menu";
                }
            }
            prevKbd = kbd;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(screenTexture, screen, background);
            spriteBatch.DrawString(font, gameStateTitle, Vector2.Zero, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
