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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using XNACS1Lib;


namespace ClassExample
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ClassExample : XNACS1Base
    {
        private const float kWorldWidth = 100f;

        // 
        private List<PatrolObject> mPatrols;
        private HeroObject mHero;


        protected override void InitializeWorld()
        {
            World.SetWorldCoordinate(new Vector2(0, 0), kWorldWidth);

            mPatrols = new List<PatrolObject>();
            mPatrols.Add(new PatrolObject());

            mHero = new HeroObject();
        }

        protected override void UpdateWorld()
        {
            if (GamePad.ButtonBackClicked())
                Exit();

            if (GamePad.ButtonAClicked())
                mPatrols.Add(new PatrolObject());

            if (GamePad.ButtonBClicked())
            {
                if (mPatrols.Count > 0)
                {
                    PatrolObject p = mPatrols[0];
                    p.RemoveFromAutoDrawSet();
                    mPatrols.RemoveAt(0);
                }
            }

            int numChase = 0;

            for (int i = 0; i<mPatrols.Count; i++)
            {
                PatrolObject p = mPatrols[i];

                if (p.ChasingHero())
                    numChase++;

                if (p.Update(mHero))
                {
                    mPatrols.Remove(p);
                    p.RemoveFromAutoDrawSet();
                }
            }


            mHero.Update(GamePad.ThumbSticks.Right);

            EchoToTopStatus("A to add a new patrol, B to remove a patrol");
            EchoToBottomStatus("Currently NumPatrol=" + mPatrols.Count + " NumChasing=" + numChase + "    Hero caught=" + mHero.NumTimesCaught);
        }
    }
}
