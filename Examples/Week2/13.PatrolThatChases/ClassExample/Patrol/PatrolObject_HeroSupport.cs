using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using XNACS1Lib;

namespace ClassExample
{
    partial class PatrolObject : XNACS1Circle
    {
        
        private const float kDistToBeginChase = 10f; // this is the distance to trigger partol chasing of hero
        

        private void DetectHero(HeroObject hero)
        {
            Vector2 toHero = hero.Center - Center;
            if (toHero.Length() < kDistToBeginChase)
            {
                mStateTimer = kStateTimer * 2; // 5 times as much time for chasing
                Speed *= 3f;                   // twice the current speed!
                mCurrentState = PatrolState.ChaseHero;
                mTargetPosition = hero.Center;
                Color = Color.Red;
            }
        }

        private bool UpdateChaseHeroState(HeroObject hero, float distToHero)
        {
            bool caught = false;

            caught = Collided(hero);
            mTargetPosition = hero.Center;

            if (caught || (mStateTimer < 0) )
            {
                if (caught)
                    hero.Caught();

                Color = new Color(XNACS1Base.RandomFloat(0f, 0.4f),
                              XNACS1Base.RandomFloat(0f, 0.4f),
                              XNACS1Base.RandomFloat(0f, 0.4f));
                // now tansit out of current state ...
                Vector2 midPt = 0.5f * (XNACS1Base.World.WorldMax - XNACS1Base.World.WorldMin);
                if (CenterX > midPt.X)
                {
                    if (CenterY > midPt.Y)
                    {
                        mCurrentState = PatrolState.TopRightRegion;
                        mTargetPosition = RandomTopRightPosition();
                    }
                    else
                    {
                        mCurrentState = PatrolState.BottomRightRegion;
                        mTargetPosition = RandomBottomRightPosition();
                    }
                }
                else
                {
                    if (CenterY > midPt.Y)
                    {
                        mCurrentState = PatrolState.TopLeftRegion;
                        mTargetPosition = RandomTopLeftPosition();
                    }
                    else
                    {
                        mCurrentState = PatrolState.BottomLeftRegion;
                        mTargetPosition = RandomBottomLeftPosition();
                    }
                }
                mStateTimer = kStateTimer;
                ComputePositionAndVelocity();
            }
            return caught;
        }
    }
}
