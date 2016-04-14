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
        private const float kRoadY = 30f;
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

        private float mSecondsToTravel = 5f;
        private int mUpdatesSinceShoot = 0;
        private XNACS1Circle mBall;


        protected override void InitializeWorld()
        {
            World.SetWorldCoordinate(new Vector2(-5f, 0f), kDistanceCovered+kUnits);

            // Create the road mile posts ...
            for (int i = 0; i < kUnits; i++)
            {
                int centerX = (i * kUnits) + (kUnits/2);
                XNACS1Rectangle r = new XNACS1Rectangle(new Vector2((float)centerX, kRoadY), kUnits, 1);
                r.Color = kRoadColor[i];
                r.Label = centerX.ToString();
            }

            mBall = new XNACS1Circle(new Vector2(kDistanceCovered+1f, kRoadY), 2f);
            mBall.ShouldTravel = true;
            mBall.RemoveFromAutoDrawSet();
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

            #region re-compute ball speed
            if (GamePad.ButtonAClicked())
            {
                ComputeBallSpeed();
                mUpdatesSinceShoot = 0;
                mBall.CenterX = 0f;
                mBall.AddToAutoDrawSet();
            }
            #endregion 

            #region continue to draw the ball?
            if (mBall.CenterX > kDistanceCovered)
                mBall.RemoveFromAutoDrawSet();
            else
                mUpdatesSinceShoot++;
            #endregion

            EchoToTopStatus("Ball position: " + mBall.CenterX);
            EchoToBottomStatus("Travel across 100 units in: " + mSecondsToTravel + " seconds" 
                    + " UpdatesSinceShoot(" + mUpdatesSinceShoot + ")");
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
            mBall.VelocityX = kDistanceCovered * (1f / World.TicksInASecond) * (1f / mSecondsToTravel);
        }
        
    }
}
