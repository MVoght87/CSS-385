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

    public class PaddleSet
    {
        public enum PaddleSetType
        {
            eLeftSet,
            eRightSet
        };

        private Paddle[] mPaddles;

        /// <summary>
        /// Create a set of paddles lined up in y, that either ract to left or right trigger
        /// </summary>
        /// <param name="num">number of paddels in a row</param>
        /// <param name="xPosition">xLocation of the paddles</param>
        /// <param name="initYPosition">First yPosition of the paddles</param>
        /// <param name="yDistance">How far are the paddles away from each other</param>
        public PaddleSet(PaddleSetType type, int num, float xPosition, float initYPosition, float yDistance)
        {
            mPaddles = new Paddle[num];
            float y = initYPosition;
            for (int i = 0; i < num; i++)
            {
                if (PaddleSetType.eLeftSet == type)
                    mPaddles[i] = new LeftPaddle(new Vector2(xPosition, y));
                else
                    mPaddles[i] = new RightPaddle(new Vector2(xPosition, y));

                y += yDistance;
            }
        }

        public bool UpdatePaddles(float rotation, MyBall ball)
        {
            bool hit = false;
            int i = 0; 

            while ((!hit) && (i<mPaddles.Length)) {
                    hit = mPaddles[i].Update(rotation, ball);
                    i++;
            }
            return hit;
        }
    }
}