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
        public XNACS1Circle mBall = null;
        private float mBallRadius = 2f;
        private Vector2 mBallPos;
        private Vector2 mBallVelocity;
        private float mBallSpeed = 1f;

        public Ball(Vector2 pos, float speed)
        {
            mBallVelocity = new Vector2(speed, speed);
            CreateBall(pos);
        }

        private void CreateBall(Vector2 pos)
        {
            mBall = new XNACS1Circle(pos, mBallRadius, "SoccerBall");
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
