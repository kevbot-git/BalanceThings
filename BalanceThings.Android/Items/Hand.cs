using BalanceThings.Drawing;
using BalanceThings.Physics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BalanceThings.Items
{
    class Hand : GameObject
    {
        internal Hand(World world, Texture2D texture, Vector2 position)
            : base(world, new Sprite(texture, new Rectangle(6, 1, 2, 6), position, new Point(1, 3), 1f, 0f), 32f)
        {
            Body.IsStatic = true;
        }


    }
}