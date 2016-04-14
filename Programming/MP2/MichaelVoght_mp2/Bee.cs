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
    public class Bee : XNACS1Circle
    {
        protected enum BeeState
        {
            Patrol,
            Confused
        }

        public Chaser myChaser;

        private const float kBeeSize = 1.5f;
        private const float kBeeSpeed = 0.5f;
        private const float mPeriod = 4;
        private float mAmplitude;
        private float mFrequencyScale;
        private bool mPatrolRight;
        private Vector2 mBeePosition = new Vector2(XNACS1Base.World.WorldDimension.X, XNACS1Base.World.WorldDimension.Y / 2);
        private Vector2 mInitPos;
        private BeeState mCurrentState;

        public Bee()
        {
            myChaser = new Chaser();

            Radius = kBeeSize;
            ShouldTravel = false;
            mPatrolRight = true;

            Center = RandomPosition();
            mInitPos = Center;
            mCurrentState = BeeState.Patrol;

            Speed = kBeeSpeed;
            ComputeFrequencyScale();
            mAmplitude = XNACS1Base.World.WorldDimension.Y * XNACS1Base.RandomFloat(0.18f, 0.22f);
        }

        public void Update(Hero mHero)
        {
            myChaser.Update(mHero);
            switch(mCurrentState)
            {
                case BeeState.Patrol:
                    UpdatePatrolState(mHero);
                    break;
                case BeeState.Confused:
                    UpdateConfusedState(mHero);
                    break;
            }
        }

        private Vector2 RandomPosition()
        {
            float randX = XNACS1Base.World.WorldDimension.X * XNACS1Base.RandomFloat(0.20f, 0.80f);
            float randY = XNACS1Base.World.WorldDimension.Y * XNACS1Base.RandomFloat(0.25f, 0.75f);
            return new Vector2(randX, randY);
        }

        private void ComputeFrequencyScale()
        {
            mFrequencyScale = mPeriod * 2f * (float)(Math.PI) / XNACS1Base.World.WorldDimension.X;
        }

        private void UpdatePatrolState(Hero hero)
        {
            BoundCollideStatus collideStatus = XNACS1Base.World.CollideWorldBound(this);
            if ((collideStatus == BoundCollideStatus.CollideRight) || (collideStatus == BoundCollideStatus.CollideLeft))
                mPatrolRight = !mPatrolRight;
            if (!mPatrolRight)
            {
                CenterX -= Speed;
            }
            else
            {
                CenterX += Speed;
            }
            CenterY = mInitPos.Y + getYValue(CenterX);

            if(getDistance(hero) < 5f)
            {
                myChaser.Visible = true;
                ShouldTravel = true;
                mCurrentState = BeeState.Confused;
            }
        }

        private void UpdateConfusedState(Hero hero)
        {
            VelocityDirection = dirChange();
            if (getDistance(hero) > 5f)
            {
                ShouldTravel = false;
                mCurrentState = BeeState.Patrol;
            }
        }

        private float getYValue(float x)
        {
            return mAmplitude * (float)(Math.Sin(x * mFrequencyScale));
        }

        private Vector2 dirChange()
        {
            float randX = XNACS1Base.RandomFloat(-1f, 1f);
            float randY = XNACS1Base.RandomFloat(-1f, 1f);

            return new Vector2(randX, randY);
        }

        private double getDistance(Hero hero)
        {
            double x1 = CenterX;
            double y1 = CenterY;
            double x2 = hero.CenterX;
            double y2 = hero.CenterY;

            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2)) - (Radius + hero.Radius);
        }
    }
}
