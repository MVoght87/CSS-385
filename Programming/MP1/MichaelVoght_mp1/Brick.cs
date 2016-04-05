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
    public class Brick
    {
        public XNACS1Rectangle mBrick = null;
        private Vector2 pos;
        private const float mBrickHeight = 2f;
        private const float mBrickWidth = 8f;
        private int life;
        public bool mTargetedForRemoval = false;

        public Brick(Vector2 pos, int hp)
        {
            CreateBrick(pos, hp);
        }

        public void DamageBrick()
        {
            life -= 1;
        }

        public void updateBrick()
        {
            switch (life)
            {
                case 0:
                    if(mBrick != null)
                        mTargetedForRemoval = true;
                    break;
                case 1:
                    mBrick.Color = Color.Blue;
                    break;
                case 2:
                    mBrick.Color = Color.Green;
                    break;
                case 3:
                    mBrick.Color = Color.Orange;
                    break;
                default:
                    break;
            }
        }

        private void CreateBrick(Vector2 pos, int hp)
        {
            mBrick = new XNACS1Rectangle(pos, mBrickWidth, mBrickHeight);
            life = hp;
        }

        public void DestroyBrick()
        {
            mTargetedForRemoval = false; // no longer needed to be removed
            mBrick = null;
        }
    }
}
