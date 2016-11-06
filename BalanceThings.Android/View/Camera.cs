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
        private Vector2 _position;
        private float _baseZoom;
        private float _zoom;

        private Camera(GraphicsDevice graphicsDevice, float scaledZoom, float startZoom)
        {
            _viewport = graphicsDevice.Viewport;
            _baseZoom = scaledZoom;
            Zoom = startZoom;
        }

        internal Camera(Core.Game game, Vector2 position, float startZoom)
            : this(game.GraphicsDevice, game.SCALED_ZOOM, startZoom)
        {
            _position = position;
            _transformMatrix = setTransform();
        }

        private Matrix setTransform()
        {
            Vector2 position = (_viewport.Bounds.Center.ToVector2() - Position * new Vector2(_zoom, _zoom));
            return Matrix.CreateScale(new Vector3(_zoom, _zoom, 0)) *
                Matrix.CreateTranslation(new Vector3(position.X, position.Y, 0));
        }

        internal void Follow(GameObject gameObject)
        {
            if (gameObject != null)
                Position = gameObject.Position;
        }

        internal Matrix Transform { get { return _transformMatrix; } }
        internal float Zoom
        {
            get { return _zoom / _baseZoom; }
            set
            {
                if (value > 0f && value < 1000f)
                {
                    _zoom = value * _baseZoom;
                    _transformMatrix = setTransform();
                }
            }
        }

        internal Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                _transformMatrix = setTransform();
            }
        }
    }
}