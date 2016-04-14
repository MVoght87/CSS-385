#region Reference to system libraries
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
#endregion

// Reference to XNACSLib
using XNACS1Lib;

namespace XNACS1Lib_PrimitiveSet
{
 
    public class Game1 : XNACS1Base
    {
        #region Instance Variables
        
            const float SPEED = 0.5f; // speed of the soccer

            // Instance variables: two circle and two rectangles
            XNACS1Rectangle m_RotatingLadder; // A. ladder: shows primitive rotation.

            XNACS1Circle m_SoccerBall;        // B. soccer: shows AutoRedrawSet membership and world bound bouncing 

            XNACS1Circle m_RightThumbCircle;  // C. right thumbStick circle: shows world bound clamping

            XNACS1Rectangle m_Eraser;         // D. Eraser rectangle: shows relative position            

            XNACS1PrimitiveSet m_VisibleSet;  // E. Button-X click makes this set invisible
            bool m_SetIsVisible;              //    Record if the set is currently visible
        #endregion 

        protected override void  InitializeWorld()
        {
            World.SetWorldCoordinate(new Vector2(0.0f, 0.0f), 100.0f);     // Set coordinate system 
            World.SetBackgroundColor(Color.Aquamarine);   // Set the background color

            // A. Create the initialize the rotating ladder
            Vector2 aPos = new Vector2(50.0f, 50.0f);
            Vector2 bPos = new Vector2(75.0f, 5.0f);
            m_RotatingLadder = new XNACS1Rectangle(aPos, bPos, 3.0f, "Ladder");
            m_RotatingLadder.Label = "Rotating Ladder";
            m_RotatingLadder.LabelColor = Color.White;

            // B. Create and initialize the bouncing soccer ball
            m_SoccerBall = new XNACS1Circle(new Vector2(20.0f, 20.0f), 2.0f, "SoccerBall");
            m_SoccerBall.VelocityDirection = new Vector2(5.0f, 3.0f);
            m_SoccerBall.Speed = SPEED;
            m_SoccerBall.ShouldTravel = true;

            // C. Create and initialize the cicle controlled by the right thumb stick
            m_RightThumbCircle = new XNACS1Circle(new Vector2(80.0f, 30.0f), 5.0f);
            m_RightThumbCircle.Color = Color.Pink;
            m_RightThumbCircle.Label = "Controlled BY\n Right Thumb Stick";

            // D. Create and initialize the eraser Rectangle
            Vector2 pos = (World.WorldMax + World.WorldMin) / 2.0f;
            m_Eraser = new XNACS1Rectangle(new Vector2(5.0f, pos.Y), 3.0f, 65.0f);
            m_Eraser.Color = Color.Red;
            m_Eraser.Label = "Hide Soccer:\n Left Thumb Stick";            
        
            // E. Insert: RightThumbCircle, and Ladder into the m_VisibleSet
            m_VisibleSet = new XNACS1PrimitiveSet();
            m_VisibleSet.AddToSet(m_RightThumbCircle);
            m_VisibleSet.AddToSet(m_RotatingLadder);
            m_SetIsVisible = true;
        }
       
        protected override void UpdateWorld()
        {
            if (GamePad.ButtonBackClicked())
                this.Exit();

            #region  A. Update the Rotating ladder
                m_RotatingLadder.RotateAngle = (m_RotatingLadder.RotateAngle + 0.5f) % 360;
                if (GamePad.ButtonBClicked())
                    m_RotatingLadder.TopOfAutoDrawSet();
            #endregion 

            #region B. Update the soccer ball, make sure it bounces off the window bounds
                // Soccer will move because its ShouldTravel is true and has a non-zero velocity
                BoundCollideStatus collideStatus = World.CollideWorldBound(m_SoccerBall);
                switch (collideStatus)
                {
                    case BoundCollideStatus.CollideTop:
                    case BoundCollideStatus.CollideBottom:
                        World.ClampAtWorldBound(m_SoccerBall);
                        m_SoccerBall.VelocityY = -m_SoccerBall.VelocityY;
                        break;
                    case BoundCollideStatus.CollideLeft:
                    case BoundCollideStatus.CollideRight:
                        World.ClampAtWorldBound(m_SoccerBall);
                        m_SoccerBall.VelocityX = -m_SoccerBall.VelocityX;
                        break;
                }
            #endregion 

            #region C. Update the right thumb circle (clamp to the window bounds)
                m_RightThumbCircle.Center = m_RightThumbCircle.Center + GamePad.ThumbSticks.Right;
                World.ClampAtWorldBound(m_RightThumbCircle);
                if (GamePad.ButtonAClicked())
                    m_RightThumbCircle.TopOfAutoDrawSet();
            #endregion 

            #region D. Update the eraser, hide/show soccer ball based on relative position
                m_Eraser.CenterX += GamePad.ThumbSticks.Left.X;
                if (m_Eraser.RightOf(m_SoccerBall))
                {
                    if (m_SoccerBall.IsInAutoDrawSet())
                    {
                        m_SoccerBall.RemoveFromAutoDrawSet();
                    }
                }
                else
                {
                    if (!m_SoccerBall.IsInAutoDrawSet())
                    {
                        m_SoccerBall.AddToAutoDrawSet();
                    }
                }
            #endregion 

            #region E. toggle VisibleSet by the X-Button
                if (GamePad.ButtonXClicked())
                {
                    if (m_SetIsVisible)
                        m_VisibleSet.RemoveAllFromAutoDrawSet();
                    else
                        m_VisibleSet.AddAllToAutoDraw();
                    m_SetIsVisible = !m_SetIsVisible;
                }
            #endregion 

            #region F. Collide the soccer with the rotating ladder
                bool collided = m_SoccerBall.Collided(m_RotatingLadder);
                if (collided)
                {
                    m_SoccerBall.VelocityDirection = m_SoccerBall.Center - m_RotatingLadder.Center;
                    m_SoccerBall.Speed = SPEED * RandomFloat(0.5f, 2.0f);
                }
                if (GamePad.ButtonYClicked())
                {
                    m_SoccerBall.Center = m_RotatingLadder.Center;
                    m_SoccerBall.VelocityDirection = m_RotatingLadder.Velocity;
                }
            #endregion 
                        
            EchoToTopStatus("Soccer Position: " + m_SoccerBall.Center);
            EchoToBottomStatus("A-Raise Ladder, B-Raise Soccer, X-Hide Ladder, LeftThumbStick: move eraser to hide soccer");
        }
        
    }
}
