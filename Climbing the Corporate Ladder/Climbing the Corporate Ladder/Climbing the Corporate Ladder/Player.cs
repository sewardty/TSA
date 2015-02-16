using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using xTile.Tiles;

namespace Climbing_the_Corporate_Ladder
{
    class Player : Sprite
    {

        public float gravityTimer = 0f;
        public float gravityTimerMax = 10f;
        public float gravity = 25f;
        private ParticleManager particleManager;
        //cooldowns
        int punchCD = 0;
        int kickCD = 0;

        //collsions
        Rectangle myRect;
        int picWide = 212;
        int picHigh = 318;

        //gameStats
        int life;
        double dmg;

        bool isPunching = false;

        public bool win = false;

        public Rectangle WalkableArea = new Rectangle(250, 0, 250, 400);

        public bool onGround = false;

        private ContentManager content;

        //player constructor
        public Player(
           ContentManager content,
           Vector2 location,
           Vector2 velocity,
           ParticleManager particleManager)
            : base(location, velocity)
        {
            // mario, new Rectangle(0, 0, 48, 48)
            this.content = content;
            this.particleManager = particleManager;

            LoadContent();
            //gameStats construction
            myRect = new Rectangle(Convert.ToInt32(location.X), Convert.ToInt32(location.Y), picWide, picHigh);
            life = 10;
            dmg = 10.0;

            currentAnimation = "idle";
        }
        //gameStats accessors
        public bool Punch
        {
            get { return this.isPunching; }
        }
        public Rectangle MyRect
        {
            get { return this.myRect; }
            set { this.myRect = value; }
        }
        public int Life
        {
            get { return this.life; }
            set { this.life = value; }
        }
        public double Dmg
        {
            get { return this.dmg; }
            set { this.dmg = value; }
        }

        public void LoadContent()
        {
            animations.Clear();

            animations.Add("idle",
                new AnimationStrip(
                    content.Load<Texture2D>(@"idle"),
                    "idle",
                    new Rectangle(0, 95, 172, 397),
                    1));

            animations.Add("run",
                              new AnimationStrip(
                                  content.Load<Texture2D>(@"run"),
                                  "run",
                                  new Rectangle(0, 0, 212, 302),
                                  2));
            animations["run"].LoopAnimation = true;
            animations["run"].FrameLength = 0.2f;

            animations.Add("jumpup",
                  new AnimationStrip(
                                  content.Load<Texture2D>(@"jump"),
                                  "jumpup",
                                  new Rectangle(236, 95, 212, 302),
                                  1)
                );
            animations.Add("jumpdown",
                  new AnimationStrip(
                                  content.Load<Texture2D>(@"jump"),
                                  "jumpdown",
                                  new Rectangle(464, 95, 212, 302),
                                  1)
                );
            animations.Add("kick",
                  new AnimationStrip(
                                  content.Load<Texture2D>(@"kick"),
                                  "kick",
                                  new Rectangle(0, 0, 242, 342),
                                  1)
                );
            animations.Add("punch",
                  new AnimationStrip(
                                  content.Load<Texture2D>(@"punch"),
                                  "punch",
                                  new Rectangle(0, 0, 233, 298),
                                  2));
            animations["punch"].LoopAnimation = true;
            animations["punch"].FrameLength = 0.2f;
            animations.Add("crouch",
                  new AnimationStrip(
                                  content.Load<Texture2D>(@"idle"),
                                  "crouch",
                                  new Rectangle(0, 0, 212, 1490),
                                  1));

            animations["idle"].LoopAnimation = true;

            animations.Add("touchdown",
                    new AnimationStrip(
                      content.Load<Texture2D>(@"jump"),
                      "touchdown",
                      new Rectangle(681, 95, 212, 302),
                      1));

            animations["touchdown"].FrameLength = 0.4f;


        }


        public void Jump()
        {
            this.velocity = new Vector2(0, -750);
            currentAnimation = "jumpup";
            onGround = false;
        }

