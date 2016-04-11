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
        private float mPeriods; // periods of since curve in the world dimension X
        private float mFrequencyScale;
        private float mAmplitude;

        private Vector2 mInitPos;
        private XNACS1Circle mTravel;
        private XNACS1PrimitiveSet mPlotSet;
        private bool mConstantSpeed;

        protected override void InitializeWorld()
        {
            World.SetWorldCoordinate(new Vector2(0, 0), 100f);
            mPeriods = 2f;
            ComputeFrequencyScale();
            mAmplitude = 10f;

            mInitPos = new Vector2(0, World.WorldDimension.Y/2f);
            mTravel = new XNACS1Circle(mInitPos, 2f);
            mTravel.Color = Color.Black;

            mPlotSet = new XNACS1PrimitiveSet();

            mConstantSpeed = false;

            PlotSineCurve();
        }

        protected override void UpdateWorld()
        {
            if (GamePad.ButtonBackClicked())
                Exit();

            if (GamePad.ButtonAClicked())
                mConstantSpeed = !mConstantSpeed;

            #region Update the "mTravel" object position according to the sine curve
            if (!mConstantSpeed)
            {
                mTravel.CenterX += 0.5f;
                mTravel.CenterY = mInitPos.Y + GetYValue(mTravel.CenterX); // this is the sine function!!
            }
            else
            {
                Vector2 next = new Vector2(mTravel.CenterX + 0.5f, mInitPos.Y + GetYValue(mTravel.CenterX));
                Vector2 dir = next - mTravel.Center;
                dir.Normalize();
                mTravel.Center += dir;
            }
            #endregion 

            if (mTravel.CenterX > World.WorldDimension.X)
                mTravel.Center = mInitPos;

            #region Changing the Sine curve
            bool once = false;
            if (GamePad.ThumbSticks.Left.Y != 0f)
            {
                mPeriods += (GamePad.ThumbSticks.Left.Y * 0.1f);
                once = true;
            }
            if (GamePad.ThumbSticks.Left.X != 0f)
            {
                mAmplitude += GamePad.ThumbSticks.Left.X;
                once = true;
            }

            if (once)
                PlotSineCurve();
            #endregion 

            EchoToTopStatus("LeftThumb.Y adjust Period; LeftThumb.X adjust Amplitude");
            EchoToBottomStatus("Period=" + mPeriods + "     Amplitude=" + mAmplitude + "   ConstantSpeed=" + mConstantSpeed);
        }
        
        private void ComputeFrequencyScale()
        {
            mFrequencyScale = mPeriods * 2f * (float)(Math.PI) / World.WorldDimension.X;
        }

        private float GetYValue(float x) {
            return mAmplitude * (float) (Math.Sin(x * mFrequencyScale));
        }

        private void PlotSineCurve()
        {
            mPlotSet.RemoveAllFromSet();
            ComputeFrequencyScale();

            for (int x = 0; x < World.WorldDimension.X; x++)
            {
                float y = mInitPos.Y + GetYValue(x);
                XNACS1Circle c = new XNACS1Circle(new Vector2(x, y), 0.2f);
                mPlotSet.AddToSet(c);
            }
            mTravel.TopOfAutoDrawSet();
        }
    }
}
