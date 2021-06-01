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
    public class PawnRenderSystem : EntityDrawSystem
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly SpriteBatch _spriteBatch;
        private ComponentMapper<Pawn> _pawnMapper;
        private ComponentMapper<Texture2D> _textureMapper;
        private GameScreen _screen;
        private OrthographicCamera _camera;

        public PawnRenderSystem(GraphicsDevice graphicsDevice, GameScreen screen, OrthographicCamera camera)
            : base(Aspect.All( typeof(Pawn), typeof(Texture2D)))
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(graphicsDevice);
            _screen = screen;
            _camera = camera;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
         
            _pawnMapper = mapperService.GetMapper<Pawn>();
            _textureMapper = mapperService.GetMapper<Texture2D>();
        }

        public override void Draw(GameTime gameTime)
        {
            var transformMatrix = _camera.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrix, samplerState: SamplerState.PointClamp, sortMode:SpriteSortMode.Deferred, blendState: BlendState.AlphaBlend);

            foreach (var entity in ActiveEntities)
            {
                var pawn = _pawnMapper.Get(entity);
                var texture = _textureMapper.Get(entity);
                _spriteBatch.Draw(texture, pawn.Coordinate, Color.White);
               // _spriteBatch.Draw(texture, pawn.Coordinate, texture.Bounds, Color.White, 0, Vector2.Zero,  scale, SpriteEffects.None, 0);
            }
            _spriteBatch.End();
        }

    }
}
