using RogueSharp.Random;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoLifeUltimate.Components
{
    public static class Settings
    {
        public static readonly int MapWidth = 64;
        public static readonly int MapHeight = 64;
        public static readonly int SpriteWidth = 64;
        public static readonly int SpriteHeight = 64;
        public static readonly float CameraSpeed = 400;
        public static readonly float CameraZoomSpeed = 0.01f;
        public static readonly IRandom Random = new DotNetRandom();
    }
}
