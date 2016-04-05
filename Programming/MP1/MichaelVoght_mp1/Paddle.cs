/*
 * Author:      Michael Voght
 * Date:        April 7th, 2016
 * Paddle.cs:   Class handle the players paddle within the game. Handles the players
 *              controls, the creation of the object, and the removal of the object.
 */
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

namespace MichaelVoght_NameSpace
{

    public class Paddle
    {
        #region Initialization of instances and variables
        public XNACS1Rectangle mPaddle = null;
        private float mPaddleWidth = 10f;
        private float mPaddleHeight = 2f;
        private float mPaddleSpeed = 2f;
        private float mPaddleXPos;
        #endregion

        public Paddle()
        {
            CreatePaddle(new Vector2(50f, 5f));
        }

        private void CreatePaddle(Vector2 pos)
        {
            mPaddle = new XNACS1Rectangle(pos, mPaddleWidth, mPaddleHeight);
            mPaddle.Color = Color.Red;
        }

        public void updatePaddle(bool xButton, bool bButton, Vector2 key)
        {
            if( mPaddle != null )
            {
                if (xButton)
                    ChangePaddleWidthCheat(1f);
                if (bButton)
                    ChangePaddleWidthCheat(-1f);
                mPaddle.CenterX += (mPaddleSpeed * key.X);
            }
        }

        private void ChangePaddleWidthCheat(float inc)
        {
            if ((inc > 0 && mPaddleWidth < 30) || (inc < 0 && mPaddleWidth > 2))
            {
                mPaddleXPos = mPaddle.CenterX;
                mPaddleWidth += inc;
                DestroyPaddle();
                CreatePaddle(new Vector2(mPaddleXPos, 5f));
            }
        }

        private void DestroyPaddle()
        {
            mPaddle.RemoveFromAutoDrawSet();
            mPaddle = null;
        }

    }
}
