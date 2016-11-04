using System;
using BalanceThings.Drawing;
using BalanceThings.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BalanceThings.View
{
    internal class Camera
    {
        private Matrix _transformMatrix;
        private Viewport _viewport;
        private float _zoom;

        private Camera(GraphicsDevice graphicsDevice, float zoom)
        {
            _viewport = graphicsDevice.Viewport;
            _zoom = zoom;
        }

        internal Camera(GraphicsDevice graphicsDevice, float zoom, Vector2 position)
            : this(graphicsDevice, zoom)
        {
            Position = position;
            _transformMatrix = setTransform();
        }

        private Matrix setTransform()
        {
            Vector2 position = _viewport.Bounds.Center.ToVector2() - Position;
            return Matrix.CreateScale(new Vector3(_zoom, _zoom, 0)) *
                Matrix.CreateTranslation(new Vector3(position.X, position.Y, 0));
        }

        internal void Follow(GameObject gameObject)
        {
            if (gameObject != null)
                Position = gameObject.Sprite.Position;

            _transformMatrix = setTransform();
        }

        internal Matrix Transform { get { return _transformMatrix; } }
        internal float Zoom
        {
            get { return _zoom; }
            set { if (value > 0f && value < 1000f) _zoom = value; }
        }

        internal Vector2 Position { get; set; }
    }
}