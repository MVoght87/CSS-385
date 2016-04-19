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
 * Hero.cs:             Inherits from circle, has simple control with left stick (W, A, S, D).
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
    public class Hero : XNACS1Circle
    {
        private const float kHeroSize = 3f;
        private Vector2 mHeroPosition = new Vector2(5.0f, 5.0f);

        public Hero()
        {
            Texture = "mag";
            Radius = kHeroSize;
            ShouldTravel = false;
            Center = mHeroPosition;
        }

        public void Update()
        {
            Center += XNACS1Base.GamePad.ThumbSticks.Left;
            XNACS1Base.World.ClampAtWorldBound(this);
        }
    }
}
