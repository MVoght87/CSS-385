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

using XNACS1Lib;

namespace ClassExample
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ClassExample : XNACS1Base
    {
        XNACS1Circle mTarget;
        MySoccer mSocccer;

        float mTurnRate = 1f;

        /// <summary>
        /// Initialize the internal state of your game
        /// </summary>
        protected override void InitializeWorld()
        {
            mTarget = new XNACS1Circle(new Vector2(50, 30), 1f);
            mTarget.Color = Color.Red;

            mSocccer = new MySoccer();
        }

        /// <summary>
        /// Update the internal sate of your game
        /// </summary>
        protected override void UpdateWorld()
        {
            if (GamePad.ButtonBackClicked())
                Exit();

            // move the target
            mTarget.Center += GamePad.ThumbSticks.Right;

            // change the turn rate
            mTurnRate += 0.02f * GamePad.ThumbSticks.Left.X;
            mTurnRate = MathHelper.Clamp(mTurnRate, 0f, 1f);

            mSocccer.Update(mTarget, mTurnRate);

            EchoToTopStatus("LeftThumb-X: TurnRate(" + mTurnRate + ") RightThumbStick: move target");
        }
        
    }
}
