using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Screens;
using MonoGame.Extended.ViewportAdapters;
using MonoLifeUltimate.Components;
using MonoLifeUltimate.Systems;

namespace MonoLifeUltimate.Scenes
{
    public class SceneInGame : GameScreen
    {
        private new LifeGame Game => (LifeGame)base.Game;

        private Texture2D _logo;
        private SpriteFont _font;
        private Vector2 _position = new Vector2(50, 50);
        private World _world;
        private OrthographicCamera _camera;
        public SceneInGame(LifeGame game) : base(game) { }

        public override void LoadContent()
        {
            var viewportAdapter = new BoxingViewportAdapter(Game.Window, GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            _camera = new OrthographicCamera(viewportAdapter);
            _world = new WorldBuilder()
                .AddSystem(new MapCreateAndRenderSystem(GraphicsDevice, this, _camera))
                .Build();
            Game.Components.Add(_world);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _camera.Move(GetMovementDirection() * Settings.CameraSpeed * gameTime.GetElapsedSeconds());
            _camera.Zoom = GetZoomDirection();
            _position = Vector2.Lerp(_position, Mouse.GetState().Position.ToVector2(), 1f * gameTime.GetElapsedSeconds());
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(new Color(25, 139, 204));
        }
        private Vector2 GetMovementDirection()
        {
            var movementDirection = Vector2.Zero;
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.S))
            {
                movementDirection += Vector2.UnitY;
            }
            if (state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W))
            {
                movementDirection -= Vector2.UnitY;
            }
            if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A))
            {
                movementDirection -= Vector2.UnitX;
            }
            if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
            {
                movementDirection += Vector2.UnitX;
            }
            return movementDirection;
        }

        private float GetZoomDirection()
        {
            var currentdir = _camera.Zoom;
            var state = Keyboard.GetState();
            var mousestate = Mouse.GetState();
            if (state.IsKeyDown(Keys.OemPlus))
            {
                currentdir += Settings.CameraZoomSpeed;
            }
            if (state.IsKeyDown(Keys.OemMinus))
            {
                currentdir -= Settings.CameraZoomSpeed;
            }

            if (currentdir>_camera.MaximumZoom)
            {
                return _camera.MaximumZoom;
            }

            if (currentdir<_camera.MinimumZoom)
            {
                return _camera.MinimumZoom;
            }
            return currentdir;
        }
    }
}
