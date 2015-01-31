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

// xTile engine namespaces
using xTile;
using xTile.Display;

namespace Climbing_the_Corporate_Ladder
{
        // declares variables
    enum GameStates
    {
        Menu,
        Help,
        Game
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;



        GameStates gameState;
        KeyboardState kbd, prevKbd;
        Color background;
        Rectangle screen;
        Texture2D menuTexture, helpTexture, gameTexture;


        // xTile map, display device reference, and rendering viewpoer
        Map map;
        IDisplayDevice mapDisplayDevice;
        xTile.Dimensions.Rectangle viewport;

        bool isFullScreen = false;
        bool requestFullScreen = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.IsFullScreen = isFullScreen;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            // initializes variables
            screen = new Rectangle(0,0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
            gameState = GameStates.Menu;
            background = Color.White;


            // initialise xTile map display device
            mapDisplayDevice = new XnaDisplayDevice(
                this.Content, this.GraphicsDevice);

            // initialise xTile rendering viewport (hardcoded for now)
            viewport = new xTile.Dimensions.Rectangle(new xTile.Dimensions.Size(1024, 768));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
  
            // initializes images
            menuTexture = Content.Load<Texture2D>("Menu");
            helpTexture = Content.Load<Texture2D>("Help");
            gameTexture = Content.Load<Texture2D>("Game");
   
            //load xTile map from content pipeline
            map = Content.Load<Map>("Maps\\Level1");

            map.LoadTileSheets(mapDisplayDevice);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {

            kbd = Keyboard.GetState();

            // sets fullscreen when tilde is pressed
            if (kbd.IsKeyDown(Keys.OemTilde))
                requestFullScreen = true;

            if (kbd.IsKeyUp(Keys.OemTilde) && requestFullScreen)
            {
                requestFullScreen = false;
                isFullScreen = !isFullScreen;
                graphics.IsFullScreen = isFullScreen;
                graphics.ApplyChanges();
            }

            // Updates the game state
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

                // update xTile map for animations etc.
                // and update viewport for camera movement
                map.Update(gameTime.ElapsedGameTime.Milliseconds);
                viewport.X++;
            }

            prevKbd = kbd;




            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            // updates the screen based on the current game state
            switch (gameState)
            {
                case GameStates.Game:
                    // render xTile map
                    map.Draw(mapDisplayDevice, viewport);
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
