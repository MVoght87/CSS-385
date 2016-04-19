using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using XNACS1Lib;

namespace ClassExample
{
    class PlotBox : XNACS1Rectangle
    {
        private Vector2 mLeftCenterPos;
        private Vector2 mNormalDir;

        public PlotBox(Vector2 p, float w, float h)
        {
            mLeftCenterPos = p; // this is the left-center position of the box
            CenterY = p.Y;
            CenterX = p.X + (w / 2f);
            Width = w;
            Height = h;
            Color = Color.DarkGray;

            ComputePosition();
        }

        /// <summary>
        /// Uses the Center/NormalDir/FrontDir and Width/Height to compute the 
        /// LeftCenterPos of the plot box (for library drawing purposes)
        /// </summary>
        private void ComputePosition()
        {
            mNormalDir.X = -FrontDirection.Y;
            mNormalDir.Y = FrontDirection.X;

            mLeftCenterPos = Center - (Width / 2) * FrontDirection;
        }

        /// <summary>
        /// Changes the plot box location, width, and/or height
        /// </summary>
        /// <param name="orgDelta">amount to change for the origin</param>
        /// <param name="wDelta">amount to change for the width</param>
        /// <param name="hDelta">amount to change for the height</param>
        /// <param name="rotDelta">amount to change for the rotation</param>
        /// <returns>did any change occure?</returns>
        public bool Update(Vector2 orgDelta, float wDelta, float hDelta, float rotDelta)
        {
            bool once = false;

            if (orgDelta != Vector2.Zero)
            {
                Center += orgDelta;
                once = true;
            }
            if (wDelta != 0f)
            {
                once = true;
                Width += wDelta;
                if (Width < 1f)
                    Width = 1f;
            }
            if (hDelta != 0f)
            {
                once = true;
                Height += hDelta;
                if (Height < 1f)
                    Height = 1f;
            }
            if (rotDelta != 0f)
            {
                once = true;
                RotateAngle += rotDelta;
                if (RotateAngle > 360f)
                    RotateAngle -= 360f;
                else if (RotateAngle < -360f)
                    RotateAngle += 360f;
            }

            if (once)
                ComputePosition();

            return once;
        }

        private Vector2 PlotOrigin { get { return mLeftCenterPos; } }
        private float PlotOriginX { get { return mLeftCenterPos.X; } }
        private float PlotOriginY { get { return mLeftCenterPos.Y; } }

        public Vector2 GetBoxPosition(float xDistFromOri, float yDistFromOri)
        {
            return PlotOrigin + xDistFromOri * FrontDirection + yDistFromOri * mNormalDir;
        }
    }
}