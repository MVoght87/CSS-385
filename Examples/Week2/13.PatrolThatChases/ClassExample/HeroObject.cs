using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using XNACS1Lib;

namespace ClassExample
{
    class HeroObject : XNACS1Circle
    {
        int mCaught = 0;

        public HeroObject()
        {
            Radius = 2f;
            CenterX = XNACS1Base.RandomFloat(XNACS1Base.World.WorldMin.X, XNACS1Base.World.WorldMax.X);
            CenterY = XNACS1Base.RandomFloat(XNACS1Base.World.WorldMin.Y, XNACS1Base.World.WorldMax.Y);
            Color = new Color(1f, 0.8f, 0.8f);
        }

        public void Update(Vector2 delta)
        {
            Center += delta;
            XNACS1Base.World.ClampAtWorldBound(this);
        }

        public void Caught() { mCaught++; }
        public int NumTimesCaught { get { return mCaught; } }
    }
}
