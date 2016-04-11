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
        private Color kUVecColor = Color.Blue;
        private Color kVVecColor = Color.Green;

        private XNACS1Rectangle mVec, mTangent, mNormal;
        private XNACS1Circle mStart;

        private float kDrawSize = 8f; // draw size ...

        public VectorComponents()
        {
            // Create first to show on bottom!
            mStart = new XNACS1Circle();
            mStart.Radius = 0.5f;
            mStart.Color = Color.Black;

            mVec = new XNACS1Rectangle();
            mTangent = new XNACS1Rectangle();
            mNormal = new XNACS1Rectangle();
        }


        public void Update(Vector2 from, Vector2 to, Vector2 tangent)
        {
            Vector2 toCenter = to - from;

            float sizeOnTangent = Vector2.Dot(toCenter, tangent); // may be negative, its OK!
            Vector2 normal = toCenter - (sizeOnTangent * tangent);

            /// Compute project size on the normal vector
            normal.Normalize();
            float sizeOnNormal = Vector2.Dot(toCenter, normal);
                    /// 
                    /// An alternate, and seeming more straightforward approach would be:
                    ///           float sizeOnNormal = normal.Length();  // to compute the size or normal
                    ///           vector normal /= sizeOnNormal;         // to normalize the normal vector
                    ///
                    /// One problem with this alternate approach is that, the "sizeOnNormal" is now an absolute
                    /// value quantity and does not inlcude information on direction, where is the "dotProduct"
                    /// approach above gives us a "signed-value" were we know if toCenter is in the direction of
                    /// the normal vector" or in the opposite direction of the norml direciton.
                    /// 

            Vector2 tangentEndPos = from + (kDrawSize * sizeOnTangent * tangent);
            mStart.Center = from;
            mVec.SetEndPoints(from, from+(kDrawSize * toCenter), kVecWidth);
            mTangent.SetEndPoints(from, tangentEndPos, kVecWidth);
            mNormal.SetEndPoints(tangentEndPos, tangentEndPos+(kDrawSize* sizeOnNormal * normal), kVecWidth);

            mVec.Color = kVecColor;
            mTangent.Color = kUVecColor;
            mNormal.Color = kVVecColor;
        }

        public Vector2 TangentDir() { return mTangent.FrontDirection; }
        public float TangentSize() { return mTangent.Width/kDrawSize; }
        public Vector2 TangentVector() { return TangentDir() * TangentSize(); }
        
        public Vector2 NormalDir() { return mNormal.FrontDirection; }
        public float NormalSize() { return mNormal.Width/kDrawSize; }
        public Vector2 NormalVector() { return NormalDir() * NormalSize(); }

        public void HideVectorComponents()
        {
            mStart.RemoveFromAutoDrawSet();
            mVec.RemoveFromAutoDrawSet();
            mTangent.RemoveFromAutoDrawSet();
            mNormal.RemoveFromAutoDrawSet();
        }

    }
}
