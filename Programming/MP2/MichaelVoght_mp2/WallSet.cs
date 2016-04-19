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
 * WallSet.cs:          Contains all instance of walls so they are easier to handle, draw, and update.
 *                      No implementation or removing walls from the set as the game doesn't remove
 *                      them.
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
