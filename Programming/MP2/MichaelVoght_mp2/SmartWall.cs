/*
 * Author:              Michael Voght
 * Date:                April 14th, 2016
 * Content Mentions:    - Bug: http://opengameart.org/content/parts-2-art-spider
 *                      - Smart Walls: http://opengameart.org/content/32-x-32-bricks
 *                      - Background: http://opengameart.org/content/country-side-platform-tiles
 *                      - Wall: http://opengameart.org/content/wall-0
 *                      - Chaser: http://opengameart.org/content/ufo-enemy-game-character
 *                      - Hero: http://opengameart.org/content/sorcerer
 *                      - Reformat Kevin MacLeod (incompetech.com)
 *                        Licensed under Creative Commons: By Attribution 3.0 License
 *                        http://creativecommons.org/licenses/by/3.0/
 * SmartWall.cs:        Inherits from wall, as it's a wall with additional features. Has 3 states
 *                      one state that moves up and down when the player is far enough away from the
 *                      wall, one that moves left and right and pushing the player if they're to
 *                      close, and one that freezes when a chaser spawns. The player cannot pass
 *                      through the walls.
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
    public class SmartWall : Wall
    {
        protected enum WallState
        {
            Ambient,
            Angry,
            HasChaser
        }
        #region Instance Variables for Smart Walls
        private const float mWallLength = 3f;
        private const float mWallWidth = 30f;
        private Vector2 mInitPos;
        private float mCurrentSpeed;
        private float mCurrentDisplacement;
        private bool mCurrentDirIsUp;
        private bool mCurrentDirIsRight;

        private WallState mCurrentState;
        #endregion

        public SmartWall(Vector2 pos) : base(pos)
        {
            SizeX = mWallLength;
            SizeY = mWallWidth;
            mInitPos = pos;
            Texture = "BrickGrey";

            mCurrentState = WallState.Ambient;
            mCurrentSpeed = 5f / 40f;
            mCurrentDisplacement = 5f;
            mCurrentDirIsUp = true;
            mCurrentDirIsRight = true;
        }

        override public void Update(Hero hero, Bee bee)
        {
            #region Handle Current State Update
            switch (mCurrentState)
            {
                case WallState.Ambient:
                    UpdateAmbientState(hero, bee);
                    break;
                case WallState.Angry:
                    UpdateAngryState(hero, bee);
                    break;
                case WallState.HasChaser:
                    UpdateHasChaserState(hero, bee);
                    break;
            }
            #endregion
        }

        private void UpdateAmbientState(Hero hero, Bee bee)
        {
            #region Move Up and Down
            if (mCurrentDirIsUp)
            {
                if (CenterY - mInitPos.Y < mCurrentDisplacement)
                    CenterY += mCurrentSpeed;
                else
                    mCurrentDirIsUp = false;
            }
            else
            {
                if (CenterY - mInitPos.Y > -mCurrentDisplacement)
                    CenterY -= mCurrentSpeed;
                else
                    mCurrentDirIsUp = true;
            }
            #endregion

            #region Change to Angry when Player gets to close to Walls
            if (getDistance(hero) < mWallWidth * 0.7f)
            {
                Texture = "BrickFireRed";
                mCurrentSpeed = 10f / 40f;
                mCurrentDisplacement = 10f;
                mCurrentState = WallState.Angry;
            }
            #endregion

            #region Change to HasChaser when Player gets to close to Bee
            if (bee.myChaser.Visible)
            {
                Texture = "BrickLightGrey";
                mCurrentState = WallState.HasChaser;
            }
            #endregion
        }

        private void UpdateAngryState(Hero hero, Bee bee)
        {
            #region Wall pushes player
            if (Collided(hero))
            {
                if(RightOf(hero))
                    hero.CenterX = CenterX - (hero.Radius + mWallLength / 2);
                else
                    hero.CenterX = CenterX + (hero.Radius + mWallLength / 2);
            }
            #endregion

            #region Move Left and Right
            if (mCurrentDirIsRight)
            {
                if (CenterX - mInitPos.X < mCurrentDisplacement)
                    CenterX += mCurrentSpeed;
                else
                    mCurrentDirIsRight = false;
            }
            else
            {
                if (CenterX - mInitPos.X > -mCurrentDisplacement)
                    CenterX -= mCurrentSpeed;
                else
                    mCurrentDirIsRight = true;
            }
            #endregion

            #region Change to Ambient if Player moves far enough away from Wall
            if (getDistance(hero) > mWallWidth * 0.7f)
            {
                Texture = "BrickGrey";
                mCurrentSpeed = 5f / 40f;
                mCurrentDisplacement = 5f;
                mCurrentState = WallState.Ambient;
            }
            #endregion

            #region Change to HasChaser of Player gets to close to Bee
            if (bee.myChaser.Visible)
            {
                Texture = "BrickLightGrey";
                mCurrentState = WallState.HasChaser;
            }
            #endregion
        }

        private void UpdateHasChaserState(Hero hero, Bee bee)
        {
            base.Update(hero, bee);

            #region Change to Ambient once Chaser dissapears
            if(!bee.myChaser.Visible)
            {
                Texture = "BrickGrey";
                mCurrentState = WallState.Ambient;
            }
            #endregion
        }

        private double getDistance(Hero hero)
        {
            #region Variables for (x1, y1) and (x2, y2)
            double x1 = CenterX;
            double y1 = CenterY;
            double x2 = hero.CenterX;
            double y2 = hero.CenterY;
            #endregion

            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
    }
}
