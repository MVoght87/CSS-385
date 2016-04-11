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

        private PaddleSet mLeftPaddles, mRightPaddles;
        private MyBall mBall;
        private int mHit, mMissed;

        protected override void InitializeWorld()
        {
            World.SetWorldCoordinate(new Vector2(0, 0), kWorldWidth);
            mHit = mMissed = 0;

            mLeftPaddles = new PaddleSet(PaddleSet.PaddleSetType.eLeftSet, 
                                            3,      // number of paddles 
                                            15,     // x position of the paddles
                                            10,     // initial y position
                                            15);    // y distance between the paddles

            mRightPaddles = new PaddleSet(PaddleSet.PaddleSetType.eRightSet, 
                                            4,      // number of paddles
                                            70,     // x position of the paddles
                                            15,     // initial y positions
                                            8);    // y distance between the paddles
            
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

            if ((mLeftPaddles.UpdatePaddles(userLeft, mBall)) ||
                (mRightPaddles.UpdatePaddles(userRight, mBall)))
                    mHit++;

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