using BalanceThings.Drawing;
using BalanceThings.Physics;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BalanceThings.Items
{
    class Hand : GameObject
    {
        internal Hand(World world, ContentManager contentManager, Vector2 position)
            : base(BodyFactory.CreateEllipse(world, ConvertUnits.ToSimUnits(1f), ConvertUnits.ToSimUnits(3f), 8, 1f,
                ConvertUnits.ToSimUnits(position + new Vector2(6, 1))),
                  new Sprite(contentManager.Load<Texture2D>("img/hand_default"), new Rectangle(6, 1, 2, 6), position, 1f, 0f))
        {
            Body.BodyType = BodyType.Static;
            Body.IsKinematic = true;
            Body.Restitution = 0.0f;
            Body.Friction = 1f;
        }
    }
}