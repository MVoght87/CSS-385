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
    /// 
    /// </summary>
    public class MySoccer : XNACS1Rectangle
    {
        private MyRoad mRoad;
        private int mCurrentRoadSegment = 0;

        // Use number of ticks to determine state transition
        private float mNumTicksLeft = 0f;

        public MySoccer(Vector2 pos, MyRoad road)
        {
            Texture = "SoccerBall";
            Width = 4f;
            Height = 4f;

            Center = pos;
            ShouldTravel = true;
            Speed = 0.1f;

            mRoad = road;
        }

        public bool Update(float ticksToTravel)
        {
            // continuously adjust the ball's forward direction and speed
            bool newState = false;

            /// distinct states to be concerned with
            /// 
            ///     1. Has a valide next state (road segment)
            ///         1a. continues to be valid (within the bounds of road segment)
            ///         1b. if not, trasition to next state
            ///     2. No more valid next state: reset
            ///     
            if (mCurrentRoadSegment < mRoad.NumDefinedSegments())
            {
                mNumTicksLeft--;
                if (mNumTicksLeft < 0f)
                {
                    mCurrentRoadSegment++;

                    newState = true;
                    if (mCurrentRoadSegment >= mRoad.NumDefinedSegments())
                        ResetBallPosition();
                }
            }
            else
            {
                ResetBallPosition();
                newState = true;
            }

// **** set dir and velocity only during state trasition ***
// CONSEQUENCE?: Last state/setment (under user control) may be
//                 wrong if user changes the state (Road Size/Dir)
//                 while we are in the segment
            if ( newState
                 || (mCurrentRoadSegment == (mRoad.NumDefinedSegments() - 1))
                )
                UpdateBallDirAndSpeed(ticksToTravel);
// SOLUTION? Call UpdateBallDirAndSpeed() during _EVERY_ Update
//                 for LAST SEGMENT only.
            
            return newState;
        }

        public void ResetBallPosition()
        {
            Center = ClassExample.kInitPosition;
            mCurrentRoadSegment = 0;
            RotateAngle = 0f;
        }

        private void UpdateBallDirAndSpeed(float ticksToTravel)
        {
            if (mNumTicksLeft <= 0) // beginning of a new road
                mNumTicksLeft = ticksToTravel;

            // use num ticks to determine how far we have travelled
            float percentTravelled = (ticksToTravel - mNumTicksLeft) / ticksToTravel;

            // ball's center position should be percentTravelled along the road's entire length
            float roadLength = mRoad.RoadSegmentLength(mCurrentRoadSegment);
            Vector2 v = mRoad.RoadSegmentDir(mCurrentRoadSegment);
            Vector2 startPos = mRoad.RoadSegmentStartPos(mCurrentRoadSegment);
            Center = startPos + percentTravelled * roadLength * v;
            
            VelocityDirection = v;

            // speed will be a function of what is remaining length and remaining ticks left.
            float totalLength = mRoad.RoadSegmentLength(mCurrentRoadSegment);
            float travelled = (Center - startPos).Length();
            float remainLength = totalLength - travelled;

            Speed = remainLength/ mNumTicksLeft;
        }
    }
}
