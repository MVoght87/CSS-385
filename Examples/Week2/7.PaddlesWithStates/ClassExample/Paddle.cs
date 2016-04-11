using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using XNACS1Lib;

namespace ClassExample
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Paddle : XNACS1Rectangle
    {
        protected enum PaddleState
        {
            PaddleAtRest = 0,
            PaddleReturning = 1
        };

        private const float kPaddleWidth = 30f;
        private const float kPaddleHeight = 3f;

        protected const float kDownDuration = 40f; // slow to return 
        protected const float kAngleDisplacement = 90f;      // maximum rotation allowed for the paddle
        protected const float kRotateDownSpeed = kAngleDisplacement / kDownDuration; // down rotation speed
        protected const float kMaxUpRotation = 45f;

        protected const float kLeftRestPosition = kMaxUpRotation - kAngleDisplacement;

        protected PaddleState mCurrentPaddleState;

        private XNACS1Rectangle mShowDir;

        public Paddle(Vector2 pos)
        {
            mCurrentPaddleState = PaddleState.PaddleAtRest;

            Center = pos;
            Width = kPaddleWidth;
            Height = kPaddleHeight;
            RotateAngle = InitialPosition();

            mShowDir = new XNACS1Rectangle();
            mShowDir.SetEndPoints(Center, Center + (3 * FrontDirection), 0.8f);
            mShowDir.Color = Color.Black;
        }

        public bool Update(float userInput, MyBall ball)
        {
            float rotateTo = RotatedAngle(userInput);

            // update the rotine stuff
            bool ballDead = UpdateCollisionWithBall(ball);

            // Now come to handle our own state
            switch (mCurrentPaddleState)
            {
                case PaddleState.PaddleAtRest:
                    UpdatePaddleFromRest(rotateTo);
                    break;

                case PaddleState.PaddleReturning:
                    UpdatePaddleFromDownMotion(rotateTo);
                    break;
            }

            mShowDir.SetEndPoints(Center, Center + (3 * FrontDirection), 0.8f);
            mShowDir.Color = Color.Black;

            return ballDead;
        }

        private bool UpdateCollisionWithBall(MyBall ball)
        {
            bool collided = (ball.IsInAutoDrawSet()) &&  Collided(ball);
            if (collided)
                ball.RemoveFromAutoDrawSet();
            return collided;
        }

        private void UpdatePaddleFromRest(float angle)
        {
            if (angle == 0f)
                return;

            RotateAngle = angle;
            mCurrentPaddleState = PaddleState.PaddleReturning;
        }


        /// <summary>
        ///  
        /// </summary>
        /// <param name="angle">Newly requested angle from the user</param>
        private void UpdatePaddleFromDownMotion(float angle)
        {

            bool newAngle = false;

            if (angle != 0f)
            {
                /// if newly requested angle is more than the current angle setting, we should follow the new one
                if (ShouldSetAngle(angle))
                {
                    RotateAngle = angle;
                    newAngle = true;
                }

            }

            /// if user has set new angle, do not start returning until the next update
            /// this means, if user's trigger is hold in the middle, we will not start returning
            /// this avoid the "shaky paddle" problem.
            if (!newAngle)
            {
                if (NotAtRestPosition())
                {
                    UpdateRotateAngle();
                }
            }
        }

        protected float InitialPosition() { return kLeftRestPosition; }
        protected float RotatedAngle(float userInput) { return kLeftRestPosition + (userInput * kAngleDisplacement); }

        protected bool ShouldSetAngle(float angle)
        {
            return (angle >= RotateAngle);
        }

        protected bool NotAtRestPosition()
        {
            return RotateAngle > kLeftRestPosition;
        }

        protected void UpdateRotateAngle()
        {
            RotateAngle -= kRotateDownSpeed;
            if (RotateAngle < kLeftRestPosition)
            {
                RotateAngle = kLeftRestPosition;
                mCurrentPaddleState = PaddleState.PaddleAtRest;
            }
        }
    }
}
