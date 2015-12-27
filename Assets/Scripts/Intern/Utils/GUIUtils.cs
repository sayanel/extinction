using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Extinction
{
    namespace Utils
    {
        /// <summary>
        /// This class provides functions to draw a rectangle box on the screen.
        /// </summary>
        public class GUIUtils
        {
            /// <summary>
            /// simple 1*1 pixel texture with white color
            /// </summary>
            private static Texture2D _whiteTexture;
            public static Texture2D WhiteTexture
            {
                get
                {
                    if( _whiteTexture == null )
                    {
                        _whiteTexture = new Texture2D( 1, 1 );
                        _whiteTexture.SetPixel( 0, 0, Color.white );
                        _whiteTexture.Apply();
                    }

                    return _whiteTexture;
                }
            }

            /// <summary>
            /// draw a rectangle on screen
            /// </summary>
            public static void DrawScreenRect( Rect rect, Color color )
            {
                Color lastGUIColor = GUI.color; 
                GUI.color = color;
                GUI.DrawTexture( rect, WhiteTexture );
                GUI.color = lastGUIColor;
            }

            /// <summary>
            /// draw 4 rectangles to make a rectangle box on screen
            /// </summary>
            public static void DrawScreenRectBorder( Rect rect, float thickness, Color color )
            {
                // Top
                DrawScreenRect( new Rect( rect.xMin, rect.yMin, rect.width, thickness ), color );
                // Left
                DrawScreenRect( new Rect( rect.xMin, rect.yMin, thickness, rect.height ), color );
                // Right
                DrawScreenRect( new Rect( rect.xMax - thickness, rect.yMin, thickness, rect.height ), color );
                // Bottom
                DrawScreenRect( new Rect( rect.xMin, rect.yMax - thickness, rect.width, thickness ), color );
            }
        }
    }
}
