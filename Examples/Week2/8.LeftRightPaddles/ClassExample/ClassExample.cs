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
    public class ClassExample : XNACS1Base
    {

        public const float kWorldWidth = 100f;
        public const float kWorldHeight = kWorldWidth * 9f / 16f;

        private const int kNumPaddlesPerSide = 2;

        private const float kYDist = kWorldHeight / ((kNumPaddlesPerSide) + 1);
        private const float kLeftPosition = 25f;
        private const float kRightPosition = 75f;

        private Paddle[] mLeftPaddles, mRightPaddles;
        private MyBall mBall;
        private int mHit, mMissed;

        protected override void InitializeWorld()
        {
            World.SetWorldCoordinate(new Vector2(0, 0), kWorldWidth);
            mHit = mMissed = 0;

            mLeftPaddles = new Paddle[kNumPaddlesPerSide];
            mRightPaddles = new Paddle[kNumPaddlesPerSide];
            float y = kYDist;
            int i = 0;
            while (i < kNumPaddlesPerSide)
            {
                // allocate the left/right paddles
                mLeftPaddles[i] = new LeftPaddle(new Vector2(kLeftPosition, y));
                mRightPaddles[i] = new RightPaddle(new Vector2(kRightPosition, y));

                i++;
                y += kYDist;
            }
            mBall = new MyBall();
        }

        protected override void UpdateWorld()
        {
            if (GamePad.ButtonBackClicked())
                this.Exit();

            if (GamePad.ButtonAClicked())
            {
                if (mBall.IsInAutoDrawSet())
                    mMissed++;

                mBall.InitializeNewBall();
            }

            if (GamePad.ButtonBClicked())
            {
                mBall.RemoveFromAutoDrawSet();
                mMissed++;
            }


            float userLeft = GamePad.Triggers.Left;
            float userRight = GamePad.Triggers.Right;

            for (int i =0; i<kNumPaddlesPerSide; i++)
            {
                if ((mLeftPaddles[i].Update(userLeft, mBall)) ||
                     (mRightPaddles[i].Update(userRight, mBall)))
                    mHit++;
            }

            if (mBall.IsInAutoDrawSet())
            {
                mBall.Update();
                EchoToBottomStatus("Ball position: " + mBall.Center);
            }
            else
                EchoToBottomStatus("No Ball in the world");

            EchoToTopStatus("Hit: " + mHit + "   Missed:" + mMissed);
        }
    }
}