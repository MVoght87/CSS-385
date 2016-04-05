/*
 * Author:              Michael Voght
 * Date:                April 7th, 2016
 * Content Mentions:    - http://www.freepik.com/free-vector/video-game-background_711818.htm Designed by Freepik
 *                      - Reformat Kevin MacLeod (incompetech.com)
 *                        Licensed under Creative Commons: By Attribution 3.0 License
 *                        http://creativecommons.org/licenses/by/3.0/
 * ExtremePaddle.cs:    Base class for a basic game related to brick breaker. This clss sets up the world
 *                      coordinates, background, and objects that the game uses while handling some inputs.
 */
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
        #region Initialization of instances and variables
        private const float kWorldWidth = 100f;
        private int Score = 0;
        private bool start = false; // switches after very first update

        Ball mMyBall;
        Paddle mMyPaddle;
        Brick[] CSS;
        Brick[] Three85;
        XNACS1PrimitiveSet mBricks;
        #endregion

        public ExtremePaddle()
        {
        }

        protected override void InitializeWorld()
        {
            World.SetWorldCoordinate(new Vector2(0f, 0f), kWorldWidth);
            World.SetBackgroundTexture("BackGround");
            EchoToTopStatus("");
            PlayBackgroundAudio("Reformat", 0.25f);
            mMyPaddle = new Paddle();
            CSS = new Brick[41];
            Three85 = new Brick[42];
            
            mBricks = new XNACS1PrimitiveSet();

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

            #region Paddle Update
            if (mMyPaddle != null)
            {
                mMyPaddle.updatePaddle(
                    GamePad.ButtonXClicked(),
                    GamePad.ButtonBClicked(),
                    GamePad.ThumbSticks.Right);
                World.ClampAtWorldBound(mMyPaddle.mPaddle);
            }
            #endregion

            #region Ball Update
            if (mMyBall != null) // don't update unless instance of ball exists
            {

                mMyBall.updateBall(
                    GamePad.Buttons.LeftShoulder,
                    GamePad.Buttons.RightShoulder);

                bool collided = mMyBall.mBall.Collided(mMyPaddle.mPaddle);
                if (collided)
                {
                    Score++;
                    PlayACue("collision");
                    mMyBall.PaddleCollide(mMyPaddle);
                }

                if (mBricks.SetSize() > 0)
                {
                    if (start)
                    {
                        for (int i = 0; i < CSS.Length; i++)
                        {
                            if (CSS[i].mBrick != null)
                            {
                                collided = mMyBall.mBall.Collided(CSS[i].mBrick);
                                if (collided)
                                {
                                    PlayACue("collision");
                                    mMyBall.BrickCollide(CSS[i]);
                                    CSS[i].DamageBrick();
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < Three85.Length; i++)
                        {
                            if (Three85[i].mBrick != null)
                            {
                                collided = mMyBall.mBall.Collided(Three85[i].mBrick);
                                if (collided)
                                {
                                    PlayACue("collision");
                                    mMyBall.BrickCollide(Three85[i]);
                                    Three85[i].DamageBrick();
                                }
                            }
                        }
                    }
                }

                BoundCollideStatus collideStatus = World.CollideWorldBound(mMyBall.mBall);

                switch (collideStatus)
                {
                    case BoundCollideStatus.CollideBottom:
                        PlayACue("loseball");
                        Score = 0;
                        mMyBall.DestroyBall();
                        mMyBall = null;
                        break;
                    case BoundCollideStatus.CollideTop:
                        PlayACue("collision");
                        World.ClampAtWorldBound(mMyBall.mBall);
                        mMyBall.WallCollide(true);
                        EchoToTopStatus(collideStatus.ToString());
                        break;
                    case BoundCollideStatus.CollideLeft:
                    case BoundCollideStatus.CollideRight:
                        PlayACue("collision");
                        World.ClampAtWorldBound(mMyBall.mBall);
                        mMyBall.WallCollide(false);
                        EchoToTopStatus(collideStatus.ToString());
                        break;
                }
            }
            #endregion

            if (mBricks.SetSize() > 0)
            {
                if (start)
                {
                    for (int i = 0; i < CSS.Length; i++)
                    {
                        if (CSS[i] != null)
                        {
                            CSS[i].updateBrick();
                            if (CSS[i].mTargetedForRemoval) // check if update made brick null
                            {
                                mBricks.RemoveFromSet(CSS[i].mBrick);
                                CSS[i].DestroyBrick();
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < Three85.Length; i++)
                    {
                        if (Three85[i] != null)
                        {
                            Three85[i].updateBrick();
                            if (Three85[i].mTargetedForRemoval) // check if update made brick null
                            {
                                mBricks.RemoveFromSet(Three85[i].mBrick);
                                Three85[i].DestroyBrick();
                            }
                        }
                    }
                }
            }
            else
            {
                start = !start;
                PopulateBricks();
            }

                EchoToBottomStatus("Score: " + Score.ToString());
        }

        private void PopulateBricks()
        {
            if (start)
            {
                #region CSS Bricks
                // C
                CSS[0] = new Brick(new Vector2(26f, 50f), RandomInt(1, 4));
                CSS[1] = new Brick(new Vector2(18f, 50f), RandomInt(1, 4));
                CSS[2] = new Brick(new Vector2(10f, 50f), RandomInt(1, 4));
                CSS[3] = new Brick(new Vector2(26f, 48f), RandomInt(1, 4));
                CSS[4] = new Brick(new Vector2(18f, 48f), RandomInt(1, 4));
                CSS[5] = new Brick(new Vector2(10f, 48f), RandomInt(1, 4));
                CSS[6] = new Brick(new Vector2(10f, 46f), RandomInt(1, 4));
                CSS[7] = new Brick(new Vector2(10f, 44f), RandomInt(1, 4));
                CSS[8] = new Brick(new Vector2(10f, 42f), RandomInt(1, 4));
                CSS[9] = new Brick(new Vector2(10f, 40f), RandomInt(1, 4));
                CSS[10] = new Brick(new Vector2(18f, 40f), RandomInt(1, 4));
                CSS[11] = new Brick(new Vector2(26f, 40f), RandomInt(1, 4));
                CSS[12] = new Brick(new Vector2(10f, 38f), RandomInt(1, 4));
                CSS[13] = new Brick(new Vector2(18f, 38f), RandomInt(1, 4));
                CSS[14] = new Brick(new Vector2(26f, 38f), RandomInt(1, 4));

                // S
                CSS[15] = new Brick(new Vector2(58f, 50f), RandomInt(1, 4));
                CSS[16] = new Brick(new Vector2(50f, 50f), RandomInt(1, 4));
                CSS[17] = new Brick(new Vector2(42f, 50f), RandomInt(1, 4));
                CSS[18] = new Brick(new Vector2(42f, 48f), RandomInt(1, 4));
                CSS[19] = new Brick(new Vector2(42f, 46f), RandomInt(1, 4));
                CSS[20] = new Brick(new Vector2(42f, 44f), RandomInt(1, 4));
                CSS[21] = new Brick(new Vector2(50f, 44f), RandomInt(1, 4));
                CSS[22] = new Brick(new Vector2(58f, 44f), RandomInt(1, 4));
                CSS[23] = new Brick(new Vector2(58f, 42f), RandomInt(1, 4));
                CSS[24] = new Brick(new Vector2(58f, 40f), RandomInt(1, 4));
                CSS[25] = new Brick(new Vector2(58f, 38f), RandomInt(1, 4));
                CSS[26] = new Brick(new Vector2(50f, 38f), RandomInt(1, 4));
                CSS[27] = new Brick(new Vector2(42f, 38f), RandomInt(1, 4));

                // S
                CSS[28] = new Brick(new Vector2(90f, 50f), RandomInt(1, 4));
                CSS[29] = new Brick(new Vector2(82f, 50f), RandomInt(1, 4));
                CSS[30] = new Brick(new Vector2(74f, 50f), RandomInt(1, 4));
                CSS[31] = new Brick(new Vector2(74f, 48f), RandomInt(1, 4));
                CSS[32] = new Brick(new Vector2(74f, 46f), RandomInt(1, 4));
                CSS[33] = new Brick(new Vector2(74f, 44f), RandomInt(1, 4));
                CSS[34] = new Brick(new Vector2(82f, 44f), RandomInt(1, 4));
                CSS[35] = new Brick(new Vector2(90f, 44f), RandomInt(1, 4));
                CSS[36] = new Brick(new Vector2(90f, 42f), RandomInt(1, 4));
                CSS[37] = new Brick(new Vector2(90f, 40f), RandomInt(1, 4));
                CSS[38] = new Brick(new Vector2(90f, 38f), RandomInt(1, 4));
                CSS[39] = new Brick(new Vector2(82f, 38f), RandomInt(1, 4));
                CSS[40] = new Brick(new Vector2(74f, 38f), RandomInt(1, 4));

                for (int i = 0; i < CSS.Length; i++)
                    mBricks.AddToSet(CSS[i].mBrick);
                #endregion

            } else
            {
                #region 385 Bricks
                // 3
                Three85[0] = new Brick(new Vector2(26f, 50f), RandomInt(1, 4));
                Three85[1] = new Brick(new Vector2(18f, 50f), RandomInt(1, 4));
                Three85[2] = new Brick(new Vector2(10f, 50f), RandomInt(1, 4));
                Three85[3] = new Brick(new Vector2(26f, 48f), RandomInt(1, 4));
                Three85[4] = new Brick(new Vector2(26f, 46f), RandomInt(1, 4));
                Three85[5] = new Brick(new Vector2(26f, 44f), RandomInt(1, 4));
                Three85[6] = new Brick(new Vector2(18f, 44f), RandomInt(1, 4));
                Three85[7] = new Brick(new Vector2(26f, 42f), RandomInt(1, 4));
                Three85[8] = new Brick(new Vector2(26f, 40f), RandomInt(1, 4));
                Three85[9] = new Brick(new Vector2(26f, 38f), RandomInt(1, 4));
                Three85[10] = new Brick(new Vector2(18f, 38f), RandomInt(1, 4));
                Three85[11] = new Brick(new Vector2(10f, 38f), RandomInt(1, 4));

                // 8
                Three85[12] = new Brick(new Vector2(58f, 50f), RandomInt(1, 4));
                Three85[13] = new Brick(new Vector2(50f, 50f), RandomInt(1, 4));
                Three85[14] = new Brick(new Vector2(42f, 50f), RandomInt(1, 4));
                Three85[15] = new Brick(new Vector2(42f, 48f), RandomInt(1, 4));
                Three85[16] = new Brick(new Vector2(42f, 46f), RandomInt(1, 4));
                Three85[17] = new Brick(new Vector2(42f, 44f), RandomInt(1, 4));
                Three85[18] = new Brick(new Vector2(58f, 48f), RandomInt(1, 4));
                Three85[19] = new Brick(new Vector2(58f, 46f), RandomInt(1, 4));
                Three85[20] = new Brick(new Vector2(58f, 44f), RandomInt(1, 4));
                Three85[21] = new Brick(new Vector2(50f, 44f), RandomInt(1, 4));
                Three85[22] = new Brick(new Vector2(42f, 42f), RandomInt(1, 4));
                Three85[23] = new Brick(new Vector2(42f, 40f), RandomInt(1, 4));
                Three85[24] = new Brick(new Vector2(42f, 38f), RandomInt(1, 4));
                Three85[25] = new Brick(new Vector2(58f, 42f), RandomInt(1, 4));
                Three85[26] = new Brick(new Vector2(58f, 40f), RandomInt(1, 4));
                Three85[27] = new Brick(new Vector2(58f, 38f), RandomInt(1, 4));
                Three85[28] = new Brick(new Vector2(50f, 38f), RandomInt(1, 4));

                // 5
                Three85[29] = new Brick(new Vector2(90f, 50f), RandomInt(1, 4));
                Three85[30] = new Brick(new Vector2(82f, 50f), RandomInt(1, 4));
                Three85[31] = new Brick(new Vector2(74f, 50f), RandomInt(1, 4));
                Three85[32] = new Brick(new Vector2(74f, 48f), RandomInt(1, 4));
                Three85[33] = new Brick(new Vector2(74f, 46f), RandomInt(1, 4));
                Three85[34] = new Brick(new Vector2(74f, 44f), RandomInt(1, 4));
                Three85[35] = new Brick(new Vector2(82f, 44f), RandomInt(1, 4));
                Three85[36] = new Brick(new Vector2(90f, 44f), RandomInt(1, 4));
                Three85[37] = new Brick(new Vector2(90f, 42f), RandomInt(1, 4));
                Three85[38] = new Brick(new Vector2(90f, 40f), RandomInt(1, 4));
                Three85[39] = new Brick(new Vector2(90f, 38f), RandomInt(1, 4));
                Three85[40] = new Brick(new Vector2(82f, 38f), RandomInt(1, 4));
                Three85[41] = new Brick(new Vector2(74f, 38f), RandomInt(1, 4));

                for (int i = 0; i < Three85.Length; i++)
                    mBricks.AddToSet(Three85[i].mBrick);
                #endregion
            }
        }
    }
}
