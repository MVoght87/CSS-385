// #define UseXNACS1Lib     // uses the XNACS1Lib functionality to set current Ball Speed, 
                            // notice that as ball speed approches zero, velocity is really undefined
                            // and thus the motion of the ball starts to become random!!

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
        XNACS1Circle mBall;
        float mCurrentSpeed;
        Vector2 mCurrentDir; 
        /// <summary>
        /// You should initialize/allocate instance variables here!
        /// </summary>
        protected override void InitializeWorld()
        {
            World.SetWorldCoordinate(new Vector2(0, 0), 100);

            mBall = new XNACS1Circle(new Vector2(20f, 20f), 1);
            mBall.Texture = "SoccerBall";
            mBall.ShouldTravel = true;

            mCurrentSpeed = 0.1f;
            mCurrentDir = new Vector2();

            SetRandomBallVelocityDir();
        }

        /// <summary>
        /// You should update the state of your world here!
        /// </summary>
        protected override void UpdateWorld()
        {
            if (GamePad.ButtonBackClicked())
                Exit();

            if (GamePad.ButtonAClicked())
                SetRandomBallVelocityDir();

            UpdateBallSpeed(GamePad.ThumbSticks.Right.Y * 0.01f);
            
        }

        /// <summary>
        /// Change the ball speed without changing 
        /// the travelling direction
        /// </summary>
        /// <param name="delta">Amount to change</param>
        private void UpdateBallSpeed(float delta)
        {
            // This is what should happen:
            // 1. update the speed
            mCurrentSpeed += delta;

#if UseXNACS1Lib
            // 2. set the new speed into the ball velocity with existing 
            //      velocity direction
            mBall.Speed = mCurrentSpeed;
                // This DOES NOT WORK!! As when Speed approaches zero, velocity approaches zero
                // The Ball does not know its current velocity direction any more!!
#else
            // alternatively, we can figure out what is the current velocity direction ...
            mBall.Velocity = mCurrentDir * mCurrentSpeed;
#endif

        }

        /// <summary>
        /// Assign a new completely random direction for 
        /// the ball velocity without changing the speed!
        /// </summary>
        private void SetRandomBallVelocityDir()
        {
            mCurrentDir.X = RandomFloat(-1f, 1f);
            mCurrentDir.Y = RandomFloat(-1f, 1f);
            mCurrentDir.Normalize();


            mBall.Velocity = mCurrentDir * mCurrentSpeed;

            // Alternatively, you can also do:
            //     mBall.Speed = mCurrentSpeed;
            //     mBall.VelocityDirection = mCurrentDir;
        }
    }
}
