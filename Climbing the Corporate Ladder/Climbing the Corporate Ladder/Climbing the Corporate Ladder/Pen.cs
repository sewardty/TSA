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
    class Pen
    {
        Rectangle penBox;
        float speed;
        int penHigh = 128;
        int penWide = 128;
        Texture2D penPic;
        int range;

        public Pen(Texture2D pic, Vector2 pos)
        {
            
            //penBox = new Rectangle(Convert.ToInt32(pos.X), Convert.ToInt32(pos.Y), penWide, penHigh);
            penBox = new Rectangle(350 + (Convert.ToInt32(pos.X - World.viewport.X) - 250), Convert.ToInt32(pos.Y), penWide, penHigh);
            speed = 12f;
            penPic = pic;
            range = 500;
        }
        public Rectangle PenBox
        {
            get { return this.penBox; }
            set { this.penBox = value; }
        }
        public Texture2D PenPic
        {
            get { return this.penPic; }
        }
        public float Speed
        {
            get { return this.speed; }
            set { this.speed = value; }
        }
        public int PenHigh
        {
            get { return this.penHigh; }
        }
        public int PenWide
        {
            get { return this.penWide; }
        }
        public int Range
        {
            get { return this.range; }
        }
        
    }
}
