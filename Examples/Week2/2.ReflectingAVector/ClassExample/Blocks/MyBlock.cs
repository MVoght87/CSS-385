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
    public class MyBlock : XNACS1Rectangle
    {
         
        private const float kBlockPosX = 70f;
        private const float kBlockPosY = 0.5f * 100f * 9 / 16;
        private const float kBlockWidth = 100f;
        private const float kBlockHeight = 3f;

        private const float kBlockInitRotation = -90f;
        private const float kBlockRotation = 60f;


        public MyBlock() : base(new Vector2(kBlockPosX, kBlockPosY), kBlockWidth, kBlockHeight)
        {
            Color = Color.Blue;
        }

        public void UpdateBlock(Vector2 delta, float rotate)
        {
            Center += delta;
            RotateAngle = kBlockInitRotation + (rotate * kBlockRotation);
        }
    }
}
