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

        private  Color[] kRoadColor = {
                                     Color.Red,
                                     Color.White,
                                     Color.Blue,
                                     Color.Black,
                                     Color.Brown,
                                     Color.Coral,
                                     Color.BurlyWood,
                                     Color.Green,
                                     Color.Gold,
                                     Color.Honeydew};
        private const int kUnits = 10;
        private Vector2 kInitPosition = new Vector2(0f, 0f);
        private const float kRoadWidth = 1f;

        private Vector2 mDir;
        private Vector2 mCurrentPos;
        private float mVectorSize = 5;
        private ShowVector mCurrentVec;

        private int mAlreadyShown = kUnits+1;


        protected override void InitializeWorld()
        {
            World.SetWorldCoordinate(new Vector2(-5f, -5f), (1.5f*kDistanceCovered+5f));
            DrawGrid();
            mDir = new Vector2(1, 0);
            mCurrentPos = new Vector2();
            mCurrentPos = kInitPosition;

            mCurrentVec = new ShowVector(kInitPosition, mDir, mVectorSize);
        }

        protected override void UpdateWorld()
        {
            if (GamePad.ButtonBackClicked())
                Exit();

            #region add a new ViewVector
            if (GamePad.ButtonAClicked())
            {
                if (null != mCurrentVec)
                {
                    mCurrentVec.FinalizeVector();
                    mCurrentPos = mCurrentVec.EndPos();
                }
                mCurrentVec = new ShowVector(mCurrentPos, mDir, mVectorSize);
            }
            #endregion

            #region Update vectorDir by right thumbStick
                mDir += GamePad.ThumbSticks.Right;
                mDir.Normalize();
            #endregion 

            #region Update vectorSize by left thumbStick y
                mVectorSize += GamePad.ThumbSticks.Left.Y;
            #endregion 

            #region show another Vector addition
                if (null != mCurrentVec)
                {
                    mCurrentVec.SetVectorDir(mDir);
                    mCurrentVec.SetVectorSize(mVectorSize);
                }
            #endregion 

            #region restart ...
            if (GamePad.ButtonBClicked())
            {
                World.RemoveAllFromDrawSet();
                mCurrentVec = null;
                mAlreadyShown = 0;
                mCurrentPos = kInitPosition;
                DrawGrid();
            }
            #endregion 

            EchoToTopStatus("Segments shown:" + mAlreadyShown + "CurrentPos=" + mCurrentPos);
            EchoToBottomStatus("Vector Direction" + mDir + " Size: " + mVectorSize);
        }

        private void ComputeNewDir()
        {
            mDir.X = RandomFloat(0f, 1f);
            mDir.Y = RandomFloat(0f, 1f);
            mDir.Normalize();
        }

        private void DrawGrid()
        {
            const float kGridLineSize = 0.2f;
            for (int x = 0; x < World.WorldMax.X; x += 5)
            {
                XNACS1Rectangle r = new XNACS1Rectangle(new Vector2(x, 0f), new Vector2(x, World.WorldMax.Y), kGridLineSize, "");
                r.Color = Color.White;
                r.Color = Color.White;

                r = new XNACS1Rectangle(new Vector2(x, 0), kGridLineSize, kGridLineSize);
                r.Label = x.ToString();
            }

            for (int y = 0; y < World.WorldMax.Y; y += 5)
            {
                XNACS1Rectangle r = new XNACS1Rectangle(new Vector2(0f, y), new Vector2(World.WorldMax.X, y), kGridLineSize, "");
                r.Color = Color.White;
                r.Color = Color.White;
                r = new XNACS1Rectangle(new Vector2(0, y), kGridLineSize, kGridLineSize);
                r.Label = y.ToString();
            }
        }



    }
}
