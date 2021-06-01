using RogueSharp.Random;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoLifeUltimate.Components
{
    public static class Settings
    {
        public static readonly int MapWidth = 64;
        public static readonly int MapHeight = 64;
        public static readonly int SpriteSize = 16;
        public static readonly float CameraSpeed = 400;
        public static readonly float CameraZoomSpeed = 0.01f;
        public static readonly int MapFillProb = 50;
        public static readonly int MapFillIteration = 10;
        public static readonly int MapCutOfBigAreaFill = 10;
        public static Vector2 GetWorldPosition(Vector2 mappos)
        {
            return new Vector2(mappos.X * Settings.SpriteSize, mappos.Y * Settings.SpriteSize);
        }
        public static Vector2 GetMapPosition(Vector2 worldpos)
        {
            return new Vector2(worldpos.X / Settings.SpriteSize, worldpos.Y / Settings.SpriteSize);
        }
        public static Vector2 GetScale(Texture2D texture)
        {
            return new Vector2(-texture.Width/SpriteSize, -texture.Height / SpriteSize);
        }

    }
}
