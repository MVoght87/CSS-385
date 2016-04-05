/*
 * Author:              Michael Voght
 * Date:                April 7th, 2016
 * Content Mentions:    - http://exchange.smarttech.com/details.html?id=4d5d9103-d27a-4b29-b2ca-462a43009ad4
 *                        Publisher Trident MediaWorks
 * Ball.cs:             Class handles the ball within the game. Determines velocity direction based off the
 *                      collided object, handles the speed of the ball, the creation of the ball, and removal
 *                      of the ball.
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
    public class Ball
    {
        #region Initialization of variables and instances
        public XNACS1Circle mBall = null;
        private float mBallRadius = 2f;
        private Vector2 mBallPos;
        private Vector2 mBallVelocity;
        private float mBallSpeed = 1f;
        #endregion

        public Ball(Vector2 pos, float speed)
        {
            mBallVelocity = new Vector2(speed, speed);
            CreateBall(pos);
        }

        private void CreateBall(Vector2 pos)
        {
            mBall = new XNACS1Circle(pos, mBallRadius, "Ball");
            mBall.VelocityDirection = mBallVelocity;
            mBall.Speed = mBallSpeed;
            mBall.ShouldTravel = true;
        }

        public void DestroyBall()
        {
            mBall.RemoveFromAutoDrawSet();
            mBall = null;
        }

        public void updateBall(ButtonState uButton, ButtonState oButton)
        {
            if (ButtonState.Pressed == uButton)
                ChangeBallSpeedCheat(-0.1f);
            if (ButtonState.Pressed == oButton)
                ChangeBallSpeedCheat(0.1f);
        }

        public void WallCollide(bool top)
        {
            if (top)
            {
                mBall.VelocityY = -mBall.VelocityY;
            }
            else
            {
                mBall.VelocityX = -mBall.VelocityX;
            }
        }

        public void PaddleCollide(Paddle myPaddle)
        {
            mBall.VelocityDirection = mBall.Center - myPaddle.mPaddle.Center;
        }

        public void BrickCollide(Brick myBrick)
        {
            mBall.VelocityDirection = mBall.Center - myBrick.mBrick.Center;
        }

        private void ChangeBallSpeedCheat(float inc)
        {
            if ((inc > 0 && mBallSpeed < 2f) || (inc < 0 && mBallSpeed > 0.1f))
            {
                mBallPos = mBall.Center;
                mBallVelocity = mBall.VelocityDirection;
                mBallSpeed += inc;
                DestroyBall();
                CreateBall(mBallPos);
            }
        }
    }
}
