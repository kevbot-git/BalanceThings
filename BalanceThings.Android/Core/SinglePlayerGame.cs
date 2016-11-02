using System;
using BalanceThings.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BalanceThings.Core
{
    class SinglePlayerGame : Game
    {
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
                    System.Threading.Thread.Sleep(200);
                }));
            }

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
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}