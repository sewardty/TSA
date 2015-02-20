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
using System.Collections;

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
        Game,
        Game2,
        Game3,
        Game4
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameStates gameState;
        KeyboardState kbd, prevKbd;
        Color background;
        Rectangle screen;
        Texture2D menuTexture, helpTexture, copier;

        ArrayList enemies = new ArrayList();
        Texture2D pic;
        int timer;
        int interval = 0;

        // xTile map, display device reference, and rendering viewpoer
        Map map;
        IDisplayDevice mapDisplayDevice;
        xTile.Dimensions.Rectangle viewport;

        ParticleManager particleManager;
        Player player;

        bool isFullScreen = false;
        bool requestFullScreen = false;

        //collsions
        bool isPunch;
        Rectangle rekt;
        Rectangle rect;

        int max = 10;
        int count = 0;
        int killed = 0;

        Rectangle penHitBox;
        Texture2D penImage;
        Vector2 startLocation;
        bool penVisible = false;
        int penCD;
        int penRange = 500;
        float speed;

        Texture2D heart;
        SpriteFont spriteFont;

        int lifeScore = 0;
        int killScore = 0;
        int timeScore = 0;
        int finalScore = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;
            graphics.IsFullScreen = isFullScreen;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            // initializes variables
            screen = new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
            gameState = GameStates.Menu;
            background = Color.White;

            // initialise xTile map display device
            mapDisplayDevice = new XnaDisplayDevice(
                this.Content, this.GraphicsDevice);

            // initialise xTile rendering viewport (hardcoded for now)
            World.viewport = new xTile.Dimensions.Rectangle(new xTile.Dimensions.Size(1366, 768));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // initializes images
            menuTexture = Content.Load<Texture2D>("Menu");
            helpTexture = Content.Load<Texture2D>("Help");
            copier = Content.Load<Texture2D>("Copier");


            //load xTile map from content pipeline
            World.map = Content.Load<Map>("Maps\\Level1");

            World.map.LoadTileSheets(mapDisplayDevice);

            particleManager = new ParticleManager();
            player = new Player(this.Content, new Vector2(350, this.Window.ClientBounds.Height - (48 * 2) - 500), Vector2.Zero, particleManager);

            pic = Content.Load<Texture2D>("theBall");
            penImage = Content.Load<Texture2D>("black-pen");

            heart = Content.Load<Texture2D>("heart");

            spriteFont = Content.Load<SpriteFont>("Arial");
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




                player.Update(gameTime);

                // update xTile map for animations etc.
                // and update viewport for camera movement
                World.map.Update(gameTime.ElapsedGameTime.Milliseconds);
                viewport.X++;
            }

            prevKbd = kbd;



            if (gameState == GameStates.Game)
            {
                timer = gameTime.TotalGameTime.Seconds;
                if (timer <= 60)
                {

                    //pen ability mechansism
                    if (timer > penCD)
                    {

                        if (kbd.IsKeyDown(Keys.R))
                        {
                            startLocation = player.Location;
                            Pen pen = new Pen(penImage, player.Location);
                            penVisible = true;
                            penHitBox = pen.PenBox;
                            speed = pen.Speed;
                            penCD += 2;

                        }
                    }
                    if (penHitBox.X <= player.Location.X + penRange && (penVisible))
                    {
                        penHitBox.X += Convert.ToInt32(speed);
                    }
                }
                //enemy mechanism
                if (timer >= interval)
                {
                    if (count <= max)
                    {
                        enemies.Add(new Enemy(pic));
                        count++;
                    }
                    //spawn = true;
                    interval += 5;
                }
                int countIndex = -1;
                int deadIndex = -1;
                foreach (Object obj in enemies)
                {
                    Enemy unit = (Enemy)obj;

                    unit.Update();
                    isPunch = player.Punch;
                    rekt = player.MyRect;
                    rect = unit.ARect;
                    countIndex++;

                    if (rekt.Intersects(rect) && isPunch == false)
                    {
                        player.Life -= 1;
                        deadIndex = countIndex;

                    }
                    if ((penVisible) && penHitBox.Intersects(rect))
                    {
                        penVisible = false;
                        deadIndex = countIndex;
                        penHitBox = new Rectangle(0, 0, 1, 1);
                    }
                }
                if (deadIndex != -1)
                {
                    enemies.RemoveAt(deadIndex);
                    killed++;
                }
                if (player.Life <= 0.0)
                {
                    //just a test, you can edit the game to exit
                    gameState = GameStates.Menu;
                    player.Life = 10;

                }
            }
            else
                gameState = GameStates.Menu;

            lifeScore = player.Life;
            timeScore = 60 - timer;
            killScore = killed;
            finalScore = lifeScore + timeScore + killScore;


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
                    World.map.Draw(mapDisplayDevice, World.viewport);
                    player.Draw(spriteBatch);
                    break;

                case GameStates.Help:
                    spriteBatch.Draw(helpTexture, screen, background);
                    break;

                case GameStates.Menu:
                    spriteBatch.Draw(menuTexture, screen, background);
                    break;
            }
            // spriteBatch.Draw(orderPic, )
            foreach (Object obj in enemies)
            {
                Enemy unit = (Enemy)obj;
                spriteBatch.Draw(unit.EnemyPic, unit.Pos, Color.White);

            }
            if (penVisible)
            {
                spriteBatch.Draw(penImage, penHitBox, Color.White);
            }

            if (player.Life > 0 && gameState == GameStates.Game)
            {
                spriteBatch.Draw(heart, new Vector2(10, 700), Color.White);

                if (player.Life > 1)
                {
                    spriteBatch.Draw(heart, new Vector2(100, 700), Color.White);

                    if (player.Life > 2)
                    {
                        spriteBatch.Draw(heart, new Vector2(190, 700), Color.White);

                        if (player.Life > 3)
                        {
                            spriteBatch.Draw(heart, new Vector2(280, 700), Color.White);

                            if (player.Life > 4)
                            {
                                spriteBatch.Draw(heart, new Vector2(370, 700), Color.White);
                                if (player.Life > 5)
                                {
                                    spriteBatch.Draw(heart, new Vector2(460, 700), Color.White);
                                    if (player.Life > 6)
                                    {
                                        spriteBatch.Draw(heart, new Vector2(550, 700), Color.White);
                                        if (player.Life > 7)
                                        {
                                            spriteBatch.Draw(heart, new Vector2(640, 700), Color.White);
                                            if (player.Life > 8)
                                            {
                                                spriteBatch.Draw(heart, new Vector2(730, 700), Color.White);
                                                if (player.Life > 9)
                                                {
                                                    spriteBatch.Draw(heart, new Vector2(820, 700), Color.White);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            spriteBatch.DrawString(spriteFont, "Score : " + finalScore, new Vector2(600, 50), Color.White);
            spriteBatch.DrawString(spriteFont, "player x : "+player.MyRect.X+" local "+World.viewport.X+" play loc x "+ player.Location.X+" pen x = "+penHitBox.X, new Vector2(600, 150), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);

           
        }
    }
}

