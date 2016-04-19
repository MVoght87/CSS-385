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
    class ParabolaFunctionPath : FunctionPath
    {
        // y = 2 * x^2;
        //
        // mFunctionXCoverage: -X to +X for ploting the parabola
        // mFunctionYCoverage scales the parabola y such that:
        //
        //      computeY / (2f*mFunctionXCoverage*mFunctionXCoverage) = retrunY / mFunctionYCoverage

        public ParabolaFunctionPath(Vector2 org, float w, float h, float xCoverage, float yCoverage)
            : base(org, w, h, xCoverage, yCoverage) { }

        private float GetUseX(float x)
        {
            float useX = x - (mPlotBox.Width / 2f);
            useX *= 2f * mFunctionXCoverage / mPlotBox.Width;
            return useX;
        }

        protected override float GetYValue(float x)
        {
            float useX = GetUseX(x);
            float computedY = 2f * useX * useX;
            float returnY = computedY * mFunctionYCoverage / (2f * mFunctionXCoverage * mFunctionXCoverage);
            return returnY - (mPlotBox.Height/2f);
        }
    }
}
