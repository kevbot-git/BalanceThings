using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BalanceThings.Drawing
{
    class AnimatedSprite : Sprite
    {
        internal AnimatedSprite(Texture2D texture, Rectangle? collider, Vector2 position, Point origin, float scale, float rotation)
            : base(texture, collider, position, origin, scale, rotation) { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }
    }
}