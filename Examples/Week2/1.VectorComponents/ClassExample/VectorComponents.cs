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
    /// </summary>
    public class VectorComponents 
    {
        private const float kVecWidth = 0.2f;
        private Color kVecColor = Color.Red;
        private Color kUVecColor = Color.LightBlue;
        private Color kVVecColor = Color.LightGreen;

        private XNACS1Rectangle mVec, mTangent, mNormal;
        private XNACS1Circle mStart, mPtOnTangent;

        public VectorComponents()
        {
            // Create first to show on bottom!
            mStart = new XNACS1Circle();
            mStart.Radius = 0.5f;
            mStart.Color = Color.White;

            mVec = new XNACS1Rectangle();
            mTangent = new XNACS1Rectangle();
            mNormal = new XNACS1Rectangle();
            
            mPtOnTangent = new XNACS1Circle();
            mPtOnTangent.Color = Color.Black;
            mPtOnTangent.Radius = 2f;
        }


        public void Update(Vector2 from, Vector2 to, Vector2 tangent)
        {
            Vector2 toCenter = to - from;

            float sizeOnTangent = Vector2.Dot(toCenter, tangent); // may be negative, its OK!
            Vector2 normal = toCenter - (sizeOnTangent * tangent);
            normal.Normalize();
            float sizeOnNormal = Vector2.Dot(toCenter, normal);
            
            mPtOnTangent.Center = from + (sizeOnTangent * tangent);
            mPtOnTangent.AddToAutoDrawSet();

            mStart.Center = from;
            mVec.SetEndPoints(from, to, kVecWidth);
            mTangent.SetEndPoints(from, from+(sizeOnTangent * tangent), kVecWidth);
            mNormal.SetEndPoints(mPtOnTangent.Center, mPtOnTangent.Center+(sizeOnNormal * normal), kVecWidth);

            mVec.Color = kVecColor;
            mTangent.Color = kUVecColor;
            mNormal.Color = kVVecColor;
        }

        public void Update(Vector2 from, Vector2 to, Vector2 tangent, Vector2 normal)
        {
            Vector2 toCenter = to - from;

            float sizeOnTangent = Vector2.Dot(toCenter, tangent); // may be negative, its OK!
            float sizeOnNormal = Vector2.Dot(toCenter, normal);

            mVec.SetEndPoints(from, to, kVecWidth);
            mTangent.SetEndPoints(from, from + (sizeOnTangent * tangent), kVecWidth);
            mNormal.SetEndPoints(from, from + (sizeOnNormal * normal), kVecWidth);

            mVec.Color = kVecColor;
            mTangent.Color = kUVecColor;
            mNormal.Color = kVVecColor;
        }


        public void HideVectorComponents()
        {
            mStart.RemoveFromAutoDrawSet();
            mVec.RemoveFromAutoDrawSet();
            mTangent.RemoveFromAutoDrawSet();
            mNormal.RemoveFromAutoDrawSet();
            mPtOnTangent.RemoveFromAutoDrawSet();
        }

    }
}
