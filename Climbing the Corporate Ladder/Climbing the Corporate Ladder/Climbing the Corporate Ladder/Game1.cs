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
        KeyboardState kbd, prevKbd;
        Color background;
        Rectangle screen;
        Texture2D menuTexture, helpTexture, gameTexture;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // initializes my variables -Connor
            screen = new Rectangle(0,0, 800, 500);
            gameState = GameStates.Menu;
            background = Color.White;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // initializes images -Connor
            menuTexture = Content.Load<Texture2D>("Menu");
            helpTexture = Content.Load<Texture2D>("Help");
            gameTexture = Content.Load<Texture2D>("Game");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            kbd = Keyboard.GetState();

            // Updates the game state -Connor
            if (gameState == GameStates.Menu)
            {
                if (kbd.IsKeyDown(Keys.Enter) && prevKbd.IsKeyUp(Keys.Enter))
                {
                    gameState = GameStates.Help;
                }
            }
            else if (gameState == GameStates.Help)
            {
                if (kbd.IsKeyDown(Keys.Enter) && prevKbd.IsKeyUp(Keys.Enter))
                {
                    gameState = GameStates.Game;
                }
            }
            else if (gameState == GameStates.Game)
            {
                if (kbd.IsKeyDown(Keys.Escape) && prevKbd.IsKeyUp(Keys.Escape))
                {
                    gameState = GameStates.Menu;
                }
            }

            prevKbd = kbd;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            // updates the screen based on the current game state -Connor
            switch (gameState)
            {
                case GameStates.Game:
                    spriteBatch.Draw(gameTexture, screen, background);
                    break;

                case GameStates.Help:
                    spriteBatch.Draw(helpTexture, screen, background);
                    break;

                case GameStates.Menu: 
                    spriteBatch.Draw(menuTexture, screen, background);
                    break;
            }
           
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
