using BalanceThings.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using BalanceThings.Physics;
using BalanceThings.Drawing;
using BalanceThings.Items;
using FarseerPhysics;

namespace BalanceThings.Core
{
    class SinglePlayerGame : Game
    {
        private World _world;

        private GameObject _hand;

        public SinglePlayerGame(AccelerometerHandlerDelegate accelerometerHandler)
            : base(accelerometerHandler) { }

        protected override void Init()
        {
            //
        }

        // My alternative to LoadContent; makes progress bars easier
        protected override ContentManager Load()
        {
            ContentManager c = new ContentManager();

            for (int i = 1; i <= 10; i++)
            {
                c.AddTask(new LoadTask("test" + i, delegate ()
                {
                    System.Threading.Thread.Sleep(100);
                }));
            }

            c.AddTask(new LoadTask("physics", delegate ()
            {
                ConvertUnits.SetDisplayUnitToSimUnitRatio(1f);
                _world = new World(new Vector2(0, 9.98f));

                _hand = new Hand(_world, Content, Vector2.Zero);//GraphicsDevice.Viewport.Bounds.Center.ToVector2());
            }));

            return c;
        }

        protected override void OnLoaded()
        {

            base.OnLoaded();
        }

        protected override void Restart()
        {
            base.Restart();
        }

        protected override void Update(GameTime gameTime)
        {
            switch(_currentGameState)
            {
                case GameState.PLAYING:
                    _world.Step((float) 1 / 60);

                    _hand.Update(gameTime);

                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Background);

            switch(_currentGameState)
            {
                case GameState.PLAYING:

                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, null);

                    _hand.Draw(gameTime, spriteBatch);

                    spriteBatch.End();

                    break;
            }

            base.Draw(gameTime);
        }
    }
}