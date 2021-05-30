using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Screens;

namespace MonoLifeUltimate.Scenes
{
    public class SceneMainMenu : GameScreen
    {
        private new LifeGame Game => (LifeGame)base.Game;

        private Texture2D _logo;
        private SpriteFont _font;
        private Vector2 _position = new Vector2(50, 50);
        public SceneMainMenu(LifeGame game) : base(game) { }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _position = Vector2.Lerp(_position, Mouse.GetState().Position.ToVector2(), 1f * gameTime.GetElapsedSeconds());
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(new Color(64, 139, 204));
        }
    }
}
