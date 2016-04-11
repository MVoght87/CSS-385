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

        private const float kWorldSize = 100f;
        public static Vector2 kInitPosition = new Vector2(0f, 0f);

        private MyBlock mBlock;
        private MySoccer mBall;
        private VectorComponents mShowVecA, mShowVecB;


        protected override void InitializeWorld()
        {
            World.SetWorldCoordinate(Vector2.Zero, kWorldSize);
            DefineGrid();

            mBlock = new MyBlock();
            mBall = new MySoccer();

            mShowVecA = new VectorComponents();
            mShowVecA.HideVectorComponents();

            mShowVecB = new VectorComponents();
            mShowVecB.HideVectorComponents();
        }

        protected override void UpdateWorld()
        {
            if (GamePad.ButtonBackClicked())
                Exit();

            // Ball
            mBall.UpdateSoccerPosition(GamePad.ThumbSticks.Left);

            // Block
            mBlock.UpdateBlock(GamePad.ThumbSticks.Right, GamePad.Triggers.Right);
            
            if (GamePad.Buttons.A == ButtonState.Pressed)
            {
                mShowVecA.Update(mBlock.Center, mBall.Center, mBlock.FrontDirection);
            }
            else 
            {
                mShowVecA.HideVectorComponents();
            }

            if (GamePad.Buttons.B == ButtonState.Pressed)
            {
                mShowVecB.Update(mBall.Center, mBlock.Center, mBlock.FrontDirection, mBlock.NormalDirection);
            } else {
                mShowVecB.HideVectorComponents();
            }

            EchoToTopStatus("Block rotated angle=" + mBlock.RotateAngle);
        }

        private void DefineGrid()
        {
            const float kGridLineSize = 0.2f;
            for (int x = 0; x < World.WorldMax.X; x += 5)
            {
                XNACS1Rectangle r = new XNACS1Rectangle(new Vector2(x, 0f), new Vector2(x, World.WorldMax.Y-5f), kGridLineSize, "");
                r.Color = Color.White;

                r = new XNACS1Rectangle(new Vector2(x, 0), kGridLineSize, kGridLineSize);
                r.Label = x.ToString();
            }

            for (int y = 0; y < World.WorldMax.Y-5f; y += 5)
            {
                XNACS1Rectangle r = new XNACS1Rectangle(new Vector2(0f, y), new Vector2(World.WorldMax.X, y), kGridLineSize, "");
                r.Color = Color.White;
                r = new XNACS1Rectangle(new Vector2(0, y), kGridLineSize, kGridLineSize);
                r.Label = y.ToString();
            }
        }



    }
}
