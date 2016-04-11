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
        private VelocityBlock mVBlock;


        protected override void InitializeWorld()
        {
            World.SetWorldCoordinate(Vector2.Zero, kWorldSize);
            DefineGrid();

            mBlock = new MyBlock();
            mBall = new MySoccer(mBlock);
            mVBlock = new VelocityBlock(kInitPosition, new Vector2(1, 1));
        }

        protected override void UpdateWorld()
        {
            if (GamePad.ButtonBackClicked())
                Exit();

            #region Pause everything
            if (GamePad.ButtonXClicked())
                World.Paused = !World.Paused;
            
            if (World.Paused)
                return;
            #endregion

            #region shoot the ball
            if (GamePad.ButtonAClicked())
            {
                Vector2 velocity = mVBlock.VelocityDirection * mVBlock.Speed;
                mBall.ShootSoccer(kInitPosition, velocity);
            }
            #endregion

            #region Update Velocity Dir and size by thumbSticks
            mVBlock.UpdateVelocityBlock(GamePad.ThumbSticks.Right, GamePad.ThumbSticks.Left.Y);
            #endregion

            #region tell the Ball to update itself
            mBall.Update();
            #endregion

            #region Rotate the Block
            mBlock.UpdateBlock(Vector2.Zero, GamePad.Triggers.Right);
            #endregion

            EchoToTopStatus("Block rotated angle=" + mBlock.RotateAngle);
            EchoToBottomStatus("Vector Direction" + mVBlock.VelocityDirection + " Size: " + mVBlock.Speed);
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
