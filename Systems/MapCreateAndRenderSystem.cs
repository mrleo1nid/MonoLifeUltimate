using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Screens;
using MonoGame.Extended.ViewportAdapters;
using MonoLifeUltimate.Components;
using RogueSharp;
using RogueSharp.MapCreation;

namespace MonoLifeUltimate.Systems
{
    public class MapCreateAndRenderSystem : EntityDrawSystem
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly SpriteBatch _spriteBatch;
        private IMap map;
        private ComponentMapper<MapCellComponent> _mapCellMapper;
        private ComponentMapper<Texture2D> _textureMapper;
        private Texture2D _floor;
        private Texture2D _wall;
        private GameScreen _screen;
        private OrthographicCamera _camera;

        public MapCreateAndRenderSystem(GraphicsDevice graphicsDevice, GameScreen screen, OrthographicCamera camera)
            : base(Aspect.All( typeof(MapCellComponent), typeof(Texture2D)))
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(graphicsDevice);
            _screen = screen;
            _camera = camera;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
         
            _mapCellMapper = mapperService.GetMapper<MapCellComponent>();
            _textureMapper = mapperService.GetMapper<Texture2D>();
            IMapCreationStrategy<Map> mapCreationStrategy = new RandomRoomsMapCreationStrategy<Map>(Settings.MapWidth, Settings.MapHeight, 100, 7, 3);
            map = RogueSharp.Map.Create(mapCreationStrategy);
            _floor = _screen.Content.Load<Texture2D>("floor");
            _wall = _screen.Content.Load<Texture2D>("wall");
            foreach (Cell cell in map.GetAllCells())
            {
                var position = new Vector2(cell.X * Settings.SpriteWidth, cell.Y * Settings.SpriteHeight);
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

        public override void Draw(GameTime gameTime)
        {
            _graphicsDevice.Clear(Color.DarkBlue * 0.2f);
            var transformMatrix = _camera.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrix, samplerState: SamplerState.PointClamp);

            foreach (var entity in ActiveEntities)
            {
                var mapCell = _mapCellMapper.Get(entity);
                var texture = _textureMapper.Get(entity);
                _spriteBatch.Draw(texture, mapCell.WorldCoord, Color.White);
            }
            _spriteBatch.End();
        }
        private int CreateMapCell(Vector2 position, Vector2 mapCoord, bool isWall, Texture2D texture)
        {
            var entity = CreateEntity();
            entity.Attach(new MapCellComponent() { MapCoord = mapCoord, WorldCoord = position, IsExplored = false, IsWall = isWall });
            entity.Attach(texture);
            return entity.Id;
        }
    }
}
