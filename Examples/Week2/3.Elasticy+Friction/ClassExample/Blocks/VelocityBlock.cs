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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using XNACS1Lib;

namespace ClassExample
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class VelocityBlock : XNACS1Rectangle
    {
        private const float kDirSize = 0.5f;
        private const float kDirDrawFactor = 4f;

        Vector2 mInitPos;
        Vector2 mDir;

        public VelocityBlock(Vector2 initPos, Vector2 dir) 
        {
            mInitPos = initPos;
            mDir = dir;
        }

        public void UpdateVelocityBlock(Vector2 delta)
        {
            mDir += delta;

            Vector2 endPos = mInitPos + (mDir * kDirDrawFactor);
            SetEndPoints(mInitPos, endPos, kDirSize);

            Velocity = mDir;
        }
    }
}
