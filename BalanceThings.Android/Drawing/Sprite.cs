using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BalanceThings.Drawing
{
    class Sprite : IDrawable, IUpdatable
    {
        internal Sprite(Texture2D texture, Rectangle? collider, Vector2 position, float scale, float rotation)
        {
            Texture = texture;
            if (collider.HasValue)
                Collider = collider.Value;
            else
                Collider = texture.Bounds;

            Origin = Collider.Center.ToVector2();
            Position = position;
            Scale = scale;
            Rotation = rotation;
        }

        internal Sprite(Texture2D texture, Rectangle? collider, Vector2 position)
            : this(texture, collider, position, 1f, 0f) { }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, Rotation, Origin, Scale, SpriteEffects.None, 1f);
        }

        internal Texture2D Texture { get; private set; }
        internal Rectangle Collider { get; private set; }
        internal Vector2 Position { get; set; }
        internal Vector2 Origin { get; private set; }
        internal float Scale { get; set; }
        internal float Rotation { get; set; }
    }
}