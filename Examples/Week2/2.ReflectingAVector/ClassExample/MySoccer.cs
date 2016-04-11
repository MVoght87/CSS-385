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
        private MyBlock mTargetBlock;
        private List<VectorComponents> mShowVecs = null;
        private List<XNACS1Primitive> mReflection = null;

        private float kDrawSize = 8f;

        public MySoccer(MyBlock b)
        {
            Texture = "SoccerBall";
            Radius = 4f;

            ShouldTravel = false;
            mTargetBlock = b;

            mShowVecs = new List<VectorComponents>();
            mReflection = new List<XNACS1Primitive>();
        }

        public void ShootSoccer(Vector2 initPos, Vector2 velocity)
        {
            ResetBallPosition();

            Velocity = velocity;
            Center = initPos;
            ShouldTravel = true;

            // this is not efficient but, for showing this will do ...
            mShowVecs = new List<VectorComponents>();
            mReflection = new List<XNACS1Primitive>();
        }

        public void Update()
        {
            if (Collided(mTargetBlock))
            {
                VectorComponents currentV = new VectorComponents();
                currentV.Update(Center, Center + Velocity, mTargetBlock.FrontDirection);

                Vector2 newVelocity = currentV.TangentVector() - currentV.NormalVector();
                XNACS1Rectangle reflectV = new XNACS1Rectangle();
                reflectV.SetEndPoints(Center, Center + (newVelocity * kDrawSize), 0.3f);
                reflectV.Color = Color.DeepPink;

                mShowVecs.Add(currentV);
                mReflection.Add(reflectV);

                Velocity = newVelocity;
            }
            else
            {
                XNACS1Lib.BoundCollideStatus status = XNACS1Base.World.ClampAtWorldBound(this);
                switch (status)
                {
                    case BoundCollideStatus.CollideBottom:
                    case BoundCollideStatus.CollideTop:
                        VelocityY = -VelocityY;
                        break;

                    case BoundCollideStatus.CollideLeft:
                    case BoundCollideStatus.CollideRight:
                        VelocityX = -VelocityX;
                        break;
                }
            }
        }

        public void ResetBallPosition()
        {
            Center = ClassExample.kInitPosition;
            foreach (XNACS1Primitive sv in mReflection)
                sv.RemoveFromAutoDrawSet();

            foreach (VectorComponents sv in mShowVecs)
                sv.HideVectorComponents();

            mReflection = new List<XNACS1Primitive>();
            mShowVecs = new List<VectorComponents>();
        }
    }
}
