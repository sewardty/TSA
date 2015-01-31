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
        double life;
        double dmg;
        float move;
        
        public void TheEnemy()
        {
            life = 100.0;
            dmg = 12.5;
            move = 10f;
            
        }
        

    }
}
