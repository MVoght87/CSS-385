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
    public class WallSet
    {

        private Wall[] mWalls;
        private Bee mBee;
        private int mSizeOfWallSet;
        private int mWallCount;

        public WallSet(int num, Bee bee)
        {
            mWallCount = 0;
            mSizeOfWallSet = num;
            mWalls = new Wall[mSizeOfWallSet];
            mBee = bee;
        }

        public void Update(Hero hero)
        {
            for (int i = 0; i < mWallCount; i++)
            {
                if (mWalls[i] != null)
                    mWalls[i].Update(hero, mBee);
            }
        }

        public void AddToSet(Vector2 pos, bool SmartWall)
        {
            if (mWallCount < mSizeOfWallSet)
            {
                if (!SmartWall)
                    mWalls[mWallCount] = new Wall(pos);
                else
                    mWalls[mWallCount] = new SmartWall(pos);
                mWallCount++;
            }
        }

        public int Size()
        {
            return mWallCount;
        }

        public Wall indexAt(int i)
        {
            return mWalls[i];
        }
    }
}
