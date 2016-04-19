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
    class SineFunctionPath : FunctionPath
    {
        // mFunctionXCoverage is mapped to number of period
        // mFunctionYCoverage is mapped to sine's amplitude

        public SineFunctionPath(Vector2 org, float w, float h, float xCoverage, float yCoverage)
            : base(org, w, h, xCoverage, yCoverage) { }


        protected override float GetYValue(float x)
        {
            float frequencyScale = mFunctionXCoverage * 2f * (float)(Math.PI) / mPlotBox.Width;
            return (mFunctionYCoverage * (float)(Math.Sin(x * frequencyScale)));
        }

    }
}
