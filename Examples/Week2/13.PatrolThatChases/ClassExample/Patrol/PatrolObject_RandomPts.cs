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
    partial class PatrolObject : XNACS1Circle
    {
        #region compute random position in one of the 4 regions
        private Vector2 GetRandomWorldPosition()
        {
            float x = XNACS1Base.World.WorldDimension.X * XNACS1Base.RandomFloat(0.02f, 0.48f);
            float y = XNACS1Base.World.WorldDimension.Y * XNACS1Base.RandomFloat(0.02f, 0.48f);
            return new Vector2(x, y);
        }
        
        private Vector2 RandomBottomRightPosition()
        {
            Vector2 p = GetRandomWorldPosition();
            return new Vector2(XNACS1Base.World.WorldMax.X - p.X, XNACS1Base.World.WorldMin.Y + p.Y);
        }

        private Vector2 RandomBottomLeftPosition()
        {
            return XNACS1Base.World.WorldMin + GetRandomWorldPosition();
        }

        private Vector2 RandomTopRightPosition()
        {
            return XNACS1Base.World.WorldMax - GetRandomWorldPosition();
        }

        private Vector2 RandomTopLeftPosition()
        {
            Vector2 p = GetRandomWorldPosition();
            return new Vector2(XNACS1Base.World.WorldMin.X + p.X, XNACS1Base.World.WorldMax.Y - p.Y);
        }
        #endregion 
    }
}
