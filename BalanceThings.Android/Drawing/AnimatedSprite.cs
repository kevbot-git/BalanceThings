using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BalanceThings.Drawing
{
    class AnimatedSprite : Sprite
    {
        internal AnimatedSprite(Texture2D texture, Rectangle? collider, Vector2 position, float scale, float rotation)
            : base(texture, collider, position, scale, rotation) { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }
    }
}