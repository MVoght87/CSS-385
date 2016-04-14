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
    public class SoccerTarget
    {
        XNACS1Circle m_Target = null;

        public SoccerTarget()
        {
        }

        public void CreateTarget()
        {
            m_Target = new XNACS1Circle(new Vector2(30f, 30f), 2f, "SoccerBall");
        }

        public void UpdateTarget(Vector2 delta)
        {
            if (null != m_Target)
            {
                m_Target.CenterX += 0.5f;
                m_Target.Center += delta;
            }
        }

        public void DestroyTarget()
        {
            m_Target.RemoveFromAutoDrawSet();
            m_Target = null;
        }
        
    }
}
