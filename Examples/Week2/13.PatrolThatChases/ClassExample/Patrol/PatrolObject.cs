﻿using System;
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
        protected enum PatrolState {
            TopLeftRegion,         
            TopRightRegion,
            BottomLeftRegion,
            BottomRightRegion,
            ChaseHero
        }
        private const float kPatrolSpeed = 5f / 40f; // 5 units per 40 ticks, 
                                                // assuming 40 ticks per second, this is about 5 units per second

        private PatrolState mCurrentState;  // current state
        private Vector2 mTargetPosition;    // This it the target we are moving towards

        private const int kStateTimer = 40 * 3; // assuming 40 ticks per section, this is about three seconds
        private int mStateTimer;    // interestingly, with "gradual" velocity changing, we cannot
                                    // guarantee that we will ever rich the mTargetPosition 
                                    // (we may ended up orbiting the target), so we set a timer
                                    // when timer is up, we transit

       
        public PatrolObject()
        {
            Radius = 0.7f;
            ShouldTravel = true;
            Color = new Color(XNACS1Base.RandomFloat(0f, 0.4f), 
                              XNACS1Base.RandomFloat(0f, 0.4f), 
                              XNACS1Base.RandomFloat(0f, 0.4f));

            Center = RandomTopLeftPosition();
            mCurrentState = PatrolState.TopLeftRegion;

            mTargetPosition = RandomTopRightPosition();
            ComputePositionAndVelocity(); 
        }

        public bool ChasingHero() { return mCurrentState == PatrolState.ChaseHero; }

        public bool Update(HeroObject hero)
        {
            bool caught = false;

            // perform operation common to all states ...
            mStateTimer--;

            Vector2 toTarget = mTargetPosition - Center;
            float distToTarget = toTarget.Length();
            toTarget /= distToTarget; // this is the same as normalization
            ComputeNewDirection(toTarget);

            // operations specific to each states// operations specific to each states
            switch (mCurrentState)
            {
                case PatrolState.BottomLeftRegion:
                    UpdateBottomLeftState(hero, distToTarget);
                    break;

                case PatrolState.BottomRightRegion:
                    UpdateBottomRightState(hero, distToTarget);
                    break;

                case PatrolState.TopRightRegion:
                    UpdateTopRightState(hero, distToTarget);
                    break;

                case PatrolState.TopLeftRegion:
                    UpdateTopLeftState(hero, distToTarget);
                    break;

                case PatrolState.ChaseHero:
                    caught = UpdateChaseHeroState(hero, distToTarget);
                    break;
            }
            return caught;

        }

        private void ComputeNewDirection(Vector2 toTarget)
        {
            // figure out if we should continue to adjust our direciton ...
            double cosTheta = Vector2.Dot(toTarget, FrontDirection);
            float theta = MathHelper.ToDegrees((float)Math.Acos(cosTheta));
            if (theta > 0.001f)
            {
                Vector3 frontDir3 = new Vector3(FrontDirection, 0f);
                Vector3 toTarget3 = new Vector3(toTarget, 0f);
                Vector3 zDir = Vector3.Cross(frontDir3, toTarget3);
                RotateAngle += Math.Sign(zDir.Z) * 0.05f * theta; // rotate 5% at a time towards final direction
                VelocityDirection = FrontDirection;
            }
        }

        private void UpdateBottomLeftState(HeroObject hero, float distToTarget)
        {
            DetectHero(hero); // check if we should transit to ChaseHero

            if ((mStateTimer < 0) || (distToTarget < 5f))
            {
                mStateTimer = (int)(kStateTimer * XNACS1Base.RandomFloat(0.8f, 1.2f)); // 20% randomness
                if (XNACS1Base.RandomFloat() > 0.5f)
                {
                    mCurrentState = PatrolState.BottomRightRegion;
                    mTargetPosition = RandomBottomRightPosition();
                }
                else
                {
                    mCurrentState = PatrolState.TopLeftRegion;
                    mTargetPosition = RandomTopLeftPosition();
                }

                ComputePositionAndVelocity();
            }
        }

        private void UpdateBottomRightState(HeroObject hero, float distToTarget)
        {
            DetectHero(hero); // check if we should transit to ChaseHero

            if ((mStateTimer < 0) || (distToTarget < 5f))
            {
                mStateTimer = (int)(kStateTimer * XNACS1Base.RandomFloat(0.6f, 1.4f)); // 40% randomness
                if (XNACS1Base.RandomFloat() > 0.5f)
                {
                    mCurrentState = PatrolState.TopRightRegion;
                    mTargetPosition = RandomTopRightPosition();
                }
                else
                {
                    mCurrentState = PatrolState.BottomLeftRegion;
                    mTargetPosition = RandomBottomLeftPosition();
                }
                ComputePositionAndVelocity();
            }
        }

        private void UpdateTopRightState(HeroObject hero, float distToTarget)
        {
            DetectHero(hero); // check if we should transit to ChaseHero

            if ((mStateTimer < 0) || (distToTarget < 5f))
            {
                mStateTimer = (int)(kStateTimer * XNACS1Base.RandomFloat(0.9f, 1.1f)); // 10% randomness
                if (XNACS1Base.RandomFloat() > 0.5f)
                {
                    mCurrentState = PatrolState.TopLeftRegion;
                    mTargetPosition = RandomTopLeftPosition();
                }
                else
                {
                    mCurrentState = PatrolState.BottomRightRegion;
                    mTargetPosition = RandomBottomRightPosition();
                }
                ComputePositionAndVelocity();
            }
        }


        private void UpdateTopLeftState(HeroObject hero, float distToTarget)
        {

            DetectHero(hero); // check if we should transit to ChaseHero

            if ((mStateTimer < 0) || (distToTarget < 5f))
            {
                mStateTimer = (int)(kStateTimer * XNACS1Base.RandomFloat(0.8f, 1.2f)); // 20% randomness
                if (XNACS1Base.RandomFloat() > 0.5f)
                {
                    mCurrentState = PatrolState.BottomLeftRegion;
                    mTargetPosition = RandomBottomLeftPosition();
                }
                else
                {
                    mCurrentState = PatrolState.TopRightRegion;
                    mTargetPosition = RandomTopRightPosition();
                }
                ComputePositionAndVelocity();
            }
        }

        
        private void ComputePositionAndVelocity()
        {     
            // Change the speed without actually change velocity direction!!
            Speed = kPatrolSpeed * XNACS1Base.RandomFloat(0.8f, 1.2f); // speed has 20% randomness

            //toNextPosition.Normalize();
            // Velocity = speed * toNextPosition;
        }
    }
}
