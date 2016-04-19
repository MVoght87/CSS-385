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
        private const float kWorldWidth = 100f;
        private const float kPlotWidth = 20f;
        private const float kPlotHeight = 12f;
 
        // the var
        private const int kTotalPlots = 16;
        private FunctionPath[] mPlotPath;
        private int mCurrentPlot;

        protected override void InitializeWorld()
        {
            World.SetWorldCoordinate(new Vector2(0, 0), kWorldWidth);

            mCurrentPlot = 0;
            mPlotPath = new FunctionPath[kTotalPlots];
            float x = 5f;
            float y = kPlotHeight;

            while (mCurrentPlot < kTotalPlots)
            {
                float ox = RandomFloat(x-2f, x + 2f);
                float oy = RandomFloat(y - 1f, y + 1f);
                float w = RandomFloat(kPlotWidth * 0.75f, kPlotWidth * 1.25f);
                float h = RandomFloat(kPlotHeight * 0.75f, kPlotHeight * 1.25f);
                if (RandomFloat() > 0.5f)
                {
                    float period = RandomFloat(2f, 5f);
                    float amp = RandomFloat(2f, 5f);
                    mPlotPath[mCurrentPlot] = new SineFunctionPath(new Vector2(ox, oy), w, h, period, amp);
                }
                else
                {
                    float max = RandomFloat(2f, 4f);
                    float amp = RandomFloat(4f, 8f);
                    mPlotPath[mCurrentPlot] = new ParabolaFunctionPath(new Vector2(ox, oy), w, h, max, amp);
                }
                mPlotPath[mCurrentPlot].ShowPlots = false;
                mPlotPath[mCurrentPlot].Update(Vector2.Zero, 0f, 0f, RandomFloat(-120f, 120f), Vector2.Zero);

                mCurrentPlot++;
                x += kPlotWidth;
                if (x > (World.WorldMax.X-kPlotWidth))
                {
                    x = 5f;
                    y += kPlotHeight;
                }
            }
            mCurrentPlot = 0;
        }

        protected override void UpdateWorld()
        {
            if (GamePad.ButtonBackClicked())
                Exit();

            // Update all of them!!
            for (int i = 0; i < kTotalPlots; i++)
            {
                if (i != mCurrentPlot)
                    mPlotPath[i].Update(Vector2.Zero, 0f, 0f, 0f, Vector2.Zero);
            }

            if (GamePad.ButtonXClicked())
            {
                mCurrentPlot++;
                if (mCurrentPlot >= kTotalPlots)
                    mCurrentPlot = 0;
            }

            if (GamePad.ButtonBClicked())
                mPlotPath[mCurrentPlot].ShowPlots = !mPlotPath[mCurrentPlot].ShowPlots;

            if (GamePad.ButtonAClicked())
                mPlotPath[mCurrentPlot].ConstantSpeed = !mPlotPath[mCurrentPlot].ConstantSpeed;

            float deltaW = GamePad.Triggers.Left;
            if (GamePad.Buttons.LeftShoulder == ButtonState.Pressed)
                deltaW -= 0.2f;

            float deltaH = GamePad.Triggers.Right;
            if (GamePad.Buttons.RightShoulder == ButtonState.Pressed)
                deltaH -= 0.2f;

            float deltaR = 0f;
            if (GamePad.Dpad.Up == ButtonState.Pressed)
                deltaR += 2f;
            else if (GamePad.Dpad.Down == ButtonState.Pressed)
                deltaR -= 2f;

            mPlotPath[mCurrentPlot].Update(GamePad.ThumbSticks.Right, deltaW, deltaH, deltaR, GamePad.ThumbSticks.Left);

            EchoToTopStatus("LeftThumb.X/Y adjust Amplitude/Period; Box: Pos-RightStick Size:Left/Right Trigger/Button; Rotate-Dpad Up/Down");
            EchoToBottomStatus("XButton=NextPlot BButton=PlotAreaVisibility ... XCovearge=" + mPlotPath[mCurrentPlot].XCoverage +
                               "   YCoverage=" + mPlotPath[mCurrentPlot].YCoverage + "   ConstantSpeed=" + mPlotPath[mCurrentPlot].ConstantSpeed);
        }
    }
}