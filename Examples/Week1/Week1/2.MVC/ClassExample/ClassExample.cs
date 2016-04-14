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

namespace ClassExample
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ClassExample : XNACS1Base
    {
        SoccerTarget mMyTarget; // This is my Model

        protected override void InitializeWorld()
        {
            /// Create and initalize the model
            mMyTarget = new SoccerTarget();
        }


        /// <summary>
        /// This is the "Controller" in the MVC software architecture.
        /// In this function, we accept user input and forward appropriate user actions
        /// to our model.
        /// </summary>
        protected override void UpdateWorld()
        {
            if (GamePad.ButtonBackClicked())
                Exit();

            /// Forward user input to our model
            if (GamePad.ButtonAClicked())
                mMyTarget.CreateTarget();

            if (GamePad.ButtonBClicked())
                mMyTarget.DestroyTarget();

            mMyTarget.UpdateTarget(GamePad.ThumbSticks.Right);
        }
        
    }
}
