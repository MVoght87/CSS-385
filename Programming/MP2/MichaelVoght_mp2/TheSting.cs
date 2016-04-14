/*
 * Author:              Michael Voght
 * Date:                April 7th, 2016
 * Content Mentions:    - http://www.freepik.com/free-vector/video-game-background_711818.htm Designed by Freepik
 *                      - Reformat Kevin MacLeod (incompetech.com)
 *                        Licensed under Creative Commons: By Attribution 3.0 License
 *                        http://creativecommons.org/licenses/by/3.0/
 * ExtremePaddle.cs:    Base class for a basic game related to brick breaker. This clss sets up the world
 *                      coordinates, background, and objects that the game uses while handling some inputs.
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
            myHero = new Hero();
            myBee = new Bee();
            myWalls = new WallSet(7, myBee);
            myWalls.AddToSet(new Vector2(mWorldLength / 6, World.WorldDimension.Y * RandomFloat(0.35f, 0.65f)), true);
            myWalls.AddToSet(new Vector2(2 * mWorldLength / 6, World.WorldDimension.Y * RandomFloat(0.35f, 0.65f)), true);
            myWalls.AddToSet(new Vector2(3 * mWorldLength / 6, World.WorldDimension.Y * RandomFloat(0.35f, 0.65f)), true);
            myWalls.AddToSet(new Vector2(4 * mWorldLength / 6, World.WorldDimension.Y * RandomFloat(0.35f, 0.65f)), true);
            myWalls.AddToSet(new Vector2(5 * mWorldLength / 6, World.WorldDimension.Y * RandomFloat(0.35f, 0.65f)), true);
            myWalls.AddToSet(new Vector2(mWorldLength / 2, 3 * World.WorldDimension.Y / 4), false);
            myWalls.AddToSet(new Vector2(mWorldLength / 2, World.WorldDimension.Y / 4), false);
        }


        protected override void UpdateWorld()
        {
            myHero.Update();
            myBee.Update(myHero);
            myWalls.Update(myHero);
        }

    }
}
