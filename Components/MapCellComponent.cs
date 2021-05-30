using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonoLifeUltimate.Components
{
    public class MapCellComponent
    {
        public Vector2 MapCoord;
        public Vector2 WorldCoord;
        public bool IsWall;
        public bool IsExplored;
    }
}
