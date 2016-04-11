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
    public class ShowVector
    {
        private const float kVecWidth = 0.2f;
        private Color kVecColor = Color.Red;
        private Color kXVecColor = Color.LightBlue;
        private Color kYVecColor = Color.LightGreen;


        private XNACS1Rectangle mVec, mVecInX, mVecInY;
        private XNACS1Circle mStart;
        private Vector2 mDir;
        private float mSize;


        public ShowVector(Vector2 startPos, Vector2 dir, float size) {
            mDir = dir;
            mSize = size;

            mVec = new XNACS1Rectangle();
            mVecInX = new XNACS1Rectangle();
            mVecInY = new XNACS1Rectangle();

            // Create last to show on top!
            mStart = new XNACS1Circle(startPos, kVecWidth / 3);
            mStart.Color = Color.Red;

            UpdateEndPoints();
        }

        public void SetStartPos(Vector2 pos) 
        {
            mStart.Center = pos;
            UpdateEndPoints();
        }

        public void SetVectorSize(float s)
        {
            mSize = s;
            UpdateEndPoints();

        }

        public void SetVectorDir(Vector2 dir)
        {
            mDir = Vector2.Normalize(dir);
            UpdateEndPoints();
        }

        public void RemoveAllFromDraw()
        {
            mStart.RemoveFromAutoDrawSet();
            mVec.RemoveFromAutoDrawSet();
            mVecInX.RemoveFromAutoDrawSet();
            mVecInY.RemoveFromAutoDrawSet();
        }

        public Vector2 GetVectorStartPos()
        {
            return mStart.Center;
        }

        public Vector2 GetVectorEndPos()
        {
            return mStart.Center + (mSize * mDir);
        }

        public Vector2 GetVectorDir()
        {
            return mDir;
        }
        public float GetVectorSize()
        {
            return mSize;
        }

        public void FinalizeVector()
        {
            mVecInX.RemoveFromAutoDrawSet();
            mVecInY.RemoveFromAutoDrawSet();
        }

        private void UpdateEndPoints()
        {
            Vector2 delta = mSize * mDir;
            mVec.SetEndPoints(mStart.Center, mStart.Center + delta, kVecWidth);
            mVecInX.SetEndPoints(mStart.Center, new Vector2(mStart.CenterX+delta.X, mStart.CenterY), kVecWidth);
            mVecInY.SetEndPoints(mStart.Center, new Vector2(mStart.CenterX, mStart.CenterY+delta.Y), kVecWidth);
            mVec.Color = kVecColor;
            mVecInX.Color = kXVecColor;
            mVecInY.Color = kYVecColor;
        }
    }
}
