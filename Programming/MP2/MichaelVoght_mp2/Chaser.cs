/*
 * Author:              Michael Voght
 * Date:                April 14th, 2016
 * Content Mentions:    - Bug: http://opengameart.org/content/parts-2-art-spider
 *                      - Smart Walls: http://opengameart.org/content/32-x-32-bricks
 *                      - Background: http://opengameart.org/content/country-side-platform-tiles
 *                      - Wall: http://opengameart.org/content/wall-0
 *                      - Chaser: http://opengameart.org/content/ufo-enemy-game-character
 *                      - Hero: http://opengameart.org/content/sorcerer
 *                      - Reformat Kevin MacLeod (incompetech.com)
 *                        Licensed under Creative Commons: By Attribution 3.0 License
 *                        http://creativecommons.org/licenses/by/3.0/
 * Chaser.cs:           Inherits from circle. When summoned, it starts a timer and chases the player
 *                      until the timer runs out or goes beyond the bounds of the screen. If the player
 *                      escapes then the escaped tally goes up, if he's caught then the caught tally
 *                      goes up.
 */

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
        #region Instance Variables for the Chaser
        private const float mChaserSize = 1.5f;
        private const float mChaserSpeed = 1.1f;
        private Vector2 mSpawnPos = new Vector2(2f, 2f);
        private int mChaserTime;
        private int mChaserCaught;
        private int mChaserEscape;

        private Vector2 mChaseTargetPos;
        private Vector2 mDirToTarget;
        #endregion

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
                #region Chaser Logic when active
                XNACS1Base.EchoToTopStatus("Chaser Duration: " + mChaserTime);

                ChaseHero(mHero);

                if (mChaserTime < 0 || Collided())
                {
                    XNACS1Base.PlayACue("multiball");
                    mChaserEscape++;
                    Visible = false;
                    Center = mSpawnPos;
                }

                if (Collided(mHero))
                {
                    XNACS1Base.PlayACue("loseball");
                    mChaserCaught++;
                    Visible = false;
                    Center = mSpawnPos;
                }

                mChaserTime--;
                #endregion
            }
            else
            {
                #region reset value to initial position
                FrontDirection = new Vector2(1f, 1f);
                ShouldTravel = false;
                mChaserTime = 800;
                XNACS1Base.EchoToTopStatus("No Chaser in the World.");
                #endregion
            }

            XNACS1Base.EchoToBottomStatus("Hero Caught: " + mChaserCaught + "   Hero Escaped: " + mChaserEscape);
        }

        public void SummonChaser()
        {
            XNACS1Base.PlayACue("powerup");
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
