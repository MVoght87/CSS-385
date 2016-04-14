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
    public class MyRoad
    {
        private const float kRoadSize = 0.5f;

        private List<ShowVector> mTheRoad;
        private ShowVector mCurrentNode;

        public MyRoad(Vector2 pos)
        {
            mTheRoad = new List<ShowVector>();
            InitializeRoad();
        }

        private void InitializeRoad()
        {
            mCurrentNode = new ShowVector(ClassExample.kInitPosition, new Vector2(1, 0), kRoadSize);
            mTheRoad.Add(mCurrentNode);
        }

        public void FinalizeRoadSegment(Vector2 dir, float len) {
            UpdateRoadSegment(dir, len);
            mCurrentNode.FinalizeVector();
            mCurrentNode = new ShowVector(mCurrentNode.GetVectorEndPos(), dir, kRoadSize);
            mTheRoad.Add(mCurrentNode);
        }

        public void UpdateRoadSegment(Vector2 dir, float len)
        {
            mCurrentNode.SetVectorDir(dir);
            mCurrentNode.SetVectorSize(len);
        }

        public Vector2 RoadSegmentStartPos(int index)
        {
            Vector2 pos = ClassExample.kInitPosition;
            if (index < mTheRoad.Count())
                pos = mTheRoad[index].GetVectorStartPos();
            return pos;
        }

        public float RoadSegmentLength(int index)
        {
            float len = 0;
            if (index < mTheRoad.Count())
                len = mTheRoad[index].GetVectorSize();
            return len;
        }

        public Vector2 RoadSegmentDir(int index)
        {
            Vector2 dir = new Vector2(1f, 0);
            if (index < mTheRoad.Count())
                dir = mTheRoad[index].GetVectorDir();
            return dir;
        }

        public Vector2 RoadSegmentEndPos(int index)
        {
            Vector2 pos = ClassExample.kInitPosition;
            if (index < mTheRoad.Count())
                pos = mTheRoad[index].GetVectorEndPos();
            return pos;
        }

        public int NumDefinedSegments()
        {
            return mTheRoad.Count();
        }

        public void ResetRoad()
        {
            for (int i = mTheRoad.Count() - 1; i >= 0; i--)
            {
                mTheRoad[i].RemoveAllFromDraw();
                mTheRoad.RemoveAt(i);
            }
            InitializeRoad();
        }


    }
}
