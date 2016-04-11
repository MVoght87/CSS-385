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
        private XNACS1Rectangle mShowV;
        private float kDrawFactor = 30f;
        private float kDrawWidth = 0.2f;

        
        public MySoccer()
        {
            Texture = "SoccerBall";
            Radius = 3f;

            ShouldTravel = true;
            Speed = 0.2f;

            mShowV = new XNACS1Rectangle(Center, Center + kDrawFactor * Velocity, kDrawWidth, null);
            mShowV.Color = Color.Blue;

        }

        public void Update(XNACS1Primitive target, float turnRate)
        {
            if (!Collided(target))
            {
                Vector2 targetDir = target.Center - Center;
                targetDir.Normalize();
                float theta = MathHelper.ToDegrees((float)Math.Acos(
                    (double)(Vector2.Dot(FrontDirection, targetDir))));

                if (theta > 0.001f)
                { // not quite aligned ...
                    Vector3 fIn3D = new Vector3(FrontDirection, 0f);
                    Vector3 tIn3D = new Vector3(targetDir, 0f);
                    Vector3 sign = Vector3.Cross(fIn3D, tIn3D);

                    RotateAngle += Math.Sign(sign.Z) * theta * turnRate;
                    VelocityDirection = FrontDirection;
                }

                mShowV.SetEndPoints(Center, Center + kDrawFactor * Velocity, kDrawWidth);
                mShowV.Color = Color.Blue;
            }
        }
    }
}
