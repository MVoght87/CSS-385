using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using XNACS1Lib;

namespace ClassExample
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MyBall : XNACS1Circle
    {
        private const float kBallX = ClassExample.kWorldWidth / 2;
        private const float kBallY = ClassExample.kWorldWidth * 9 / 32;
        private const float kMinVelocity = -2f;
        private const float kMaxVelocity = 2f;

        public MyBall() : base(new Vector2(kBallX, kBallY), 3f)
        {
            
            Velocity = new Vector2(XNACS1Base.RandomFloat(kMinVelocity, kMaxVelocity),
                                    XNACS1Base.RandomFloat(kMinVelocity, kMaxVelocity));
            ShouldTravel = true;
            Color = Color.Red;
            RemoveFromAutoDrawSet();
        }

        public void Update()
        {
            if (IsInAutoDrawSet())
            {
                BoundCollideStatus status = XNACS1Base.World.ClampAtWorldBound(this);
                switch (status)
                {
                    case BoundCollideStatus.CollideBottom:
                    case BoundCollideStatus.CollideTop:
                        VelocityY *= -1;
                        break;
                    case BoundCollideStatus.CollideLeft:
                    case BoundCollideStatus.CollideRight:
                        VelocityX *= -1;
                        break;
                }
            }
        }

        public void InitializeNewBall()
        {
            Center = new Vector2(kBallX, kBallY);
            Velocity = new Vector2(XNACS1Base.RandomFloat(kMinVelocity, kMaxVelocity),
                                       XNACS1Base.RandomFloat(kMinVelocity, kMaxVelocity));
            AddToAutoDrawSet();
        }

    }
}