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
    public class MySoccer : XNACS1Circle
    {

        public MySoccer()
        {
            Texture = "SoccerBall";
            Radius = 4f;

            ShouldTravel = false;

        }

        public void UpdateSoccerPosition(Vector2 delta)
        {
            Center += delta;
            XNACS1Base.World.ClampAtWorldBound(this);
        }
    }
}
