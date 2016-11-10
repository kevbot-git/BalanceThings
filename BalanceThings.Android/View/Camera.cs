using System;
using BalanceThings.Drawing;
using BalanceThings.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BalanceThings.View
{
    internal class Camera
    {
        private static float DEFAULT_EASE = 10f;

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

        internal void EaseTo(float zoom, Vector2 position, float rate)
        {
            rate = (Math.Abs(rate) > 1f) ? Math.Abs(rate): 1.001f;
            Zoom += (zoom - Zoom) / rate;
            Position += ((position - Position) / new Vector2(rate, rate));
        }

        internal void EaseTo(float zoom)
        {
            EaseTo(zoom, Position, DEFAULT_EASE);
        }

        internal void EaseTo(Vector2 position)
        {
            EaseTo(Zoom, position, DEFAULT_EASE);
        }

        internal void Follow(GameObject gameObject)
        {
            if (gameObject != null)
                Position = gameObject.Position;
        }

        internal Matrix Transform { get { return _transformMatrix; } }

        internal Rectangle Viewport
        {
            get { return new Rectangle((int) (_viewport.Bounds.X / _zoom), (int) (_viewport.Bounds.Y / _zoom),
                (int) (_viewport.Bounds.Width / _zoom), (int) (_viewport.Bounds.Height / _zoom)); }
        }

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