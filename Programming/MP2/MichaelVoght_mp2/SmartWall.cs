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

        private const float mWallLength = 3f;
        private const float mWallWidth = 30f;
        private Vector2 mInitPos;
        private float mCurrentSpeed;
        private float mCurrentDisplacement;
        private bool mCurrentDirIsUp;
        private bool mCurrentDirIsRight;

        private WallState mCurrentState;

        public SmartWall(Vector2 pos) : base(pos)
        {
            SizeX = mWallLength;
            SizeY = mWallWidth;
            mInitPos = pos;
            Color = Color.Blue;

            mCurrentState = WallState.Ambient;
            mCurrentSpeed = 5f / 40f;
            mCurrentDisplacement = 5f;
            mCurrentDirIsUp = true;
            mCurrentDirIsRight = true;
        }

        override public void Update(Hero hero, Bee bee)
        {
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
        }

        private void UpdateAmbientState(Hero hero, Bee bee)
        {
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

            if (getDistance(hero) < mWallWidth * 0.7f)
            {
                mCurrentSpeed = 10f / 40f;
                mCurrentDisplacement = 10f;
                mCurrentState = WallState.Angry;
            }

            if(bee.myChaser.Visible)
            {
                mCurrentState = WallState.HasChaser;
            }
        }

        private void UpdateAngryState(Hero hero, Bee bee)
        {
            if (Collided(hero))
            {
                if(hero.CenterX < CenterX)
                    hero.CenterX = CenterX - (hero.Radius + mWallLength / 2);
                else
                    hero.CenterX = CenterX + (hero.Radius + mWallLength / 2);
            }

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

            if (getDistance(hero) > mWallWidth * 0.7f)
            {
                mCurrentSpeed = 5f / 40f;
                mCurrentDisplacement = 5f;
                mCurrentState = WallState.Ambient;
            }

            if (bee.myChaser.Visible)
            {
                mCurrentState = WallState.HasChaser;
            }
        }

        private void UpdateHasChaserState(Hero hero, Bee bee)
        {
            base.Update(hero, bee);

            if(!bee.myChaser.Visible)
            {
                mCurrentState = WallState.Ambient;
            }
        }

        private double getDistance(Hero hero)
        {
            double x1 = CenterX;
            double y1 = CenterY;
            double x2 = hero.CenterX;
            double y2 = hero.CenterY;

            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
    }
}
