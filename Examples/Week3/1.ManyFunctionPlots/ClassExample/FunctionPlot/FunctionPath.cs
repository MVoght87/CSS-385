using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


using XNACS1Lib;

namespace ClassExample
{
    /// <summary>
    /// Abstracts:
    /// 
    /// 1. mPlotBox: the area upon which to plot the function
    /// 2. X/Y range of the function: as FucntionXCoverage and FunctionYCoverage
    /// 
    /// </summary>
    abstract class FunctionPath
    {
        protected const float kNumPlotPoints = 200f;
        protected const float kPlotPtRatio = 1.2f; // size of the pts in the plot as a percentage of plot distance
        protected const float kTravelRatio = 1.5f; // size of the travel circle as a percentage of plot distance

        protected PlotBox mPlotBox; // the box where the function will be plotted in
        protected float mPlotDist;  // PlotBox.Width / kNumPlotPoints
        protected bool mShowPlots;  // show the plot area and the plot positions

        protected float mFunctionXCoverage;  // X-range of function to plot
        protected float mFunctionYCoverage;  // Y-range of function to plot
        protected XNACS1PrimitiveSet mPlotSet;

        private XNACS1Circle mTravel;
        private float mXTravelDistance;
        private bool mConstantSpeed;

        public FunctionPath(Vector2 origin, float w, float h, float xCoverage, float yCoverage)
        {
            mShowPlots = true;

            mPlotBox = new PlotBox(origin, w, h);

            mFunctionXCoverage = xCoverage;
            mFunctionYCoverage = yCoverage;
            mPlotDist = mPlotBox.Width / kNumPlotPoints;

            mXTravelDistance = 0f;
            mTravel = new XNACS1Circle(Vector2.Zero, kTravelRatio * mPlotDist);
            mTravel.Center = mPlotBox.GetBoxPosition(mXTravelDistance, GetYValue(mXTravelDistance));
            mTravel.Color = Color.Black;

            mConstantSpeed = false;

            mPlotSet = new XNACS1PrimitiveSet();
            PlotFunction();
            mTravel.TopOfAutoDrawSet();

        }

        public virtual void Update(Vector2 orgDelta, float wDelta, float hDelta, float rotDelta, Vector2 deltaCoverage)
        {
            bool changed = mPlotBox.Update(orgDelta, wDelta, hDelta, rotDelta);

            if (deltaCoverage != Vector2.Zero)
            {
                changed = true;
                mFunctionXCoverage += deltaCoverage.X;
                mFunctionYCoverage += deltaCoverage.Y;
            }

            if (changed)
                PlotFunction();

            float nextX = mXTravelDistance + mPlotDist;
            float nextY = GetYValue(nextX);
            if (!mConstantSpeed)
            {
                mTravel.Center = mPlotBox.GetBoxPosition(nextX, nextY);
                mXTravelDistance += mPlotDist;
            }
            else
            {
                // 1. Compute the current position in function space
                float currentY = GetYValue(mXTravelDistance);
                Vector2 currentPt = new Vector2(mXTravelDistance, currentY);
                
                // 2. Compute the next position in function space
                Vector2 nextPt = new Vector2(nextX, nextY);

                // 3. compute the direction/size of what we should move
                Vector2 dir = nextPt - currentPt;

                // 4. normalize that direction and scale the size into 0.5f
                dir.Normalize();
                dir *= 0.5f;

                // 5. next position is the scaled offset from current position
                nextPt = currentPt + dir;

                // 6. plot position is offset the function position by the poltBox origin
                mTravel.Center = mPlotBox.GetBoxPosition(nextPt.X, nextPt.Y);
                
                // 7. this is how much we have moved 
                mXTravelDistance += dir.X;
            }

            if (mXTravelDistance > mPlotBox.Width)
            {
                mXTravelDistance = 0f;
                mTravel.Center = mPlotBox.GetBoxPosition(mXTravelDistance, GetYValue(mXTravelDistance));
            }
        }

        protected void PlotFunction()
        {
            mPlotSet.RemoveAllFromSet();
            mPlotDist = mPlotBox.Width / kNumPlotPoints;
            for (float xDist = 0f; xDist < mPlotBox.Width; xDist += mPlotDist)
            {
                float nextY = GetYValue(xDist);
                Vector2 p = mPlotBox.GetBoxPosition(xDist, nextY);
                XNACS1Circle c = new XNACS1Circle(p, kPlotPtRatio * mPlotDist);
                if (mPlotBox.Collided(c))
                    mPlotSet.AddToSet(c);
                else
                    c.RemoveFromAutoDrawSet();
            }
            mTravel.TopOfAutoDrawSet();
            if (!mShowPlots)
                mPlotSet.RemoveAllFromAutoDrawSet();
        }

        public float XCoverage { get { return mFunctionXCoverage; } set { mFunctionXCoverage = value; } }
        public float YCoverage { get { return mFunctionYCoverage; } set { mFunctionYCoverage = value; } }
        public PlotBox PlotArea { get { return mPlotBox; } }
        public bool ConstantSpeed { get { return mConstantSpeed; } set { mConstantSpeed = value; } }
        public bool ShowPlots
        {
            get { return mShowPlots; }
            set
            {
                mShowPlots = value;
                if (mShowPlots)
                {
                    mPlotBox.AddToAutoDrawSet();
                    mPlotSet.AddAllToAutoDraw();
                    mTravel.TopOfAutoDrawSet();
                }
                else
                {
                    mPlotBox.RemoveFromAutoDrawSet();
                    mPlotSet.RemoveAllFromAutoDrawSet();
                }
            }
        }

        /// <summary>
        ///  Subclass must implement these two functions to realize any actual function!
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        protected abstract float GetYValue(float x);

    }
}
