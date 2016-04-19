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
 * TheSting.cs:         Base class for a basic game. This clss sets up the world coordinates, background, 
 *                      and objects that the game uses.
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
    public class TheSting : XNACS1Base
    {
        private Hero myHero;
        private Bee myBee;
        private WallSet myWalls;

        private float mWorldLength = 100f;

        public TheSting()
        {
        }

        protected override void InitializeWorld()
        {
            World.SetWorldCoordinate(new Vector2(0f, 0f), mWorldLength);
            World.SetBackgroundTexture("country-platform-preview");
            PlayBackgroundAudio("Reformat", 0.25f);

            #region Create Objects for Game World
            myHero = new Hero();
            myBee = new Bee();
            myWalls = new WallSet(7, myBee);
            myWalls.AddToSet(new Vector2(mWorldLength / 6, World.WorldDimension.Y * RandomFloat(0.35f, 0.65f)), true);
            myWalls.AddToSet(new Vector2(2 * mWorldLength / 6, World.WorldDimension.Y * RandomFloat(0.30f, 0.70f)), true);
            myWalls.AddToSet(new Vector2(3 * mWorldLength / 6, World.WorldDimension.Y * RandomFloat(0.30f, 0.70f)), true);
            myWalls.AddToSet(new Vector2(4 * mWorldLength / 6, World.WorldDimension.Y * RandomFloat(0.30f, 0.70f)), true);
            myWalls.AddToSet(new Vector2(5 * mWorldLength / 6, World.WorldDimension.Y * RandomFloat(0.30f, 0.70f)), true);
            myWalls.AddToSet(new Vector2(mWorldLength / 2, 3 * World.WorldDimension.Y / 4), false);
            myWalls.AddToSet(new Vector2(mWorldLength / 2, World.WorldDimension.Y / 4), false);
            #endregion
        }


        protected override void UpdateWorld()
        {
            myHero.Update();
            myBee.Update(myHero);
            myWalls.Update(myHero);
        }

    }
}
