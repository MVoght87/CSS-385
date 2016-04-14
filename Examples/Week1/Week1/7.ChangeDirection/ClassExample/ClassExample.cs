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

        private const float kDistanceCovered = 100f;
        private const float kWorldSize = 2f * kDistanceCovered;

        private  Color[] kRoadColor = {
                                     Color.Red,
                                     Color.White,
                                     Color.Blue,
                                     Color.Black,
                                     Color.Brown,
                                     Color.Coral,
                                     Color.BurlyWood,
                                     Color.Green,
                                     Color.Gold,
                                     Color.Honeydew};
        private const int kUnits = 10;
        private Vector2 kInitPosition = new Vector2(0f, 0f);
        private const float kRoadWidth = 1f;

        private XNACS1Rectangle[] mRoad;
        private float mSecondsToTravel = 5f;
        private int mUpdatesSinceShoot = 0;
        private XNACS1Circle mBall;
        private float mDistanceTravelled = kWorldSize * 2f;


        protected override void InitializeWorld()
        {
            World.SetWorldCoordinate(new Vector2(-kWorldSize, -kWorldSize*(9f/16f)), (2f*kWorldSize));

            mRoad = new XNACS1Rectangle[10];
            // Create the road mile posts ...
            for (int i = 0; i < kUnits; i++)
            {
                mRoad[i] = new XNACS1Rectangle(kInitPosition, kUnits, kRoadWidth);
                mRoad[i].Color = kRoadColor[i];
                mRoad[i].Label = ((i * kUnits) + (kUnits / 2)).ToString();
            }

            mBall = new XNACS1Circle(kInitPosition, 2f);
            mBall.ShouldTravel = true;
            mBall.RemoveFromAutoDrawSet();
            mDistanceTravelled = kWorldSize * 2f;

            GenerateRandomBallVelocityDirection();
            ComputeBallSpeed();
        }

        protected override void UpdateWorld()
        {
            if (GamePad.ButtonBackClicked())
                Exit();

            #region change speed by inc/dec time for travelling
            mSecondsToTravel += GamePad.ThumbSticks.Right.Y;
            if (mSecondsToTravel <= 0f)
                mSecondsToTravel = 0.1f;
            #endregion 

            #region recompute new speed for ball
            if (GamePad.ButtonAClicked())
            {
                ComputeBallSpeed();
                mBall.AddToAutoDrawSet();
                ResetBallShootInfo();
            }
            #endregion 

            #region recompute new velocity direction for ball
            if (GamePad.ButtonBClicked())
            {
                GenerateRandomBallVelocityDirection();
                ResetBallShootInfo();
                mBall.TopOfAutoDrawSet();
            }
            #endregion 

            #region determine if should contiune to draw the ball
            if (mBall.IsInAutoDrawSet())
            {
                Vector2 delta = mBall.Center - kInitPosition;
                mDistanceTravelled = delta.Length();

                if (mDistanceTravelled > kDistanceCovered)
                    mBall.RemoveFromAutoDrawSet();
                else
                    mUpdatesSinceShoot++;
            }
            #endregion

            EchoToTopStatus("Ball velocity:" + mBall.Velocity + "  position:" + mBall.Center);
            EchoToBottomStatus("Travel across " + kDistanceCovered + " units in: " + mSecondsToTravel + " seconds" 
                    + " UpdatesSinceShoot(" + mUpdatesSinceShoot + ")");
        }

        private void GenerateRandomBallVelocityDirection()
        {
            float x = RandomFloat(-1f, 1f);
            float y = RandomFloat(-1f, 1f);
            Vector2 v = new Vector2(x, y);
            v.Normalize();
            Vector2 currPos = kInitPosition;

            // now generate the road along the new velocity direction
            for (int i = 0; i < kUnits; i++)
            {
                // Vector2 nextPos = currPos + (kUnits * v);
                Vector2 nextPos = kInitPosition + ((i+1)*kUnits) * v;
                
                mRoad[i].SetEndPoints(currPos, nextPos, kRoadWidth);
                mRoad[i].Color = kRoadColor[i];
                mRoad[i].Label = ((i * kUnits) + (kUnits / 2f)).ToString();
                currPos = nextPos;
            }
            mBall.VelocityDirection = v;
        }

        private void ComputeBallSpeed()
        {
            // To travel 
            //       kDistanceCovered in mSecondsToTravel, 
            // given we update World.TicksInASecond
            //
            // since we change position at every update,
            // our speed units will be in: 
            //
            //      displacement per tick
            //
            // Speed = distance * (Second/Tick) * 1/Second
            //
            // Note: World.TicksInASecond has unit (Ticks/Second)
            //
            float speed = kDistanceCovered * (1f / World.TicksInASecond) * (1f / mSecondsToTravel);
            mBall.Speed = speed;
        }

        private void ResetBallShootInfo()
        {
            mUpdatesSinceShoot = 0;
            mBall.Center = kInitPosition;
        }
        
    }
}
