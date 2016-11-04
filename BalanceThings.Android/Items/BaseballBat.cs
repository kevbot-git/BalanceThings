using BalanceThings.Drawing;
using BalanceThings.Physics;
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.TextureTools;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace BalanceThings.Items
{
    class BaseballBat : GameObject
    {
        internal BaseballBat(World world, ContentManager contentManager, Vector2 position)
            : base(world, new Sprite(contentManager.Load<Texture2D>("baseball_bat"), new Rectangle(5, 0, 6, 64), position, 1f, 0f), 1f)
        {
            Body.BodyType = BodyType.Dynamic;
            Body.Friction = 1f;
            Body.Restitution = 0.4f;
            Body.AngularDamping = 0f;
        }

        private static Body createBody(Sprite sprite, World world)
        {
            uint[] texData = new uint[sprite.Texture.Width * sprite.Texture.Height];

            sprite.Texture.GetData(texData);

            //Find the vertices that makes up the outline of the shape in the texture
            Vertices verts = PolygonTools.CreatePolygon(texData, sprite.Texture.Width, false);

            //For now we need to scale the vertices (result is in pixels, we use meters)
            Vector2 scale = new Vector2(ConvertUnits.ToSimUnits(1));
            verts.Scale(ref scale);

            //Since it is a concave polygon, we need to partition it into several smaller convex polygons
            List<Vertices> vertexList = Triangulate.ConvexPartition(verts, TriangulationAlgorithm.Bayazit);

            return BodyFactory.CreateCompoundPolygon(world, vertexList, 1f);
        }

    }
}