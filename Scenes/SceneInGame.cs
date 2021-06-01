using System;
using System.Collections.Generic;
using System.Linq;
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
using RogueSharp;
using RogueSharp.MapCreation;

namespace MonoLifeUltimate.Scenes
{
    public class SceneInGame : GameScreen
    {
        private new LifeGame Game => (LifeGame)base.Game;

        private Vector2 _position = new Vector2(50, 50);
        private World _world;
        public readonly Random Random = new Random(Guid.NewGuid().GetHashCode());
        private OrthographicCamera _camera;
        public SceneInGame(LifeGame game) : base(game) { }
        public IMap Map;
        public override void LoadContent()
        {
            var viewportAdapter = new BoxingViewportAdapter(Game.Window, GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            _camera = new OrthographicCamera(viewportAdapter);
            IMapCreationStrategy<Map> mapCreationStrategy1 = new CaveMapCreationStrategy<Map>(Settings.MapWidth,
                Settings.MapHeight,
                Settings.MapFillProb,
                Settings.MapFillIteration,
                Settings.MapCutOfBigAreaFill);
            Map = RogueSharp.Map.Create(mapCreationStrategy1);
            _world = new WorldBuilder()
                .AddSystem(new MapRenderSystem(GraphicsDevice, this, _camera, Map))
                .AddSystem(new PawnRenderSystem(GraphicsDevice, this, _camera))
                .Build();
            Game.Components.Add(_world);
            CreateMap();
            CreateFirstPawns();
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

        private void CreateMap()
        {
           var  _floor = Content.Load<Texture2D>("netherrack_top");
           var _wall = Content.Load<Texture2D>("nylium");
            foreach (Cell cell in Map.GetAllCells())
            {
                var position = Settings.GetWorldPosition(new Vector2(cell.X, cell.Y));
                if (cell.IsWalkable)
                {
                    CreateMapCell(position, new Vector2(cell.X, cell.Y), false, _floor);
                }
                else
                {
                    CreateMapCell(position, new Vector2(cell.X, cell.Y), true, _wall);
                }
            }
        }
        private int CreateMapCell(Vector2 position, Vector2 mapCoord, bool isWall, Texture2D texture)
        {
            var entity = _world.CreateEntity();
            entity.Attach(new MapCellComponent() { MapCoord = mapCoord, WorldCoord = position, IsExplored = false, IsWall = isWall });
            entity.Attach(texture);
            return entity.Id;
        }
        private int CreatePawn(Vector2 position, Texture2D texture, Vector2 mapCoord)
        {
            var entity = _world.CreateEntity();
            entity.Attach(new Pawn(Game, texture.Bounds, mapCoord, position));
            entity.Attach(texture);
            return entity.Id;
        }
        private Vector2 GetRandomCell()
        {
            var cells = Map.GetAllCells().Where(x => x.IsWalkable);
            var cell = cells.ElementAtOrDefault(Random.Next(cells.Count()));
            return new Vector2(cell.X, cell.Y);
        }

        private void CreateFirstPawns()
        {
            for (int i = 0; i < 6; i++)
            {
                var cell = GetRandomCell();
                var _text = Content.Load<Texture2D>("pawn");
                var ent = CreatePawn(Settings.GetWorldPosition(new Vector2(cell.X,cell.Y)), _text, new Vector2(cell.X, cell.Y));
            }
        }

    }
}
