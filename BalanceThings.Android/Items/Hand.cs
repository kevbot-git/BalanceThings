using BalanceThings.Drawing;
using BalanceThings.Physics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BalanceThings.Items
{
    class Hand : GameObject
    {
        internal Hand(World world, ContentManager contentManager, Vector2 position)
            : base(world, new Sprite(contentManager.Load<Texture2D>("hand_default"), new Rectangle(6, 1, 2, 6), position, 16f, 0f), 1f)
        {
            Body.IsStatic = true;
        }


    }
}