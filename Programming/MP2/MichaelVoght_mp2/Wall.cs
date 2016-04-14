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

using XNACS1Lib;

namespace MichaelVoght_NameSpace
{
    public class Wall : XNACS1Rectangle
    {
        private const float mWallLength = 70f;
        private const float mWallWidth = 3f;

        public Wall()
        {

        }

        public Wall(Vector2 pos)
        {
            SizeX = mWallLength;
            SizeY = mWallWidth;
            Center = pos;
            Color = Color.Red;
        }

        virtual public void Update(Hero hero, Bee bee)
        {
            if(Collided(hero))
            {
                hero.Center -= XNACS1Base.GamePad.ThumbSticks.Left;
            }

        }
    }
}
