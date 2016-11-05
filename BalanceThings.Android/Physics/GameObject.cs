using System;
using BalanceThings.Drawing;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;

namespace BalanceThings.Physics
{
    class GameObject : IUpdatable, Drawing.IDrawable
    {
        private Body _body;
        private Sprite _sprite;

        internal GameObject(World world, Body body, Sprite sprite)
        {
            _body = body;
            _sprite = sprite;
        }

        internal GameObject(World world, Sprite sprite, float density)
            : this(world, BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(sprite.Collider.Width),
                ConvertUnits.ToSimUnits(sprite.Collider.Height), density, ConvertUnits.ToSimUnits(sprite.Position)), sprite) { }

        public void Update(GameTime gameTime)
        {
            _sprite.Position = ConvertUnits.ToDisplayUnits(_body.Position);
            _sprite.Rotation = _body.Rotation;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _sprite.Draw(gameTime, spriteBatch);
        }

        internal Body Body { get { return _body; } set { _body = value; } }
        internal Sprite Sprite { get { return _sprite; } }
        internal Vector2 Position
        {
            get { return _sprite.Position; }
            set
            {
                _sprite.Position = value;
                _body.Position = ConvertUnits.ToSimUnits(value);
            }
        }
    }
}