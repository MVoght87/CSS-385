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
            ///     1. Has a valid next state (road segment)
            ///         1a. continues to be valid (within the bounds of road segment)
            ///         1b. if not, transition to next state
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
           // if ( newState
                  // || (mCurrentRoadSegment == (mRoad.NumDefinedSegments() - 1))
             //   )
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
           Vector2 endPos = mRoad.RoadSegmentEndPos(mCurrentRoadSegment);
           Vector2 dir = endPos - Center;
           VelocityDirection = dir;

           // amount of time we have to travel the remaining of the road segment
           float totalLength = mRoad.RoadSegmentLength(mCurrentRoadSegment);
           float remainLehgth = dir.Length();
           float percentLeft = remainLehgth / totalLength;
           float ticksLeft = ticksToTravel * percentLeft;

           Speed = remainLehgth / ticksLeft;
        }
    }
}
