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

        public static Vector2 kInitPosition = new Vector2(0f, 0f);

        private Vector2 mDir;
        private float mLength = 5f;
        private float mTicksToTravel = 40f; // number of updates to travel each segment
        private int mTicksHasPassed = 0;

        private MyRoad mRoad;
        private MySoccer mBall;

        protected override void InitializeWorld()
        {
            World.SetWorldCoordinate(new Vector2(-5f, -5f), (1.5f*kDistanceCovered+5f));
            DrawGrid();

            mRoad = new MyRoad(kInitPosition);
            mBall = new MySoccer(kInitPosition, mRoad);

            mDir = new Vector2(1, 0);

            mBall.TopOfAutoDrawSet();
        }

        protected override void UpdateWorld()
        {
            if (GamePad.ButtonBackClicked())
                Exit();

            #region add a new road segment
            if (GamePad.ButtonAClicked())
            {
                mRoad.FinalizeRoadSegment(mDir, mLength);
            }
            #endregion

            #region Update vectorDir and size by thumbSticks
                mDir += GamePad.ThumbSticks.Right;
                mLength += GamePad.ThumbSticks.Left.Y;
                mTicksToTravel += GamePad.ThumbSticks.Left.X;
                mRoad.UpdateRoadSegment(mDir, mLength);
                mBall.TopOfAutoDrawSet();
            #endregion 

            #region tell the Ball to update itself
                mTicksHasPassed++;
                if (mBall.Update(mTicksToTravel))
                    mTicksHasPassed = 0;
            #endregion 

            #region restart ...
            if (GamePad.ButtonBClicked())
            {
                mRoad.ResetRoad();
                mBall.ResetBallPosition();
            }
            #endregion 

            EchoToTopStatus("Number of updates to travel each segment:" + mTicksToTravel + "  NumHasPassed(" + mTicksHasPassed + ")"); 
            EchoToBottomStatus("Vector Direction" + mDir + " Size: " + mLength);
        }

        private void DrawGrid()
        {
            const float kGridLineSize = 0.2f;
            for (int x = 0; x < World.WorldMax.X; x += 5)
            {
                XNACS1Rectangle r = new XNACS1Rectangle(new Vector2(x, 0f), new Vector2(x, World.WorldMax.Y-5f), kGridLineSize, "");
                r.Color = Color.White;

                r = new XNACS1Rectangle(new Vector2(x, 0), kGridLineSize, kGridLineSize);
                r.Label = x.ToString();
            }

            for (int y = 0; y < World.WorldMax.Y-5f; y += 5)
            {
                XNACS1Rectangle r = new XNACS1Rectangle(new Vector2(0f, y), new Vector2(World.WorldMax.X, y), kGridLineSize, "");
                r.Color = Color.White;

                r = new XNACS1Rectangle(new Vector2(0, y), kGridLineSize, kGridLineSize);
                r.Label = y.ToString();
            }
        }



    }
}
