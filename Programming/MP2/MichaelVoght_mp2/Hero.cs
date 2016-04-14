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
    public class Hero : XNACS1Circle
    {
        private const float kHeroSize = 3f;
        private Vector2 mHeroPosition = new Vector2(5.0f, 5.0f);

        public Hero()
        {
            Radius = kHeroSize;
            ShouldTravel = false;

            Center = mHeroPosition;
        }

        public void Update()
        {
            Center += XNACS1Base.GamePad.ThumbSticks.Left;
            XNACS1Base.World.ClampAtWorldBound(this);
        }
    }
}
