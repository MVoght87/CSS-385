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
 * Wall.cs:             Base class for the Smart Walls. Just a generic wall with no mechanic other
 *                      than the playing not being able to pass through it.
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
    public class Wall : XNACS1Rectangle
    {
        private const float mWallLength = 70f;
        private const float mWallWidth = 3f;

        public Wall()
        {

        }

        public Wall(Vector2 pos)
        {
            SizeX = mWallLength;
            SizeY = mWallWidth;
            Center = pos;
            Texture = "00";
        }

        virtual public void Update(Hero hero, Bee bee)
        {
            if(Collided(hero))
            {
                hero.Center -= XNACS1Base.GamePad.ThumbSticks.Left;
            }

        }
    }
}
