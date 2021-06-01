using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace MonoLifeUltimate.Components
{
    public class Pawn : IEntity
    {
        private readonly LifeGame _game;
        public IShapeF Bounds { get; }
        public Vector2 MapCoord;
        public Vector2 Coordinate;

        public Pawn(LifeGame game, RectangleF rectangleF, Vector2 mapCoord, Vector2 position)
        {
            _game = game;
            Bounds = rectangleF;
            MapCoord = mapCoord;
            Coordinate = position;
        }

        public virtual void Draw(SpriteBatch spriteBatch) { }

        public virtual void Update(GameTime gameTime) { }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
           
        }

    }
}
