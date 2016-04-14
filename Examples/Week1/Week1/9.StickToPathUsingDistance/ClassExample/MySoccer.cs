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
                Vector2 travelled = Center - mRoad.RoadSegmentStartPos(mCurrentRoadSegment);
                if (travelled.Length() >= mRoad.RoadSegmentLength(mCurrentRoadSegment))
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
            // Figure out how much we have travelled
            Vector2 startPos = mRoad.RoadSegmentStartPos(mCurrentRoadSegment);
            Vector2 dir = startPos - Center;
            float travelled = dir.Length();
 
            // Figure the current direction of the road
            Vector2 v = mRoad.RoadSegmentDir(mCurrentRoadSegment);

            // Our ball should be travelled along the road direction
            Center = startPos + travelled * v;
            
            VelocityDirection = v;

            // speed will be a function of what is remaining length and remaining ticks left.
            float totalLength = mRoad.RoadSegmentLength(mCurrentRoadSegment);
            float remainLength = totalLength - travelled;
            float percentLeft =  remainLength / totalLength;
            float ticksLeft = ticksToTravel * percentLeft;

            Speed = remainLength/ ticksLeft;
        }
    }
}
