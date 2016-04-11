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


        protected override void InitializeWorld()
        {
            World.SetWorldCoordinate(new Vector2(0, 0), kWorldWidth);

            mPatrols = new List<PatrolObject>();
            mPatrols.Add(new PatrolObject());
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

            foreach (PatrolObject p in mPatrols)
                p.Update();

            EchoToTopStatus("A to add a new patrol, B to remove a patrol");
            EchoToBottomStatus("Currently NumPatrol=" + mPatrols.Count);
        }
    }
}
