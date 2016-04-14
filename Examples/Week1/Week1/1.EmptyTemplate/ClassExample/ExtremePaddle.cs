#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

using XNACS1Lib;

namespace MichaelVoght_NameSpace
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ExtremePaddle : XNACS1Base
    {
        private const float kWorldWidth = 100f;
        private int Score = 0;

        Ball mMyBall;
        Paddle mMyPaddle;
        
        public ExtremePaddle()
        {
        }

        protected override void InitializeWorld()
        {
            mMyPaddle = new Paddle();
            World.SetWorldCoordinate(new Vector2(0f, 0f), kWorldWidth);
            EchoToTopStatus("");
        }


        protected override void UpdateWorld()
        {
            if (GamePad.Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (GamePad.ButtonAClicked() && mMyBall == null)
            {
                PlayACue("powerup");
                mMyBall = new Ball((World.WorldMax + World.WorldMin) / 2f, RandomFloat(0.5f, 2f));
            }

            if (mMyPaddle != null)
            {
                mMyPaddle.updatePaddle(
                    GamePad.ButtonXClicked(), 
                    GamePad.ButtonBClicked(),
                    GamePad.ThumbSticks.Right);
                World.ClampAtWorldBound(mMyPaddle.mPaddle);
            }

            if(mMyBall != null)
            {

                mMyBall.updateBall(
                    GamePad.Buttons.LeftShoulder,
                    GamePad.Buttons.RightShoulder);
                bool collided = mMyBall.mBall.Collided(mMyPaddle.mPaddle);
                if (collided)
                {
                    Score++;
                    mMyBall.PaddleCollide(mMyPaddle);
                }

                BoundCollideStatus collideStatus = World.CollideWorldBound(mMyBall.mBall);

                switch (collideStatus)
                {
                    case BoundCollideStatus.CollideBottom:
                        Score = 0;
                        mMyBall.DestroyBall();
                        mMyBall = null;
                        break;
                    case BoundCollideStatus.CollideTop:
                        World.ClampAtWorldBound(mMyBall.mBall);
                        mMyBall.WallCollide(true);
                        EchoToTopStatus(collideStatus.ToString());
                        break;
                    case BoundCollideStatus.CollideLeft:
                    case BoundCollideStatus.CollideRight:
                        World.ClampAtWorldBound(mMyBall.mBall);
                        mMyBall.WallCollide(false);
                        EchoToTopStatus(collideStatus.ToString());
                        break;
                }
            }

            EchoToBottomStatus("Score: " + Score.ToString());
        }
    }
}
