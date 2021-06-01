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
    public class MapRenderSystem : EntityDrawSystem
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

        public MapRenderSystem(GraphicsDevice graphicsDevice, GameScreen screen, OrthographicCamera camera, IMap map)
            : base(Aspect.All( typeof(MapCellComponent), typeof(Texture2D)))
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(graphicsDevice);
            _screen = screen;
            _camera = camera;
            this.map = map;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
         
            _mapCellMapper = mapperService.GetMapper<MapCellComponent>();
            _textureMapper = mapperService.GetMapper<Texture2D>();
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
                //_spriteBatch.Draw(texture, mapCell.WorldCoord, texture.Bounds, Color.White, 0, Vector2.Zero, Settings.GetScale(texture), SpriteEffects.None, 0);
            }
            _spriteBatch.End();
        }

    }
}