        public void Die()
        {
            currentAnimation = "die";
            onGround = false;
            this.velocity = new Vector2(0, -600);
            this.Dead = true;
        }

        public override void Update(GameTime gameTime)
        {
            
            KeyboardState kb = Keyboard.GetState();

            gravityTimer += (float)gameTime.ElapsedGameTime.Milliseconds;
            if (gravityTimer > gravityTimerMax && !onGround)
            {
                this.velocity.Y += gravity;
                gravityTimer = 0f;
            }

            KeyValuePair<xTile.Tiles.Tile, Vector2> ctest;
            xTile.Tiles.Tile tile;

            /*
            // Check collision below
            KeyValuePair<xTile.Tiles.Tile, Vector2> ctest = CollisionEdgeTest(new Vector2(this.Location.X, this.Location.Y + this.BoundingBoxRect.Height + 1),
                                                      new Vector2(this.Location.X + this.BoundingBoxRect.Width, this.Location.Y + this.BoundingBoxRect.Height + 1)
                                                      );

            xTile.Tiles.Tile tile = ctest.Key;

            if (tile != null && !tile.Properties.ContainsKey("Passable"))
            {
                if (tile.Properties.Keys.Contains("causeDeath"))
                {
                    if (tile.Properties["causeDeath"])
                    {
                        if (!Dead)
                        {
                            Die();
                        }

                    }
                }

                onGround = true;

                if (!Dead)
                {
                    this.velocity.Y = 0;
                    this.location.Y -= (this.location.Y + animations[currentAnimation].FrameHeight + 1) % 48;
                }
            }
            else
                onGround = false;
            */


            if (location.Y + 302 > 650)
            {
                this.velocity.Y = 0;
                this.location.Y = 650 - BoundingBoxRect.Height;
                currentAnimation = "idle";
                onGround = true;
            }

            if (!Dead)
            {
                if (currentAnimation != "touchdown")
                    currentAnimation = "idle";

                if (!onGround)
                {
                    currentAnimation = "jump" + (this.velocity.Y < 0 ? "up" : "down");
                }

                if (kb.IsKeyDown(Keys.W) && onGround)
                {
                    Jump();
                }
                if (kb.IsKeyDown(Keys.K) && kickCD <= 5)
                {
                    currentAnimation = "kick";
                    isPunching = true;
                    kickCD = 1;
                }
                if (kb.IsKeyDown(Keys.P) && punchCD <= 5)
                {
                    currentAnimation = "punch";
                    isPunching = true;
                    punchCD = 1;

                    
                }
                //cd mechanism
                if (punchCD > 0)
                    punchCD++;

                if (kickCD > 0)
                    kickCD++;

                if (kickCD >= 20)
                {
                    kickCD = 0;
                    isPunching = false;
                }

                if (punchCD >= 20)
                {
                    punchCD = 0;
                    isPunching = false;
                }


                if (kb.IsKeyDown(Keys.S))
                {
                    currentAnimation = "crouch";
                    
                }

                if (kb.IsKeyDown(Keys.A))
                {
                    if (onGround) currentAnimation = "run";
                    this.FlipHorizontal = true;

                    if (this.location.X > (World.viewport.X + WalkableArea.Left))
                    {
                        this.location.X -= 5;
                    }
                    else if (World.viewport.X > 0)
                    {
                        World.viewport.X -= 5;
                        this.location.X -= 5;
                    }

                }

                if (kb.IsKeyDown(Keys.D))
                {
                    if (onGround) currentAnimation = "run";
                    this.FlipHorizontal = false;

                    this.location.X += 5;

                    if (this.location.X > (World.viewport.X + WalkableArea.Right))
                    {

                        World.viewport.X += 5;
                    }

                }
                if (location.X > 1200)
                {
                    win = true;
                }
                
                







            }

            base.Update(gameTime);
        }
        
    }
}
