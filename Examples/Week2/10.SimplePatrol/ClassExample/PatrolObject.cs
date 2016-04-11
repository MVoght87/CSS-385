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
    class PatrolObject : XNACS1Circle
    {
        protected enum PatrolState {
            TopLeftRegion,         
            TopRightRegion,
            BottomLeftRegion,
            BottomRightRegion
        }
        private const float kPatrolSpeed = 25f / 40f; // 25 units per 40 ticks, 
                                                // assuming 40 ticks per second, this is about 25 units per second

        private PatrolState mCurrentState;  // current state
        private Vector2 mTargetPosition;    // This it the target we are moving towards
        
        public PatrolObject()
        {
            Radius = 2f;
            ShouldTravel = true;

            Center = RandomTopLeftPosition();
            mCurrentState = PatrolState.TopLeftRegion;

            mTargetPosition = RandomTopRightPosition();
            ComputePositionAndVelocity();
        }



        public void Update()
        {
            switch (mCurrentState)
            {
                case PatrolState.BottomLeftRegion:
                    UpdateBottomLeftState();
                    break;

                case PatrolState.BottomRightRegion:
                    UpdateBottomRightState();
                    break;

                case PatrolState.TopRightRegion:
                    UpdateTopRightState();
                    break;

                case PatrolState.TopLeftRegion:
                    UpdateTopLeftState();
                    break;
            }
            // if there are common operations, we can perform the operation here ...
            
        }


        private void UpdateBottomLeftState()
        {
            Byte r = Color.R;
            r--;
            Color = new Color(r, Color.G, Color.B);

            if ((Center - mTargetPosition).Length() < 1f) {
                mCurrentState = PatrolState.BottomRightRegion;
                mTargetPosition = RandomBottomRightPosition();
                ComputePositionAndVelocity();
            }
        }

        private void UpdateBottomRightState()
        {
            Byte g = Color.G;
            g--;
            Color = new Color(Color.R, g, Color.B);
            if ((Center - mTargetPosition).Length() < 1f)
            {
                mCurrentState = PatrolState.TopRightRegion;
                mTargetPosition = RandomTopRightPosition();
                ComputePositionAndVelocity();
            }
        }

        private void UpdateTopRightState()
        {
            Byte g = Color.G;
            g++;
            Color = new Color(Color.R, g, Color.B);
            if ((Center - mTargetPosition).Length() < 1f)
            {
                mCurrentState = PatrolState.TopLeftRegion;
                mTargetPosition = RandomTopLeftPosition();
                ComputePositionAndVelocity();
            }
        }


        private void UpdateTopLeftState()
        {
            Byte r = Color.R;
            r++;
            Color = new Color(r, Color.G, Color.B);
            if ((Center - mTargetPosition).Length() < 1f)
            {
                mCurrentState = PatrolState.BottomLeftRegion;
                mTargetPosition = RandomBottomLeftPosition();
                ComputePositionAndVelocity();
            }
        }


        private void ComputePositionAndVelocity()
        {
            Vector2 toNextPosition = mTargetPosition - Center;    // this is the vector from Center to the next Position
            float distantToNextPosition = toNextPosition.Length();
            float speed = kPatrolSpeed * XNACS1Base.RandomFloat(0.8f, 1.2f); // speed has 20% randomness

            toNextPosition.Normalize();
            Velocity = speed * toNextPosition;
        }

        #region compute random position in one of the 4 regions
        private Vector2 GetRandomWorldPosition()
        {
            float x = XNACS1Base.World.WorldDimension.X * XNACS1Base.RandomFloat(0.02f, 0.48f);
            float y = XNACS1Base.World.WorldDimension.Y * XNACS1Base.RandomFloat(0.02f, 0.48f);
            return new Vector2(x, y);
        }
        
        private Vector2 RandomBottomRightPosition()
        {
            Vector2 p = GetRandomWorldPosition();
            return new Vector2(XNACS1Base.World.WorldMax.X - p.X, XNACS1Base.World.WorldMin.Y + p.Y);
        }

        private Vector2 RandomBottomLeftPosition()
        {
            return XNACS1Base.World.WorldMin + GetRandomWorldPosition();
        }

        private Vector2 RandomTopRightPosition()
        {
            return XNACS1Base.World.WorldMax - GetRandomWorldPosition();
        }

        private Vector2 RandomTopLeftPosition()
        {
            Vector2 p = GetRandomWorldPosition();
            return new Vector2(XNACS1Base.World.WorldMin.X + p.X, XNACS1Base.World.WorldMax.Y - p.Y);
        }
        #endregion 
    }
}
