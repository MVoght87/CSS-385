#define UseXNACS1Lib    // how to compute velocity direction: 
                        //      use the library functionality or otherwise


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
        }

        /// <summary>
        /// Assign a new completely random direction for the ball velocity without changing the speed!
        /// </summary>
        private void SetRandomBallVelocityDir()
        {
            mCurrentDir.X = RandomFloat(-1f, 1f);
            mCurrentDir.Y = RandomFloat(-1f, 1f);
            mCurrentDir.Normalize();

#if UseXNACS1Lib
            // With XNA CS1 Library, this is what we can do:
            mBall.Speed = mCurrentSpeed;
            mBall.VelocityDirection = mCurrentDir;
#else
            // Alternatively, this is what the actual math requires
            mBall.Velocity = mCurrentDir * mCurrentSpeed;
            
#endif

        }
    }
}
