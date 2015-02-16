using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Climbing_the_Corporate_Ladder
{
    class Enemy
    {
        float speed;
        Vector2 pos;
        Texture2D enemyPic;
        Rectangle aRect;
        int picWide = 60;
        int picHigh = 60;
        
        

        //enemy constructorino
        public Enemy(Texture2D pic)
        {
            
            speed = 10f;
            pos = new Vector2(2400, 400);
            
            enemyPic = pic;
            aRect = new Rectangle(Convert.ToInt32(pos.X), Convert.ToInt32(pos.Y), picWide, picHigh);
        }
        public float Speed
        {
            get { return this.speed; }
            set { this.speed = value; }
        }
        public Texture2D EnemyPic
        {
            get { return this.enemyPic; }
            set { this.enemyPic = value; }
        }
        public Vector2 Pos
        {
            get { return this.pos; }
            set { this.pos = value; }
        }
        public void Update()
        {
            pos.X -= speed;
            aRect.X = Convert.ToInt32(pos.X);
            
        }
        public Rectangle ARect
        {
            get { return this.aRect; }
            set { this.aRect = value; }
        }
    }
}
