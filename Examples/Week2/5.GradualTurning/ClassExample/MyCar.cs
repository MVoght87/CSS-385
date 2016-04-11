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
    public class MyCar : XNACS1Rectangle
    {
        private MyRoad mRoad;
        
        private int mCurrentRoadSegment = 0;
        private float mTurnRate = 0.5f;

        public MyCar(Vector2 pos, MyRoad road)
        {
            Texture = "MyCar";
            Width = 4f;
            Height = 4f;

            Center = pos;
            ShouldTravel = true;
            Speed = 0.8f;

            mRoad = road;
        }

        public void Update(float turnRate)
        {
            // continuously adjust the car's forward direction and speed
            mTurnRate = turnRate;


            /// Two distinct states to be concerned with
            /// 
            ///     1. There are next road segment to move towards
            ///     2. Within the bounds of current road segment
            ///     
            if (mCurrentRoadSegment < mRoad.NumDefinedSegments())
            {
                Vector2 travelled = Center - mRoad.RoadSegmentStartPos(mCurrentRoadSegment);
                if (travelled.Length() >= mRoad.RoadSegmentLength(mCurrentRoadSegment))
                {
                    mCurrentRoadSegment++;
                    if (mCurrentRoadSegment >= mRoad.NumDefinedSegments())
                        ResetCarPosition();
                }
            }
            else
            {
                ResetCarPosition();
            }

            UpdateCarDirAndSpeed();
        }

        public void ResetCarPosition()
        {
            Center = ClassExample.kInitPosition;
            mCurrentRoadSegment = 0;
            RotateAngle = 0f;
        }

        private void UpdateCarDirAndSpeed()
        {
            Vector2 targetDir = mRoad.RoadSegmentEndPos(mCurrentRoadSegment) - Center;
            targetDir.Normalize();
            
            float theta = (float)((180.0 / Math.PI) * 
                    Math.Acos((double)Vector2.Dot(FrontDirection, targetDir)));

            if (theta > 0.001f)
            { // if not already in the same direction

                Vector3 myDir3 = new Vector3(FrontDirection, 0f);
                Vector3 targetDir3 = new Vector3(targetDir, 0f);
                Vector3 sign = Vector3.Cross(myDir3, targetDir3);

                RotateAngle += Math.Sign(sign.Z) * theta * mTurnRate;
                VelocityDirection = FrontDirection;
            }
        }
    }
}
