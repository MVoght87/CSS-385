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
    public class Chaser : XNACS1Circle
    {

        private const float mChaserSize = 1.5f;
        private const float mChaserSpeed = 1.1f;
        private Vector2 mSpawnPos = new Vector2(2f, 2f);
        private int mChaserTime;
        private int mChaserCaught;
        private int mChaserEscape;

        private Vector2 mChaseTargetPos;
        private Vector2 mDirToTarget;

        public Chaser()
        {
            Visible = false;
            Radius = mChaserSize;
            Speed = mChaserSpeed;
            Center = mSpawnPos;
        }

        public void Update(Hero mHero)
        {
            if (Visible)
            {
                XNACS1Base.EchoToTopStatus("Chaser Duration: " + mChaserTime);

                ChaseHero(mHero);

                if (mChaserTime < 0 || Collided())
                {
                    mChaserEscape++;
                    Visible = false;
                    Center = mSpawnPos;
                }

                if (Collided(mHero))
                {
                    mChaserCaught++;
                    Visible = false;
                    Center = mSpawnPos;
                }

                mChaserTime--;
            }
            else
            {
                FrontDirection = new Vector2(1f, 1f);
                ShouldTravel = false;
                mChaserTime = 800;
                XNACS1Base.EchoToTopStatus("No Chaser in the World.");
            }

            XNACS1Base.EchoToBottomStatus("Hero Caught: " + mChaserCaught + "   Hero Escaped: " + mChaserEscape);
        }

        public void SummonChaser()
        {
            Visible = true;
        }

        private bool Collided()
        {
            BoundCollideStatus collideStatus = XNACS1Base.World.CollideWorldBound(this);

            switch (collideStatus)
            {
                case BoundCollideStatus.CollideBottom:
                case BoundCollideStatus.CollideTop:
                case BoundCollideStatus.CollideLeft:
                case BoundCollideStatus.CollideRight:
                    return true;
                default:
                    return false;
            }
        }

        private void ChaseHero(Hero mHero)
        {
            ShouldTravel = true;
            mChaseTargetPos = mHero.Center;
            mDirToTarget = mChaseTargetPos - Center;
            mDirToTarget.Normalize();

            double cosTheta = Vector2.Dot(mDirToTarget, FrontDirection);
            float theta = MathHelper.ToDegrees((float)Math.Acos(cosTheta));
            if(theta > 0.001f)
            {
                Vector3 frontDir3 = new Vector3(FrontDirection, 0f);
                Vector3 toTarget3 = new Vector3(mDirToTarget, 0f);
                Vector3 zDir = Vector3.Cross(frontDir3, toTarget3);
                RotateAngle += Math.Sign(zDir.Z) * 0.05f * theta;
                VelocityDirection = FrontDirection;
            }
        }
    }
}
